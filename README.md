# WebApi for SWIFT MT799 messages created by Mario Petkov (a C#/.net/sqlite project)

## Overview

This project is a .NET-based web API that processes and saves SWIFT messages to an SQLite database. It includes functionality for uploading SWIFT messages, parsing them, and saving them into a database without using EF or Swift message parser package.

## Message to reader

I have succesfully created a webApi which receives a POST request with a MT799 message, parses it and saved to SQLite DB without EF -> each entry has an ID, Header, ApplicationHeader, TextBody, Trailer, TrailerEnd

## Installation Steps 

  ### Open a terminal or command prompt and run: 
  git clone https://github.com/Mario1515/webApi
  ### Navigate to Project Directory
  cd webApi/api
  ### Restore Project Dependencies
  dotnet restore
  ### Build the Project
  dotnet build
  ### Run the Application
  dotnet run
  ### By default, the application will start and be accessible at http://localhost:5000

## Testing the Api 

  ### Access Swagger 
  open a web browser and navigate to http://localhost:5000/swagger
  ### Send a POST Request to Upload a SWIFT Message: 
  Click on the "Choose File" button to select a file from your system and click on Execute
  ### Check the Logs,
  I used a Serilog logger and everything is being logged into a file located in the project
  ### Check the DB,
  You could directly check the DB and see the entry within the SQL folder. 
  ### Check the server response
  The response should be a status 200 with a message. 
