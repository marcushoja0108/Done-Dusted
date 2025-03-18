using System.Text;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();
var connStr = "Data Source=DESKTOP-PSDS24G\\SQLEXPRESS;Initial Catalog=Done&Dusted;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
app.UseCors("AllowFrontend");


// get all tasks
app.MapGet("/D&D/tasks", async () =>
{
    var sql = "SELECT * FROM Tasks";
    await using var conn = new SqlConnection(connStr);
    var tasks = await conn.QueryAsync<Tasks>(sql);
    return Results.Ok(tasks);
});

// get all users
app.MapGet("/D&D/users", async () =>
{
    var sql = "SELECT * FROM Users";
    await using var conn = new SqlConnection(connStr);
    var users = await conn.QueryAsync<Users>(sql);
    return Results.Ok(users);
});

// get task based on task id
app.MapGet("/D&D/task/{id}", async (int id) =>
{
    var sql = "SELECT * FROM Tasks WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var task = await conn.QueryAsync(sql, new { id });
    return Results.Ok(task);
});

// get user based on id
app.MapGet("/D&D/user/{id}", async (int id) =>
{
    var sql = "SELECT * FROM Users WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var user = await conn.QueryAsync(sql, new { id });
    return Results.Ok(user);
});

// get tasks based on user id
app.MapGet("/D&D/tasks/{id}", async (int id) =>
{
    var sql = @"SELECT T.id, T.title, T.doDate, T.doTime, T.repeats, T.summary, T.done, T.doneDate, T.doneTime, T.missed 
                FROM tasks T join TaskAssignments TA ON T.id = TA.taskId WHERE TA.userId = @id";
    await using var conn = new SqlConnection(connStr);
    var myTasks = await conn.QueryAsync(sql, new { id });
    return Results.Ok(myTasks);
});

//get upcoming tasks based on user id
app.MapGet("/D&D/tasks/upcoming/{id}", async (int id) =>
{
    var sql = @"SELECT * FROM Tasks T join TaskAssignments TA ON T.id = TA.taskId 
                WHERE TA.userId = @id AND T.done IS NULL AND T.missed IS NULL";
    await using var conn = new SqlConnection(connStr);
    var myUpcomingTasks = await conn.QueryAsync(sql, new { id });
    return Results.Ok(myUpcomingTasks);
});

//get completed tasks based on id
app.MapGet("/D&D/tasks/completed/{id}", async (int id) =>
{
    var sql = @"SELECT * FROM tasks T join TaskAssignments TA ON T.id = TA.taskId WHERE TA.userId = @id AND T.done = 1";
    await using var conn = new SqlConnection(connStr);
    var myCompletedTasks = await conn.QueryAsync(sql, new { id });
    return Results.Ok(myCompletedTasks);
});

//get missed tasks based on id
app.MapGet("/D&D/tasks/missed/{id}", async (int id) =>
{
    var sql = "SELECT * FROM tasks T join TaskAssignments TA ON T.id = TA.taskId WHERE TA.userId = @id AND T.missed = 1";
    await using var conn = new SqlConnection(connStr);
    var myMissedTasks = await conn.QueryAsync(sql, new { id });
    return Results.Ok(myMissedTasks);
});

// get users based on task id
app.MapGet("/D&D/users/{taskId}", async (int taskId) =>
{
    var sql = "SELECT U.id, U.userName FROM Users U JOIN TaskAssignments TA ON U.id = TA.userId WHERE TA.taskId = @taskId";
    await using var conn = new SqlConnection(connStr);
    var assignedTaskUsers = await conn.QueryAsync(sql, new { taskId });
    return Results.Ok(assignedTaskUsers);
});

// push new task to db
app.MapPost("/D&D/tasks", async (Tasks newTask) =>
{
    Console.WriteLine($"Recieved task: {newTask.title}, {newTask.doDate}, {newTask.doTime}, {newTask.repeats}");
    var sql = @"INSERT INTO Tasks (title, doDate, doTime, repeats, summary) 
        VALUES (@title, @doDate, @doTime, @repeats, @summary);
        SELECT CAST(SCOPE_IDENTITY() as int)"; 
//Linjen over returnerer id fra den nyopprettede raden
    
    await using var conn = new SqlConnection(connStr);
    var taskId = await conn.QuerySingleAsync<int>(sql, 
        new {newTask.title, newTask.doDate, newTask.doTime, newTask.repeats, newTask.summary});
    return Results.Ok(new {id = taskId});
});

//push new TaskAssignment to db
app.MapPost("/D&D/tasks/{taskId}/user/{userId}", async (int taskId, int userId) =>
{
    var sql = "INSERT INTO TaskAssignments (taskId, userId) VALUES (@taskId, @userId)";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { taskId, userId });
    return Results.Ok(rowsAffected);
});

// push new user to db
app.MapPost("/D&D/users", async (Users newUser) =>
{
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.passwordHash);
    var sql = "INSERT INTO Users (username, PasswordHash) VALUES (@username, @passwordHash)";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { newUser.userName, passwordHash = hashedPassword });
    return Results.Ok(rowsAffected);
});

