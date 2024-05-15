using ContactsApp.Models.Abstractions;
using ContactsApp.Models;
using ContactsApp.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ContactsDatabaseSettings>(
    builder.Configuration.GetSection(nameof(ContactsDatabaseSettings)));

builder.Services.AddSingleton<IContactsDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ContactsDatabaseSettings>>().Value);

builder.Services.AddSingleton<ContactService>();

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
     });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

//builder.Services.AddControllers()
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

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
