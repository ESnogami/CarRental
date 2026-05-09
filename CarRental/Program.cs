using CarRentalServices.Contracts;
using CarRentalServices.Repositories;
using CarRentalServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IMySqlDataService, MySqlDataService>();
builder.Services.AddScoped<ICategoryRepository, MySqlCategoryRepository>();
builder.Services.AddKeyedScoped<ICategoryRepository, CategoryRepository>("MsSql");
builder.Services.AddKeyedScoped<ICarRepository, CarRepository>("MsSql");
builder.Services.AddKeyedScoped<ICustomerRepository, CustomerRepository>("MsSql");
builder.Services.AddKeyedScoped<ILocationRepository, LocationRepository>("MsSql");

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

app.UseAuthorization();

app.MapControllers();

app.Run();
