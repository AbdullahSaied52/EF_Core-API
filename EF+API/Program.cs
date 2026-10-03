using Application.Intrefaces;
using Application.Services;
using EF_API.DBcontext;
using Infrastructure.StudentRepositry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Intrefaces;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<TrainingCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IStudentRepositry, studentRepository>();
builder.Services.AddScoped<StudentService>();

var app = builder.Build();

//test
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TrainingCenterDbContext>();

    Console.WriteLine("======== ");
    Console.WriteLine(context.Database.CanConnect()
        ? "Connected! You are ready to retrieve Data :- )"
        : "Failed to connect.");
    Console.WriteLine("======== ");
}

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
