
using MarketPlace.CategoryAPI.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthentication();
builder.Host.UseSerilog((context, config) =>
{
    config
        .MinimumLevel.Override("Ocelot", Serilog.Events.LogEventLevel.Verbose) // Ensure Trace level for Ocelot
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();
});
builder.Configuration.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();
await app.UseOcelot();
app.Run();
