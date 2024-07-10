# Setting up

1- Docker should be installed.

2- Clone the repository.

3- Open the solution, and set **docker-compose** as the startup project.

4- Build and run.

# Using the loyalty system

## Serilog

Serilog is implemented in both the IdentityServer and the API (Swagger) and offers logging for events and API calls.

## Database

The database is already set up using EFCore with migrations added. A local database is bundled with the solution for ease of use.

## API Endpoints

The API has a few endpoints. **GetUsers** will show the users that are part of the loyalty system. **Earn** is the endpoint that will increment a user's points. **CreateUser** will allow adding a new user to the system.

Requests are validated using FluentValidation. When creating a user, first and last name should not be empty, and have a maximum character limit of 20 characters. 
A user can earn a maximum of 100 points at a time. They cannot earn 0 points. These checks are done with FluentValidation.

## Authentication

Swagger will run and show available API endpoints. All endpoints require authentication.

To authenticate, click on the Authorize button at the top right corner. This will trigger an OAuth2 authentication via Duende IdentityServer.

Username is "bob" and password is "Pass123$"

Once authenticated and redirected, the endpoints can be used.

## Caching

Redis is implemented for caching. This can be seen with the **GetUserPoints** endpoint. If a user's points are not cached, they are fetched and then cached, otherwise the cached points are returned.
Any time the Earn endpoint is triggered, the cached points for that user are cleared.

## Unit test

A simple unit test is done using Xunit and Moq

## Containerization

The Web API (swagger), the IdentityServer, and Redis are containerized.

## How to use

For simplicity sake, the first API endpoint is "**GetUsers**", this will show you the list of users that are defined in the loyalty system. Copy one of the users' ID to be used.

Use the **Earn** endpoint and pass along the ID with the amount to give the user their earned points.

**GetUserPoints** will use caching when applicable.

You can also create your own user with the **CreateUser** endpoint.

