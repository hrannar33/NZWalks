using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NZWalksDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));

});
builder.Services.AddScoped<IRegionsRepository, RegionsRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IWalksRepository, WalksRepository>();

builder.Services.AddScoped<IWalksDifficultyRepository, WalksDifficultyRepository>();


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
