using BPA.AppConfiguration.ServiceRegistrationExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ServiceCollectionExtension();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

/*
 builder.Services.AddHttpClient("loggerApiClient", c =>
{
    c.BaseAddress = new Uri("https://localhost:7102/");
}).AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(2, TimeSpan.FromSeconds(10)));
*/
var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/status");

app.AddCorrelationIdMiddleware();
//app.AddGlobalExceptionHandlingMiddleware();
app.Run();
