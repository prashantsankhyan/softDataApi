//using Microsoft.EntityFrameworkCore;
//using softDataApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// ✅ Proper DbContext registration
//builder.Services.AddDbContext<SoftDataContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs"))
//);
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy.AllowAnyOrigin()      // replace with "https://localhost:4200" in prod
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowAngular");
//app.UseAuthorization();
//app.MapControllers();

//app.Run();


using Microsoft.EntityFrameworkCore;
using softDataApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<SoftDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs"))
);

// CORS — IMPORTANT
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy
                .WithOrigins("https://peaceful-murdock.180-179-213-240.plesk.page")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});


var app = builder.Build();

// -------------------- PIPELINE --------------------

// Swagger only for dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🚨 CORS MUST COME BEFORE Authorization & Controllers
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
