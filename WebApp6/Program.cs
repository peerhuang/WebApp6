using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
     .ReadFrom.Configuration(context.Configuration)
     .ReadFrom.Services(services)
     .Enrich.FromLogContext()
     .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(c => c.File("Logs/Debug/logs.txt", rollingInterval: RollingInterval.Day)))
     .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(c => c.File("Logs/Information/logs.txt", rollingInterval: RollingInterval.Day)))
     .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(c => c.File("Logs/Warning/logs.txt", rollingInterval: RollingInterval.Day)))
     .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(c => c.File("Logs/Error/logs.txt", rollingInterval: RollingInterval.Day)))
     .WriteTo.Console());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();