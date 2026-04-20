using Core.Entity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LockerRepository>();
builder.Services.AddScoped<UsageRepository>();
builder.Services.AddScoped<IBase<UsageDTO>, UsageRepository>();
builder.Services.AddScoped<IBase<UserDTO>, UserRepository>();
builder.Services.AddScoped<IBase<LockerDTO>, LockerRepository>();
builder.Services.AddScoped<CommandRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CommandService>();
builder.Services.AddScoped<LockerService>();
builder.Services.AddScoped<UsageService>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // 

    if (!db.Lockers.Any())
    {
        db.Lockers.AddRange(
            new Lockers { Status = true },
            new Lockers { Status=true}
        );
        db.SaveChanges();
    }
}

app.Run();
