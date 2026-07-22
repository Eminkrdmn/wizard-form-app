using Microsoft.EntityFrameworkCore;
using WizardFormApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=wizard.db"));

var app = builder.Build();

app.MapControllers();

app.Run();