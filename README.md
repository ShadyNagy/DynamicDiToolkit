[![publish to nuget](https://github.com/ShadyNagy/DynamicDiToolkit/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/ShadyNagy/DynamicDiToolkit/actions/workflows/nuget-publish.yml)
[![DynamicDiToolkit on NuGet](https://img.shields.io/nuget/v/DynamicDiToolkit?label=DynamicDiToolkit)](https://www.nuget.org/packages/DynamicDiToolkit/)
[![NuGet](https://img.shields.io/nuget/dt/DynamicDiToolkit)](https://www.nuget.org/packages/DynamicDiToolkit)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/ShadyNagy/DynamicDiToolkit/blob/main/LICENSE)
[![paypal](https://img.shields.io/badge/PayPal-tip%20me-green.svg?logo=paypal)](https://www.paypal.me/shadynagy)
[![Visit My Website](https://img.shields.io/badge/Visit-My%20Website-blue?logo=internetexplorer)](https://ShadyNagy.com)


# Dynamic DI Toolkit

## Problem Statement

In modern software development, particularly within complex projects, managing dependencies can become cumbersome and rigid, leading to challenges in maintaining, testing, and evolving the application architecture. Traditional Dependency Injection (DI) practices, while powerful, often require a static setup that does not easily adapt to changing requirements or dynamic runtime conditions. This can lead to:

- **Tight coupling** between components, making it hard to swap implementations or integrate third-party services dynamically.
- **Complex configuration** setups, where changes to the DI container require extensive manual adjustments.
- **Limited flexibility** in scenarios where dependencies need to be resolved based on runtime data or when integrating plugins and modular systems.
- **Difficulties in testing**, especially when trying to mock or replace services for integration or unit tests without affecting the overall DI setup.

Developers need a solution that brings more flexibility and dynamism to dependency injection, allowing applications to be more modular, easier to maintain, and ready to adapt to new requirements as they arise.

## The Solution: Dynamic DI Toolkit

`DynamicDiToolkit` addresses these challenges by providing a dynamic dependency injection framework designed for .NET applications. It enables developers to:

- **Dynamically register and resolve dependencies** based on runtime conditions, making the application more adaptable and modular.
- **Simplify the configuration** of the DI container, with support for automatic registration and resolution strategies that reduce boilerplate code.
- **Enhance testing capabilities**, allowing for easy swapping of implementations in tests without overhauling the DI setup, facilitating both unit and integration testing.
- **Improve maintainability and scalability** of projects, by enabling a cleaner, more flexible approach to managing dependencies.

By leveraging `DynamicDiToolkit`, developers can overcome the limitations of traditional DI mechanisms, paving the way for more dynamic, maintainable, and testable applications.

## Getting Started with DynamicDiToolkit

DynamicDiToolkit simplifies the process of dynamically registering and resolving dependencies in your .NET applications. This guide will walk you through setting up DynamicDiToolkit and using it to resolve dependencies dynamically based on runtime conditions.

### Setup in Startup.cs or Program.cs

First, ensure that DynamicDiToolkit services are registered in your application's service collection. This is typically done in the `Startup.cs` (for .NET Core 3.1 or earlier) or `Program.cs` (for .NET 5+). Add the following line to the `ConfigureServices` method:

```csharp
services.AddScopedDynamicDiToolkitServices();
```

This line of code registers DynamicDiToolkit's services with ASP.NET Core's built-in dependency injection system, enabling you to dynamically resolve services later in your application.

### Implementing a Dynamic Service Resolver

Let's consider a scenario where you have an endpoint `TestEndpoint`, which needs to dynamically resolve services based on incoming requests. The `IServiceFactory` interface provided by DynamicDiToolkit allows you to abstract the logic for resolving these services at runtime.

Below is an example of how you might implement a `TestEndpoint` class that uses `IServiceFactory` to dynamically resolve and invoke a `ListAsync` method on a service determined by the request:

```csharp
public class TestEndpoint : EndpointBaseAsync
    .WithRequest<Request>
    .WithResult<ActionResult<List<Entity>>>
{
    private readonly IServiceFactory _serviceFactory;

    public TestEndpoint(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    [HttpPost("test")]
    public override async Task<ActionResult<string>> HandleAsync([FromBody] Request request, CancellationToken cancellationToken = default)
    {
        // Dynamically resolve the service based on the entity name provided in the request
        dynamic service = _serviceFactory.GetService(typeof(IRepository<>), request.EntityName);
        // Call the ListAsync method on the resolved service
        var response = await service.ListAsync(cancellationToken);

        return Ok(response);
    }
}
```

In this example:
- `TestEndpoint` extends `EndpointBaseAsync`, configured with a request and result type, making it ready to handle HTTP POST requests to the "test" route [Ardalis.ApiEndpoints](https://github.com/ardalis/ApiEndpoints).
- The constructor injects `IServiceFactory`, which is used to dynamically resolve services.
- In the `HandleAsync` method, the service corresponding to the `request.EntityName` is dynamically resolved using `_serviceFactory.GetService`. This allows the endpoint to handle different entity types without statically coding each service type, enhancing modularity and flexibility.
- Finally, it invokes the `ListAsync` method on the dynamically resolved service and returns the result [Ardalis.Specification](https://github.com/ardalis/Specification).

This approach demonstrates how DynamicDiToolkit facilitates dynamic service resolution, enabling more adaptable and scalable application architectures.
