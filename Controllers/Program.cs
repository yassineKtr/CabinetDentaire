using DataAccess.Readers.Clients;
using DataAccess.Readers.Consultations;
using DataAccess.Readers.Dentists;
using DataAccess.Readers.RendezVouss;
using DataAccess.Writers.Clients;
using DataAccess.Writers.Consultations;
using DataAccess.Writers.Dentistes;
using DataAccess.Writers.RendezVouss;
using Services.Rdv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRdvServices,RdvServices>();
builder.Services.AddSingleton<IReadClient,ClientReader >();
builder.Services.AddSingleton<IWriteClient, ClientWriter>();
builder.Services.AddSingleton<IReadConsultation, ConsultationReader>();
builder.Services.AddSingleton<IWriteConsultation, ConsultationWriter>();
builder.Services.AddSingleton<IReadDentiste, DentisteReader>();
builder.Services.AddSingleton<IWriteDentiste, DentisteWriter>();
builder.Services.AddSingleton<IWriteRendezVous,RendezVousWriter>();
builder.Services.AddSingleton<IReadRendezVous, RendezVousReader>();



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

app.Run();
