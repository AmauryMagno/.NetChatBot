using System.Text.Json;
using Waha;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("Swagger disponível em: http://localhost:5079/swagger");
}

app.MapPost("/bot", async (HttpContext context) =>
{
  var data = await JsonSerializer.DeserializeAsync<JsonElement>(context.Request.Body);
  Console.WriteLine($"Received event: {data.GetProperty("event").GetString()}");
  if (data.GetProperty("event").GetString() != "message")
  {
    // Process message, save it, respond, etc.
    var payload = data.GetProperty("payload");
    Console.WriteLine($"Received message: {payload.GetProperty("text").GetString()}");
  }
    await context.Response.WriteAsync("OK");
});
// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
