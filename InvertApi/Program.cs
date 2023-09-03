using InvertApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddTransient<ConfigLogic>();
builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(x =>
    {
        x.AllowAnyOrigin();
    });
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();
app.Run();