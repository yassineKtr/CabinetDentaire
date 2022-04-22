using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.Dentists;
using DataAccess.Writers.Dentistes;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DataAccess.Tests.Writers.Dentistes
{
    public class DentisteWriterShould
    {
        private readonly IWriteDentiste _dentisteWriter;
        private readonly IReadDentiste _dentisteReader;
        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        public DentisteWriterShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 17));
            _dentisteWriter = new DentisteWriter(_configuration);
            _dentisteReader = new DentisteReader(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddDentiste()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            //Act
            var result = await _dentisteReader.GetDentisteById(dentiste.Dentiste_id);
            //Assert
            Assert.Equal(dentiste.Dentiste_id, result.Dentiste_id);
        }
        [Fact]
        public async Task UpdateDentiste()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            var newDentiste = _fixture.Build<Dentiste>()
                .With(x => x.Dentiste_id, dentiste.Dentiste_id)
                .Create();
            //Act
            await _dentisteWriter.UpdateDentiste(newDentiste);
            var result = await _dentisteReader.GetDentisteById(dentiste.Dentiste_id);
            //Assert
            Assert.Equal(newDentiste.Dentiste_id, result.Dentiste_id);
        }

        [Fact]
        public async Task DeleteDentiste()
        {
            //Arrange
            var dentiste = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(dentiste);
            //Act
            await _dentisteWriter.DeleteDentiste(dentiste.Dentiste_id);
            var result = await _dentisteReader.GetDentisteById(dentiste.Dentiste_id);
            //Assert
            Assert.Null(result);
        }

    }
}
