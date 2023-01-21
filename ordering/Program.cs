using GloboTicket.Ordering.Config;
using GloboTicket.Ordering.Services;

var builder = WebApplication.CreateBuilder(args);



var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(SwaggerOptions.Key).Bind(swaggerOptions);
// Add services to the container.
builder.Configuration.AddEnvironmentVariables();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

if (swaggerOptions.IsSwaggerEnable)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
