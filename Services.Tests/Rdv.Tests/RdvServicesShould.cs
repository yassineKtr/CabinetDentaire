using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.Consultations;
using DataAccess.Readers.Dentists;
using DataAccess.Readers.RendezVouss;
using DataAccess.Tests;
using DataAccess.Writers.Clients;
using DataAccess.Writers.Consultations;
using DataAccess.Writers.Dentistes;
using DataAccess.Writers.RendezVouss;
using Microsoft.Extensions.Configuration;
using Services.Rdv;
using Xunit;

namespace Services.Tests.Rdv.Tests
{
    public class RdvServicesShould
    {
        private readonly IRdvServices _rdvServices;
        private readonly IWriteRendezVous _renderVousWriter;
        private readonly IReadRendezVous _renderVousReader;
        private readonly IReadConsultation _consultationReader;
        private readonly IReadDentiste _dentisteReader;
        private readonly IWriteDentiste _dentisteWriter;
        private readonly IWriteClient _clientWriter;
        private readonly IWriteConsultation _consultationWriter;
        private readonly IConfiguration _configuration;
        private readonly Fixture _fixture;


        public RdvServicesShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory()
                .Substring(0, Directory.GetCurrentDirectory().Length - 17));
            _renderVousWriter = new RendezVousWriter(_configuration);
            _dentisteWriter = new DentisteWriter(_configuration);
            _renderVousReader = new RendezVousReader(_configuration);
            _consultationReader = new ConsultationReader(_configuration);
            _consultationWriter = new ConsultationWriter(_configuration);
            _dentisteReader = new DentisteReader(_configuration);
            _clientWriter = new ClientWriter(_configuration);
            _rdvServices = new RdvServices(_renderVousWriter, _renderVousReader, _consultationReader, _dentisteReader);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateRdvIfNumberOfDentistesIsEnough()
        {
            //Arrange
            var dentiste = _fixture.Build<Dentiste>()
                .With(x => x.Max_clients, 2)
                .Create();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            var date = _fixture.Create<DateTime>();
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .Create();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            await _renderVousWriter.AddRendezVous(rdv);
            //Act 
            await _rdvServices.CreateRdv(dentiste.Nom, client.Client_id, consultation.Consultation_type, date);
            var result = await _renderVousReader.GetRendezVousByDentisteId(dentiste.Dentiste_id);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ReturnErrorIfNumberOfDentistesIsSurpassed()
        {
            //Arrange
            var dentiste = _fixture.Build<Dentiste>()
                .With(x => x.Max_clients, 1)
                .Create();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            var date = _fixture.Create<DateTime>();
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .Create();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            await _renderVousWriter.AddRendezVous(rdv);
            //Act 
            
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await _rdvServices.CreateRdv(dentiste.Nom, client.Client_id, consultation.Consultation_type, date));
        }

        [Fact]
        public async Task CancelRdvIfExists()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Annule, false)
                .Create();
            //Act
            await _renderVousWriter.AddRendezVous(rdv);
            var reason = _fixture.Create<string>();
            await _rdvServices.CancelRdv(rdv.Rdv_id, reason);
            //Assert
            var rdvToBeTested = await _renderVousReader.GetRendezVousById(rdv.Rdv_id);
            var annule = rdvToBeTested.Annule;
            var annuleReason = rdvToBeTested.Reason;
            Assert.NotNull(rdvToBeTested);
            Assert.True(annule);
            Assert.Equal(reason, annuleReason);
        }

        [Fact]
        public async Task ReturnErrorIfRdvDoesNotExist()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Annule, false)
                .Create();
            //Act
            await _renderVousWriter.AddRendezVous(rdv);
            var reason = _fixture.Create<string>();
            //Assert
            await Assert.ThrowsAsync<Exception>(() => _rdvServices.CancelRdv(Guid.NewGuid(), reason));
        }

        [Fact]
        public async Task PayForRdvIfExists()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Paye, false)
                .Create();
            //Act
            await _renderVousWriter.AddRendezVous(rdv);
            await _rdvServices.PayForRdv(rdv.Rdv_id);
            //Assert
            var rdvToBeTested = await _renderVousReader.GetRendezVousById(rdv.Rdv_id);
            Assert.True(rdvToBeTested.Paye);
            Assert.NotNull(rdvToBeTested);
        }

        [Fact]
        public async Task ReturnErrorIfRdvNotFound()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            var client = _fixture.Create<Client>();
            var consultation = _fixture.Create<Consultation>();
            await _dentisteWriter.AddDentiste(dentiste);
            await _clientWriter.AddClient(client);
            await _consultationWriter.AddConsultation(consultation);
            var rdv = _fixture.Build<RendezVous>()
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Paye, false)
                .Create();
            //Act
            await _renderVousWriter.AddRendezVous(rdv);
            //Assert
            await Assert.ThrowsAsync<Exception>(() => _rdvServices.PayForRdv(Guid.NewGuid()));
        }
    }
}