# MovementGroupDataRetieval
## Launch requirements

To launch this project you need to have installed:
- Docker
- Docker-compose
- .NET 8.0
- Redis docker container (from docker-compose file)
- Postgres docker container (from docker-compose file)
- Postman (for docs)
- free ports as mentioned in .env file

This solution works in docker environment, so that for testing you should run this command from root solution folder:
`docker-compose -f docker-compose.yml up -d --build`
After successful launch you will be able to see the API documentation in Swagger UI by following this link:
`http://localhost:5100/swagger/index.html`
For testing with postman you can use the collection from the docs folder of the solution:
`MovementGroupDataRetievalCollection.postman_collection.json`

If it is needed, you can change environment variables it .env file and settings in appsettings.json file.
There are available environments as :
- `Development`(to launch the service in debug mode), 
- `LocalDocker`(to launch the service with other containers), 
- `Production`. 

You can change it in `ASPNETCORE_ENVIRONMENT` variable in .env file.`

The resulting solution does not contain unit tests, but it is possible to add them in the future if more time is provided.
Also the was not implemented Polly. I think there are some possible usecases for it, e.g. RateLimiter or Retry for filesystem caching.

To mention that there should be added a few more endpoints for better experience, but system is flexible for that, so that it won't be so hard and time-consuming

---
## Backend Developer Home Task - .NET API with Multi-Layered Data Storage

### *Objective*

Develop a *.NET Web API* that provides a data retrieval service while using caching, file storage, and a database. The service must follow a layered architecture with good design patterns and security mechanisms.

#### Task Requirements
##### 1. API Development
 
- Develop a .NET Web API (ASP.NET Core).
- Implement an endpoint /data/{id} to retrieve data.
- If data does not exist, return a 404 response.

##### 2. Data Retrieval Logic

The data should be fetched in the following order:
1. Cache (Redis or in-memory cache, stored for 10 minutes).
2. File (saved as JSON, stored for 30 minutes, filename includes expiration timestamp).
3. Database (SQL or NoSQL, based on your choice).

- If data is found in the cache, return it immediately.
- If not found in the cache, check the file storage. If found, return it and store it in the cache.
- If not found in the file, check the database. If found, return it and store it in both the file and cache.
- If not found in any location, return 404 - Not Found.

##### 3. API Security
- Implement JWT authentication.
- Implement role-based authorization (Admin, User).
- Admin users should be able to insert/update data via a /data POST and PUT endpoint.
- User roles should only be able to fetch data.

##### 4. Database (SQL or NoSQL)
- Design a simple table/collection to store data (Id, Value, CreatedAt).
- Use Entity Framework Core (for SQL) or MongoDB (for NoSQL).

##### 5. Postman Collection
- Provide a Postman collection with pre-configured requests for:
- Authentication (login and token retrieval).
- Data retrieval (`GET /data/{id}`).
- Data insertion (`POST /data`).
- Data update (`PUT /data/{id}`).

##### 6. Design Patterns
- Implement **Repository Pattern** for database interaction.
- Use **Factory Pattern** for choosing the storage method.
- Implement **Decorator Pattern** (optional) to handle logging or monitoring of data retrieval flow.

##### 7. CORS Support
- Configure the API to allow CORS for specific origins.

##### *Bonus (Optional but Recommended)*
- Add Swagger documentation.
- Implement dependency injection properly.
- Write unit tests for key components.
- Use Docker to containerize the API and Redis.
- Use FluentValidation
- Use Automapper
- Use Polly

#### Submission Requirements
- Share the project as a GitHub repository.
- Include a README.md with setup instructions.
- Provide a working Postman collection.
