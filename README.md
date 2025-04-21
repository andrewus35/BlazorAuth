# BlazorAuth
A Blazor project template on .NET 9 with simplified authentication and authorization without compromising security. Designed to easily create lightweight admin portals for applications. 

Features a streamlined approach without EF Core dependencies, external registration pages, or email sending functionality. 

Requires only configuring your custom user storage and password management. See 
For testing purposes, includes hardcoded credentials: login "admin" with hashed password "admin123".
Look at CustomUserStore.cs file, there empty methods for create, get and update accounts. User Manager from Asp Net Core.Identity call these methods.
