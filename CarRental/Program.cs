using CarRentalServices.Contracts;
using CarRentalServices.Repositories;
using CarRentalServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IMySqlDataService, MySqlDataService>();
builder.Services.AddKeyedScoped<ICategoryRepository, MySqlCategoryRepository>("MySql");
builder.Services.AddKeyedScoped<ICategoryRepository, CategoryRepository>("MsSql");
builder.Services.AddKeyedScoped<ICarsRepository, CarRepository>("MsSql");
builder.Services.AddKeyedScoped<ICarsRepository, CarsRepository>("MySql");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddKeyedScoped<IColorRepository, ColorRepository>("MySql");
builder.Services.AddKeyedScoped<IBrandRepository, BrandRepository>("MySql");

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
