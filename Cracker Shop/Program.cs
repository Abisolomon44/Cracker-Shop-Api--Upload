using Microsoft.Data.SqlClient;
using System.Data;
using Cracker_Shop.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCommonRepositories(
    builder.Configuration.GetConnectionString("DefaultConnection")!
);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();      // If you want Swagger in prod, keep this
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseDefaultFiles();  
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();
