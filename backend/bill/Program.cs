using bill;
using bill.Context;
using bill.Repositories;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("BillDbContext");

builder.Services.AddDbContext<BillDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(x =>
{
    x.AddPolicy("allow", policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddTransient<UnitRepository>();
builder.Services.AddTransient<ItemRepository>();
builder.Services.AddTransient<ReceiptRepository>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //options.InvalidModelStateResponseFactory = (action) => {
    //    return new OkObjectResult(new Result { status_code = -1, message = "invalit" });
    //};
    options.SuppressModelStateInvalidFilter = true;
});



var app = builder.Build();

app.UseCors("allow");

// Configure the HTTP request pipeline.

app.UseAuthorization();


app.MapControllers();

app.Run();

