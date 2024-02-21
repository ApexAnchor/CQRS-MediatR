using CQRS.MediatR.Data;
using CQRS.MediatR.Domain;
using CQRS.MediatR.Domain.PipelineBehaviour;
using CQRS.MediatR.Domain.Query;
using CQRS.MediatR.Domain.Validations;
using CQRS.MediatR.WebApi;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseInMemoryDatabase("ProductsDb");
});

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining(typeof(GetAllProductsQuery)));
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior <,>),
    typeof(ValidationBehaviour<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(option => { });
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();



app.Run();
