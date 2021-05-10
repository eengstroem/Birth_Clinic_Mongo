# Birth_Clinic

The project is a C# Console Application, using a class library that holds all the logic required for the project.
Made for the DAB course by Jakob, Marius and Emil.

## Overview of the project ## 

The solution has 2 projects, the Application and Library. They hold all the Models, Factories, Context, Data-Generation and Display functionality to provide logic for the main application.

Below, a quick guide will walk you through set-up and interaction with the solution.

## How to use the solution ##
1. Clone or fork the project. You can also download as zip if you wish.
2. Open the solution in visual studio.
3. Replace the connection string inside the Service configurator located at the bottom of "Program.cs", with your own connection string.
4. Run the application, remember to set "Application" as Start-up project.
5. Using the ASCII-menu, navigate through to the desired point of information.
6. On most result pages, pressing "esc" will return you to the main window. Some return automatically.
7. Enjoy!

## Notes on the solution ##
Please note that dummy data is generated at runtime.
This can result in some births not being planned within the next 5 days, as the birthdays are generated randomly.
If this happens, delete the database and run the program again.

We have made a simple command that drops the entire database for you.
Use this command to do it quick-and-easy:  
<code>USE master;
GO  
ALTER DATABASE myDb  
SET SINGLE_USER  
WITH ROLLBACK IMMEDIATE;  
GO  
DROP DATABASE myDb;</code>
