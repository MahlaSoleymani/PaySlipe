using Api;
using Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWebServices(builder.Configuration, siteSetting);
builder.Services.AddApplicationServices(builder.Configuration, siteSetting);
builder.Services.AddInfrastructureServices(builder.Configuration, siteSetting);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
