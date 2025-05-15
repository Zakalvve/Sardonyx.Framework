using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Sardonyx.Framework.Client.Extensions;
using Sardonyx.Framework.Core;
using Sardonyx.Framework.Core.CQRS.Extensions;
using Sardonyx.Framework.Testing.WebApp;
using Sardonyx.Framework.Testing.WebApp.TestClient;

var appAssembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddSardonyx<ExampleContext>()
    .AddCaching()
    .AddCQRS(appAssembly)
    .AddTypedHttpClient<ITestClient, TestClient>()
    .AddDbContext(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("db_Connection"));
    })
    .Build();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
