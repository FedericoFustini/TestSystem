using Serilog;
using TestSystem.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "SwaggerAnnotation.xml"));
});

//add DI for TestSystem classes
builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddHealthChecks();


var app = builder.Build();
app.UseSerilogRequestLogging();
app.MapHealthChecks("/healthz");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapGet("/", (HttpContext context ) => {
	if (app.Environment.IsDevelopment())
		context.Response.Redirect("/swagger/index.html");
	else
		context.Response.StatusCode = StatusCodes.Status404NotFound;
});

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
