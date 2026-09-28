using BuildingBlocks.Extensions.ServiceCollection;
using BuildingBlocks.Middlewares;
using UserService.Application.Extensions;
using UserService.Infrastructure.Extensions;
using UserService.Infrastructure.Seeders;
using UserService.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.AddPresentation();

builder.Services.AddSwagger("User");

var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
await seeder.Seed();

app.UseSwagger();
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
