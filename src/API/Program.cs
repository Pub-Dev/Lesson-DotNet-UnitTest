using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using PubDev.Store.API;
using PubDev.Store.API.Interfaces.Presenters;
using PubDev.Store.API.Middlewares;
using PubDev.Store.API.Presenters;
using PubDev.Store.API.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.CustomOperationIds(operation =>
    {
        var actionDescription = (ControllerActionDescriptor)operation.ActionDescriptor;

        var actionName = actionDescription.ActionName;

        return $"{actionName}";
    });
    options.MapType<decimal>(() => new OpenApiSchema
    {
        Type = "number",
        Format = "decimal"
    });
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScoped<NotificationContext>();
builder.Services.AddScoped<IPresenter, Presenter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
