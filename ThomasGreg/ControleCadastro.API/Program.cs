using ControleCadastro.Application.Mappings;
using ControleCadastro.Infra.Data.Contexto;
using ControleCadastro.Infra.Ioc;
using Microsoft.EntityFrameworkCore;
using ControleCadastro.Infra.Data.Procedure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSecuritySwagger();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(EntitiesDTOProfile));
builder.Services.AddScoped<ProcedureService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var databaseInitializationService = scope.ServiceProvider.GetRequiredService<ProcedureService>();
    await databaseInitializationService.ExecutarProceduresAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
