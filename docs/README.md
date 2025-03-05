# MovementGroupDataRetieval

## Backend Developer Home Task - .NET API with Multi-Layered Data Storage

Objective

Develop a .NET Web API that provides a data retrieval service while using caching, file storage, and a database. The service must follow a layered architecture with good design patterns and security mechanisms.


Task Requirements
1.	 API Development
a.	 Develop a .NET Web API (ASP.NET Core).
b.	 Implement an endpoint /data/{id} to retrieve data.
c.	 If data does not exist, return a 404 response.
2.	 Data Retrieval Logic
The data should be fetched in the following order:
1. Cache (Redis or in-memory cache, stored for 10 minutes).
2. File (saved as JSON, stored for 30 minutes, filename includes expiration timestamp).
3. Database (SQL or NoSQL, based on your choice).
• If data is found in the cache, return it immediately.
• If not found in the cache, check the file storage. If found, return it and store it in the cache.
• If not found in the file, check the database. If found, return it and store it in both the file and cache.
• If not found in any location, return 404 - Not Found.





3.	 API Security
• Implement JWT authentication.
• Implement role-based authorization (Admin, User).
• Admin users should be able to insert/update data via a /data POST and PUT endpoint.
• User roles should only be able to fetch data.


4.	 Database (SQL or NoSQL)
• Design a simple table/collection to store data (Id, Value, CreatedAt).
• Use Entity Framework Core (for SQL) or MongoDB (for NoSQL).

5.	Postman Collection
• Provide a Postman collection with pre-configured requests for:
• Authentication (login and token retrieval).
• Data retrieval (GET /data/{id}).
• Data insertion (POST /data).
• Data update (PUT /data/{id}).


6.	Design Patterns
• Implement Repository Pattern for database interaction.
• Use Factory Pattern for choosing the storage method.
• Implement Decorator Pattern (optional) to handle logging or monitoring of data retrieval flow.




7.	CORS Support
• Configure the API to allow CORS for specific origins.

Bonus (Optional but Recommended)
• Add Swagger documentation.
• Implement dependency injection properly.
• Write unit tests for key components.
• Use Docker to containerize the API and Redis.
 • Use FluentValidation
• Use Automapper
• Use Polly

Submission Requirements
• Share the project as a GitHub repository.
• Include a README.md with setup instructions.
• Provide a working Postman collection.