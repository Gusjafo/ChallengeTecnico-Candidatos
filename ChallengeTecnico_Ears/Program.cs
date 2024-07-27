using ChallengeTecnico_Ears.Context;
using ChallengeTecnico_Ears.IRepository;
using ChallengeTecnico_Ears.IService;
using ChallengeTecnico_Ears.Repository;
using ChallengeTecnico_Ears.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("defaultConnection")!;
builder.Services.AddDbContext<ContextChallenge>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository,PersonRepository>();

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
