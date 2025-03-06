# Demo project
Used .NET 8.0 web API that provides endpoints for retrieving company data from free and premium third-party services. 
It uses MediatR for request handling, AutoMapper for object mapping, FluentValidation for request validation, and Polly for resilience and transient-fault handling.
The project also includes Swagger for API documentation and examples, and an in-memory database for mocking data storage.

Libraries and approaches used:

- **MediatR** for implementing the mediator pattern.
- **AutoMapper** for object-to-object mapping.
- **FluentValidation** for request validation.
- **Polly** for resilience and transient-fault handling.
- **Swashbuckle.AspNetCore** for Swagger API documentation.
- **Entity Framework Core InMemory** for mocking a database.
- **xUnit** and **Moq** for unit testing.
