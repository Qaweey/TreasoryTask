using Integration.api.Dependency_Injection;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilog(LoggerDI.AddLogger(builder));
builder.Services.AddCoreServices(builder, builder.Configuration);
var app=RequestPipeline.App(builder);
app.Run();  