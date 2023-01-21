using GloboTicket.Catalog.Config;
using GloboTicket.Catalog.Repositories;
using GloboTicket.Services.EventCatalog.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();

var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(SwaggerOptions.Key).Bind(swaggerOptions);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
// add DB context here

builder.Services.AddDbContext<EventCatalogDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConcertRepository, ConcertRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<EventCatalogDbContext>();
        context.Database.EnsureCreated();
    }
}


if (swaggerOptions.IsSwaggerEnable)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
