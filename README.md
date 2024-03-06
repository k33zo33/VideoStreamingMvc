Video Content Management Web Application

This web application is designed to manage video content streaming. It consists of three main modules: a third-party integration module, an administrative module, and a public module. Each module serves a specific purpose in managing the video content service.
Features
Third Party Integration Module

    Automates video content entry.
    Implements CRUD operations for retrieving and editing video content.
    Implements CRUD operations for managing genres and tags.
    Supports Swagger interface and HTTPS.

Administrative Module

    Accessible only by administrators.
    Manages video content, genres, tags, notifications, and users.
    Provides CRUD operations for managing various entities.
    Supports soft-delete functionality and filtering.

Public Module

    Accessible to registered users.
    Allows user self-registration and login.
    Enables users to select and view video content.
    Displays user-specific information such as username and user profile.
    Implements paging and filtering for enhanced user experience.

Technologies Used

    ASP.NET Core Web API for the integration module.
    ASP.NET Core MVC for the administrative and public modules.
    JWT token authentication for user registration and login.
    Swagger for API documentation.
    AJAX for asynchronous requests and dynamic content loading.
    Entity Framework Core for data access.

Getting Started

To run the application locally, follow these steps:

    Clone the repository to your local machine.
    Open the solution file in Visual Studio.
    Restore NuGet packages and build the solution.
    Update the database connection string in appsettings.json if necessary.
    Run the database migrations to create the database schema.
    Start the application and navigate to the appropriate URL.

Usage

    Access the application using a web browser.
    Register as a new user or log in if you already have an account.
    Explore the available video content and select a video to watch.
    Use the administrative interface to manage video content, genres, tags, and users.
