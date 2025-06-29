var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

// Configure main page
app.MapGet("/", () => Results.Content(HtmlContent.GetMainPage(), "text/html"));

// Configure API endpoints
app.MapBookmarkEndpoints();

// Start server on available port
var port = PortService.FindAvailablePort();
Console.WriteLine($"Starting server on http://localhost:{port}");
app.Run($"http://localhost:{port}");