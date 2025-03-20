
# Done & Dusted, Task management

This is a task management website created to keep track of either repeating tasks or one offs. Currently any registered user can assign tasks to any other users in the database as well as themselves.  The website features a homepage for upcoming tasks, an add task page, repeating tasks page and completed tasks page all controlled by a nav bar on the top.

The site will check if the user was logged in within 1 hour and either send them straight to the homepage or login page. This is achieved using jwt. There is also a sign up option for new users.

If a user adds a task and selects one of the repeating options, a new task will be automatically created and assigned to the correct date when the previous one is either completed or missed.

In the completed page you can see the 6 most previous completed tasks and 6 missed tasks.
The project as a whole was made to practice using technologies like bootstrap, vue and jwt.


## Installation

Clone the repository. Open the project in VS Code. Then run:

```bash
  npm install my-project
  cd my-project
```
    

create readme here
https://readme.so/editor