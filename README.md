# Easy Control API - README

This is the README for the Easy Control API project, which describes how to set up and run the application and the database using Docker Compose, as well as how to access pgAdmin for database management.

## Requirements

Make sure you have the following requirements installed on your system:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Configuration

1. Clone this repository to your development environment:

    ```bash
    git clone https://github.com/fjrbarros/easy_control_api.git

    cd easy_control_api
    ```

2. To build and start the Docker containers, run the following command at the project's root:
     ```bash
    docker-compose up -d
    ```
    This will create containers for the API,PostgreSQL database, and pgAdmin.

## Accessing the API
The API will be available at http://localhost:8000. 
<br>
you can also access `swagger` at http://localhost:8000/index.html
<br>
You can access it in your browser or use a tool like curl or Postman to make requests.

## Accessing pgAdmin
pgAdmin is a database management tool that can be accessed at http://localhost:5050. 
<br>
Use the following credentials to log in:

 - Email: pgadmin@pgadmin.com
 - Password: pgadmin123

After logging in to pgAdmin, you can set up a connection to the PostgreSQL database with the following information:

 - Host: db
 - Port: 5432
 - Database Name: easy_control_db
 - User: admin
 - Password: admin123

This will allow you to manage the database using pgAdmin.

## Running Database Migrations
To create tables in the database, you can use the following command:

inside the folder
  ```bash
  src > EasyControl.Api
  ```

  run the command

  ```bash
  dotnet ef database update
  ```

If so far everything has gone as it should, you can test the api
