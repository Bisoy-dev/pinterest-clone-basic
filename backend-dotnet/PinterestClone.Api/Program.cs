global using PinterestClone.Infrastructure;
using PinterestClone.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

const string CORS_POLICY = "CORSPOL";
builder.Services.AddCors(opt => 
    {
        opt.AddPolicy(CORS_POLICY, option => 
            option.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(CORS_POLICY);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
