# Product API Using MediatR and CQRS

In this repository, we are implementing a Product API using the Command Query Responsibility Segregation (CQRS) pattern and the MediatR library. This pattern provides a clear and structured way to design and implement our API. We'll also explore how to implement pipeline behavior and validations using the same MediatR library and FluentValidation for validation rules.


## Getting Started
You need to install the MediatR library via NuGet:
```
Install-Package MediatR
```
## Defining Models
```csharp
public class Product
{
   public Guid Id { get; set; }
   public string Name { get; set; }
   public string Description { get; set; }
   public decimal UnitPrice { get; set; }
   public int Quantity { get; set; }
}
```
## Commands and Handlers (Write Operations)

Here, we define our commands and command handlers:

### Create Product
```csharp
public class CreateProductCommand : IRequest<CreateProductCommandResponse>
{       
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }    
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
{    
    public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.Id = Guid.NewGuid();
        await productDbContext.Products.AddAsync(product);
        await productDbContext.SaveChangesAsync();
        var response = mapper.Map<CreateProductCommandResponse>(product);
        return response;
    }
}
```
### Delete Product
```csharp
public class DeleteProductCommand : IRequest<DeleteProductCommandResponse>
{
    public Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
{    
    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var prd = await productDbContext.Products.FindAsync(request.Id);
        if (prd is null)
        {
            return new DeleteProductCommandResponse() { IsDeleteSuccessful = false };
        }
        productDbContext.Products.Remove(prd);
        await productDbContext.SaveChangesAsync();            
        return new DeleteProductCommandResponse() { IsDeleteSuccessful = true };
    }
}
```

## Queries and Handlers (Read Operations)

Here, we define our queries and query handlers:

### GetProductById
```csharp
public class GetProductByIdQuery : IRequest<GetProductByIdQueryResponse>
{
    public Guid Id { get; set; }
}

 public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
 {
     public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
     {
         var response = await productDbContext.Products.FindAsync(request.Id, cancellationToken);

         return mapper.Map<GetProductByIdQueryResponse>(response);            
     }
 }
```
### Get All Products
```csharp
public class GetAllProductsQuery : IRequest<List<GetProductsQueryResponse>> 
{

}
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetProductsQueryResponse>>
{    
    public async Task<List<GetProductsQueryResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
       var response = await productDbContext.Products.ToListAsync(cancellationToken).ConfigureAwait(false);
       var result = mapper.Map<List<GetProductsQueryResponse>>(response); 
       return result;           
    }
}
```

## Usage in Controller

Once you have defined the commands, queries and their handlers, you can use them in your controller:

```csharp
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
    {
        var response = await mediator.Send(request);
        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductById([FromQuery]GetProductByIdQuery request)
    {
        var response = await mediator.Send(request);
        if (response is not null)
            return new JsonResult(response);
        else
            return BadRequest("No Product exists with the given id");
    }

    // Similar methods for update and delete
}
```

That's it! You have now implemented a basic CQRS pattern using MediatR in your .NET API.

## Pipeline Behaviors and Validations Using MediatR and FluentValidation
Install the necessary packages via NuGet Package Manager:
```
Install-Package FluentValidation
```
## Define Validation Rules
```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
        RuleFor(x=>x.Description).NotEmpty();
        RuleFor(x=>x.UnitPrice).GreaterThanOrEqualTo(1);
        RuleFor(x=>x.Quantity).GreaterThanOrEqualTo(10);
    }
}
```
## Implementing Pipeline Behavior

Pipeline behaviors in MediatR allow you to encapsulate common logic across different handlers like logging, caching, error handling and importantly validation.

Let's define a ValidationBehaviour that validates our requests before they reach our handlers.

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
```
In `ValidationBehavior`, we get all `IValidator`'s applicable and run them against the incoming request. If any validation rules are broken, we collect those failures and throw a `ValidationException`.

## Register Services

Don't forget to register your validators and pipeline behaviors in your service configuration in `Startup.cs`:

```csharp
services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

That's it! With this, your application will validate incoming requests before they get to the handler, making your code clean and maintainable.
