using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.Clients;
using DataAccess.Writers.Clients;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DataAccess.Tests.Writers.Clients
{
    public class ClientWriterShould
    {
        private readonly IWriteClient _clientWriter;
        private readonly IReadClient _clientReader;
        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        public ClientWriterShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory().Substring(0,Directory.GetCurrentDirectory().Length-17));
            _clientReader = new ClientReader(_configuration);
            _clientWriter = new ClientWriter(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddClient()
        {
            //Arrange
            var sut = _fixture.Create<Client>();
            //Act 
            await _clientWriter.AddClient(sut);
            //Assert
            var result = _clientReader.GetClientById(sut.Client_id);
            var resultToBeTested = result.Result;
            Assert.NotNull(resultToBeTested);
        }

        [Fact]
        public async Task UpdateClient()
        {
            //Arrange
            var sut = _fixture.Create<Client>();
            await _clientWriter.AddClient(sut);
            //Act 
            sut.Nom = "Updated";
            await _clientWriter.UpdateClient(sut);
            //Assert
            var result = _clientReader.GetClientById(sut.Client_id);
            var resultToBeTested = result.Result;
            Assert.Equal(sut.Nom,resultToBeTested.Nom);
        }

        [Fact]
        public async Task DeleteClient()
        {
            //Arrange
            var sut = _fixture.Create<Client>();
            await _clientWriter.AddClient(sut);
            //Act 
            await _clientWriter.DeleteClient(sut.Client_id);
            //Assert
            var result = _clientReader.GetClientById(sut.Client_id);
            var resultToBeTested = result.Result;
            Assert.Null(resultToBeTested);
        }
    }
}
