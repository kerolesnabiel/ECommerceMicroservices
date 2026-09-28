using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/user-service/swagger/v1/swagger.json", "User Service v1");
    opts.SwaggerEndpoint("/product-service/swagger/v1/swagger.json", "Product Service v1");
    opts.SwaggerEndpoint("/order-service/swagger/v1/swagger.json", "Order Service v1");
    opts.SwaggerEndpoint("/cart-service/swagger/v1/swagger.json", "Cart Service v1");
    opts.SwaggerEndpoint("/payment-service/swagger/v1/swagger.json", "Payment Service v1");
    opts.SwaggerEndpoint("/notification-service/swagger/v1/swagger.json", "Notification Service v1");

    opts.RoutePrefix = "docs";         
    opts.DocumentTitle = "E-Commerce API";
    opts.DisplayRequestDuration();              // shows response time per request
    opts.DefaultModelsExpandDepth(-1);          // hides the schema section by default
    opts.EnableTryItOutByDefault();             // opens "Try it out" automatically
});

app.UseRateLimiter();
app.MapReverseProxy();

app.Run();