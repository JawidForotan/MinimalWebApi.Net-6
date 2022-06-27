using AspNetWebApi.Data;
using AspNetWebApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register connection string from appsetting.json

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Get all records from database

async Task<List<Student>> GetAllStudents(DataContext context) =>
    await context.students.ToListAsync();


// Test get mehtod

app.MapGet("/", () => "Hello from minimal web api in .Net 6");


// GEt all students wihtout async await 😉

app.MapGet("/getAll", (DataContext context) =>

context.students.ToList());


// Get student by id

app.MapGet("/getOne/{id}", async (DataContext context, int id) =>
await context.students.FindAsync(id) is (Student student) ? Results.Ok(student) : Results.NotFound("There is no record with this id ☹️"));


// Post to create new student

app.MapPost("/creatNewStudent", async (DataContext context, Student newStudent) =>
{

    context.students.Add(newStudent);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllStudents(context));
});


// Update

app.MapPut("/updateStudent/{id}", async (DataContext context, Student updatedStudent, int id) =>
{
    var studentToUpdate = await context.students.FindAsync(id);
    if (studentToUpdate == null) return Results.NotFound("There is no record with this id ☹️");

    studentToUpdate.Id = id;
    studentToUpdate.Name = updatedStudent.Name;
    studentToUpdate.Age = updatedStudent.Age;
    studentToUpdate.City = updatedStudent.City;
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllStudents(context));
});


// Delete

app.MapDelete("/student/{id}", async (DataContext context, int id) =>
{
    var studentToDelete = await context.students.FindAsync(id);
    if (studentToDelete == null) return Results.NotFound("What the... it doesn't even exist in our database");
    context.students.Remove(studentToDelete);
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllStudents(context));
});


app.Run();