// update task based on id
app.MapPut("/D&D/tasks/{id}", async (int id, Tasks uT) =>
{
    var sql = "UPDATE Tasks SET doDate = @doDate, doTime = @doTime, repeats = @repeats, title = @title, summary = @summary WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql,
        new { id, uT.doDate, uT.doTime, uT.repeats, uT.title, uT.summary});
    return Results.Ok(rowsAffected);
});

//Helper for repeats
DateTime GetNextTask(DateTime currentDate, string repeatType)
{
    DateTime date = currentDate;
    switch (repeatType.ToLower())
    {
        case "daily":
            return date.AddDays(1);
        case "weekly":
            return date.AddDays(7);
        case "monthly":
            return date.AddMonths(1);
        default:
            return date;
    }
}

// mark task as done based on id
app.MapPut("/D&D/tasks/done/{id}", async (int id) =>
{
    var sql = "UPDATE Tasks SET done = @done, doneDate = @doneDate, doneTime = @doneTime WHERE id = @id";
    
    var norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
    var currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, norwegianTimeZone);

    string doneDate = currentDateTime.ToString("yyyy-MM-dd");
    string doneTime = currentDateTime.ToString("HH:mm:ss");
    
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { id, done = true, doneDate, doneTime });

    //deals with repeats, creates new task and assignments
    var taskSql = "SELECT repeats, doDate, doTime, title, summary FROM Tasks WHERE id = @id";
    var task = await conn.QueryFirstOrDefaultAsync<Tasks>(taskSql, new { id = id });

    if (task != null && task.repeats != "None")
    {
        DateTime nextDate = GetNextTask(task.doDate, task.repeats);

        var newTaskSql = @"INSERT INTO Tasks (title, doDate, doTime, repeats, summary)
                           VALUES (@title, @doDate, @doTime, @repeats, @summary);
                           SELECT CAST (SCOPE_IDENTITY() as int)";
        var newTaskId = await conn.QuerySingleAsync<int>(newTaskSql, new
        {
            task.title,
            doDate = nextDate,
            task.doTime,
            task.repeats,
            task.summary
        });
        
        var taskAssignmentsSql = "SELECT userId FROM TaskAssignments WHERE taskId = @taskId";
        var userIds = await conn.QueryAsync<int>(taskAssignmentsSql, new { taskId = id });

        foreach (var userId in userIds)
        {
            var insertAssignmentSql = "INSERT INTO TaskAssignments (taskId, userId) VALUES (@taskId, @userId)";
            await conn.ExecuteAsync(insertAssignmentSql, new { taskId = newTaskId, userId });
        }
    }
    
    return Results.Ok(rowsAffected);
});

//mark task as missed based on id
app.MapPut("/D&D/tasks/missed/{id}", async (int id) =>
{
    var sql = "UPDATE Tasks Set missed = 1 WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { id });

    //deals with repeats, creates new task and assignments
    var taskSql = "SELECT repeats, doDate, title, doTime, summary FROM Tasks WHERE id = @id";
    var task = await conn.QueryFirstOrDefaultAsync<Tasks>(taskSql, new { id });

    if (task != null && task.repeats != "None")
    {
        DateTime nextDate = GetNextTask(task.doDate, task.repeats);
        var newTaskSql = @"INSERT INTO Tasks (title, doDate, doTime, repeats, summary)
                           VALUES (@title, @doDate, @doTime, @repeats, @summary);
                           SELECT CAST (SCOPE_IDENTITY() as int)";

        var newTaskId = await conn.ExecuteAsync(newTaskSql, new
        {
            task.title,
            doDate = nextDate.ToString("yyyy-MM-dd"),
            task.doTime,
            task.repeats,
            task.summary
        });
        
        var assignmentsSql = "SELECT userId FROM TaskAssignments WHERE taskId = @taskId";
        var userIds = await conn.QueryAsync<int>(assignmentsSql, new { taskId = id });
        foreach (var userId in userIds)
        {
            var insertAssignmentSql = "INSERT INTO TaskAssignments (taskId, userId) VALUES (@taskId, @userId)";
            await conn.ExecuteAsync(insertAssignmentSql, new { taskId = newTaskId, userId });
        }
    }
    return Results.Ok(rowsAffected);
});


//Update password based on ID
app.MapPut("/D&D/user/{id}", async (int id, Users u) =>
{
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(u.passwordHash);
    var sql = "UPDATE Users SET passwordHash = @passwordHash WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { id, passwordHash = hashedPassword });
    return Results.Ok(rowsAffected);
});


// remove user from task
app.MapDelete("/D&D/tasks/{taskId}/user/{userId}", async (int taskId, int userId) =>
{
    var sql = "DELETE FROM TaskAssignments WHERE taskId = @taskId AND userId = @userId";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { taskId, userId });
    return Results.Ok(rowsAffected);
});

// delete task from db
app.MapDelete("/D&D/tasks/{id}", async (int id) =>
{
    var sql = "DELETE FROM Tasks WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.ExecuteAsync(sql, new { id });
    return Results.Ok(rowsAffected);
});

// delete users from db
app.MapDelete("/D&D/users/{id}", async (int id) =>
{
    var sql = "DELETE FROM Users WHERE id = @id";
    await using var conn = new SqlConnection(connStr);
    var rowsAffected = await conn.QueryAsync(sql, new { id });
    return Results.Ok(rowsAffected);
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
