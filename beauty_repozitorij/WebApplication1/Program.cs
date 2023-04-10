using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Data;
using WebApplication1.Helper;
using WebApplication1.Helper.ErrorHandler;
using WebApplication1.Service;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false).Build();

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews()
  .AddJsonOptions(options =>
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("db1")));


builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(config.GetConnectionString("db1")));

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AutorizacijaSwaggerHeader>();
});

builder.Services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, MailerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(
           options => options
           .SetIsOriginAllowed(x => _ = true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
        );

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
