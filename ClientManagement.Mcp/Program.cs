using ClientManagement.Mcp;
using ClientManagement.Mcp.Tools;
using ClientManagement.Models.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AgentOptions>(options =>
{
    builder.Configuration.GetSection("AI").Bind(options);
});

builder.Services.AddMcpServices(builder.Configuration);

// Add the MCP services: the transport to use (http) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for servers that don't need
        // server-to-client requests like sampling or elicitation.
        // See https://csharp.sdk.modelcontextprotocol.io/concepts/transports/transports.html for details.
        options.Stateless = true;
    })
    .WithToolsFromAssembly(typeof(RagToolKit).Assembly);

var app = builder.Build();
app.MapMcp();
//app.UseHttpsRedirection();

app.Run();
