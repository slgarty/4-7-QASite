# 4-7-QASite

This application is a forum where users can post questions and other users can view all the questions and post answers. It has advanced functionality that allows users to ‘like’ or comment on questions as well. 

*Note: This application was built as a clone stackoverflow.com*

This project focuses on many-to-many database relationships with Entity Framework Core and cookie authentication. It uses C# ASP.NET, MVC, Entity Framework Core, LINQ to SQL, HTML Razor,  JQuery, and Ajax. 


## To Run this Project: 
-	Clone the github repository and save it to your local device
-	Use the command line to navigate to the file location
-	Run the following prompts on the command line to set up the database
```sh
dotnet ef migrations add initial
dotnet ef database update
```
- 	Run the following prompts on the command line to build and run the project
```sh
dotnet watch run
```
