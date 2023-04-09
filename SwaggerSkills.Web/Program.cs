using Microsoft.EntityFrameworkCore;
using SwaggerSkills.Web;
using System.Reflection;
using System.Web;
using SwaggerSkills.Web.Controllers;
using SwaggerSkills.Web.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetService<IConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


UriBuilder uriBuilder = new UriBuilder("www.github.com/alperkaragoz");
uriBuilder.Query = "somekey=" + HttpUtility.UrlEncode("some+value");
Uri githubUri = uriBuilder.Uri;

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Customer Api",
        Description = "Add/Delete/Update api process for Customer",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Alper KARAG�Z",
            Email = "alperkaragoz@outlook.com",
            Url = githubUri
        }
    });
    //Swagger'�n xml dosyas�na g�re dok�mantasyon yapmas� gerekiyor.Bunun i�in xml dosyam�z� veriyoruz.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<SwaggerContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("SwaggerContext"));
});

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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Api");
});

//app.MapCustomerEndpoints();

app.Run();
