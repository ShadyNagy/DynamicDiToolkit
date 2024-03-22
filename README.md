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


In the program/startup
```csharp
services.AddScopedDynamicDiToolkitServices();
```

In the service
```csharp
public class TestEndpoint : EndpointBaseAsync
	.WithoutRequest
	.WithResult<ActionResult<List<Entity>>>
{
	private readonly IServiceFactory _serviceFactory;

	public TestEndpoint(IServiceFactory serviceFactory)
	{
		_serviceFactory = serviceFactory;
	}

	[HttpGet("test")]
	public override async Task<ActionResult<string>> HandleAsync(CancellationToken cancellationToken = default)
	{

		dynamic service = _serviceFactory.GetService(typeof(IService<>), nameof(Entity));
		var response = await service.ListAsync(cancellationToken);

		return Ok(response);
	}
}

```