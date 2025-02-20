INSERT INTO Users (userName) 
VALUES ('Alice'), ('Bob'), ('Charlie'), ('David');

INSERT INTO Tasks (doDate, doTime, repeats, title, summary, done, doneDate, doneTime) 
VALUES 
('2025-02-20', '10:00:00', 'Daily', 'Morning Workout', 'Exercise for 30 minutes', 0, NULL, NULL),
('2025-02-21', '14:00:00', 'None', 'Project Meeting', 'Discuss progress with team', 0, NULL, NULL),
('2025-02-22', '09:30:00', 'Weekly', 'Grocery Shopping', 'Buy essentials for the week', 0, NULL, NULL),
('2025-02-23', '16:00:00', 'Monthly', 'Car Maintenance', 'Oil change and tire check', 0, NULL, NULL);

INSERT INTO TaskAssignments (taskId, userId) 
VALUES 
(1, 1), 
(1, 2),  
(2, 3),  
(2, 4),  
(3, 1),  
(3, 3),  
(4, 2),  
(4, 4);  