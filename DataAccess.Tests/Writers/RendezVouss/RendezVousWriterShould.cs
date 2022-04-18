using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.RendezVouss;
using DataAccess.Writers.Clients;
using DataAccess.Writers.Consultations;
using DataAccess.Writers.Dentistes;
using DataAccess.Writers.RendezVouss;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DataAccess.Tests.Writers.RendezVouss
{
    public class RendezVousWriterShould
    {
        private readonly IWriteRendezVous _rendezVousWriter;
        private readonly IReadRendezVous _rendezVousReader;
        private readonly IWriteDentiste _dentisteWriter;
        private readonly IWriteConsultation _consultationWriter;
        private readonly IWriteClient _clientWriter;
        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        public RendezVousWriterShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 17));
            _rendezVousWriter = new RendezVousWriter(_configuration);
            _rendezVousReader = new RendezVousReader(_configuration);
            _dentisteWriter = new DentisteWriter(_configuration);
            _consultationWriter = new ConsultationWriter(_configuration);
            _clientWriter = new ClientWriter(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddRendezVous()
        {
            // Arrange
            var client = _fixture.Create<Client>();
            await _clientWriter.AddClient(client);
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            var consultation = _fixture.Create<Consultation>();
            await _consultationWriter.AddConsultation(consultation);
            var rendezVous = _fixture.Build<RendezVous>()
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .Create();
            await _rendezVousWriter.AddRendezVous(rendezVous);
            //Act
            var result = await _rendezVousReader.GetRendezVousById(rendezVous.Rdv_id);
            //Assert
            Assert.Equal(rendezVous.Rdv_id, result.Rdv_id);
        }
        [Fact]
        public async Task UpdateRendezVous()
        {
            //Arrange
            var client = _fixture.Create<Client>();
            await _clientWriter.AddClient(client);
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            var consultation = _fixture.Create<Consultation>();
            await _consultationWriter.AddConsultation(consultation);
            var rendezVous = _fixture.Build<RendezVous>()
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .Create();
            await _rendezVousWriter.AddRendezVous(rendezVous);
            //Act
            var result = await _rendezVousReader.GetRendezVousById(rendezVous.Rdv_id);
            result.Date_rdv = new DateTime(2022, 1, 1);
            await _rendezVousWriter.UpdateRendezVous(result);
            var result2 = await _rendezVousReader.GetRendezVousById(rendezVous.Rdv_id);
            //Assert
            Assert.Equal(result.Rdv_id, result2.Rdv_id);
        }

        [Fact]
        public async Task DeleteRendezVous()
        {
            //Arrange
            var client = _fixture.Create<Client>();
            await _clientWriter.AddClient(client);
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            var consultation = _fixture.Create<Consultation>();
            await _consultationWriter.AddConsultation(consultation);
            var rendezVous = _fixture.Build<RendezVous>()
                .With(x => x.Client_id, client.Client_id)
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .With(x => x.Consultation_id, consultation.Consultation_id)
                .Create();
            await _rendezVousWriter.AddRendezVous(rendezVous);
            //Act
            await _rendezVousWriter.DeleteRendezVous(rendezVous.Rdv_id);
            var result = await _rendezVousReader.GetRendezVousById(rendezVous.Rdv_id);
            //Assert
            Assert.Null(result);

        }
    }
}
