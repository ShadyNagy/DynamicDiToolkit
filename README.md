[![publish to nuget](https://github.com/ShadyNagy/DynamicDiToolkit/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/ShadyNagy/DynamicDiToolkit/actions/workflows/nuget-publish.yml)
[![DynamicDiToolkit on NuGet](https://img.shields.io/nuget/v/DynamicDiToolkit?label=DynamicDiToolkit)](https://www.nuget.org/packages/DynamicDiToolkit/)
[![NuGet](https://img.shields.io/nuget/dt/DynamicDiToolkit)](https://www.nuget.org/packages/DynamicDiToolkit)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/ShadyNagy/DynamicDiToolkit/blob/main/LICENSE)
[![paypal](https://img.shields.io/badge/PayPal-tip%20me-green.svg?logo=paypal)](https://www.paypal.me/shadynagy)
[![Visit My Website](https://img.shields.io/badge/Visit-My%20Website-blue?logo=internetexplorer)](https://ShadyNagy.com)


# Dynamic DI Toolkit

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