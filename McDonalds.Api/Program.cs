using McDonalds.Data;
using McDonalds.Repositories;
using McDonalds.Mappings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<McDonaldsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("McDonalds")));

builder.Services.AddScoped<ResourceRepository>();
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "McDonalds API", Version = "v1" });
    c.EnableAnnotations();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "McDonalds API v1"));

app.UseHttpsRedirection();
app.MapControllers();

// Автоматична міграція + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<McDonaldsContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

app.Run();