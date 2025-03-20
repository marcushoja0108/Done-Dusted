![Done & Dusted Logo](Assets/largeLogo.png)
# Done & Dusted, Task management

Done & Dusted is a task management website designed for tracking both recurring and one-off tasks. Users can assign tasks to themselves or others, with a homepage displaying upcoming tasks and additional pages for adding, tracking repeating tasks, and viewing completed ones. The site uses JWT for authentication, keeping users logged in for 60 minutes before requiring re-authentication.
The project as a whole was made to practice using technologies like bootstrap, vue and jwt.



## Installation

### 1. Clone the repository. 
Open a terminal and run: 

```bash
git clone https://github.com/marcushoja0108/Done-Dusted.git
cd Done-Dusted
```
### 2. Set up backend
Restore dependencies
```bash
cd TasksApi
dotnet restore
```
Build and run
```bash
dotnet build
dotnet run
```
### 3. Set up frontend
Install dependencies
```bash
cd dd-frontend
npm install
```
Start dev server
```bash
npm run dev
```

## API Endpoints
Description of endpoints and function
### **Tasks**
| Method  | Endpoint  | Description  |
|---------|----------|--------------|
| `GET`   | `/D&D/tasks` | Get all tasks |
| `GET`   | `/D&D/task/{id}` | Get a task by ID |
| `GET`   | `/D&D/tasks/{id}` | Get tasks assigned to a user |
| `GET`   | `/D&D/tasks/upcoming/{id}` | Get upcoming tasks for a user |
| `GET`   | `/D&D/tasks/completed/{id}` | Get completed tasks for a user |
| `GET`   | `/D&D/tasks/missed/{id}` | Get missed tasks for a user |
| `POST`  | `/D&D/tasks` | Create a new task |
| `POST`  | `/D&D/tasks/{taskId}/user/{userId}` | Assign a user to a task |
| `PUT`   | `/D&D/tasks/{id}` | Update a task |
| `PUT`   | `/D&D/tasks/done/{id}` | Mark a task as done |
| `PUT`   | `/D&D/tasks/missed/{id}` | Mark a task as missed |
| `DELETE` | `/D&D/tasks/{taskId}/user/{userId}` | Remove a user from a task |
| `DELETE` | `/D&D/tasks/{id}` | Delete a task |

### **Users**
| Method  | Endpoint  | Description  |
|---------|----------|--------------|
| `GET`   | `/D&D/users` | Get all users |
| `GET`   | `/D&D/user/{id}` | Get a user by ID |
| `GET`   | `/D&D/users/{taskId}` | Get users assigned to a task |
| `POST`  | `/D&D/users` | Create a new user |
| `PUT`   | `/D&D/user/{id}` | Update a user's password |
| `DELETE` | `/D&D/users/{id}` | Delete a user |
    
## Usage

To login to the page you can either create a new user or select one of the demo users. Passwords will also be stored hashed in the database. 

| Username  | Password  | 
|---------|----------|
| Alice   | `1111` |
| Bob     | `2222` |
| Charlie | `3333` |
| John    | `0000` |
| Marcus  | `123`  |

After you are logged in the jwt will keep you logged in for 60 minutes before you will have to do it again at refresh.

### Your first task
Start by creating your first task and set the date and time for completion, as well as selecting the relevant users. Then fill out the rest of the required fields and press the 'Add task' button. 

#### Repeating tasks
If you chose a repeating option the task will create a copy of itself to the correct repeating date when the task is either marked as done or missed.

### Editing tasks
When you click the 'Details' button on the task cards, the task modal will open. If the task is still upcoming you have the option to either edit the task or mark it as done by its respecting buttons.

### Completed and missed tasks
Here you can see your 6 last missed and completed tasks and when they were pushed. You can still check their details, but the interactivity stops there.
