using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.Dentists;
using DataAccess.Writers.Dentistes;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DataAccess.Tests.Readers.Dentistes
{
    public class DentisteReaderShould
    {
        private readonly IReadDentiste _dentisteReader;
        private readonly IWriteDentiste _dentisteWriter;
        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        public DentisteReaderShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 17));
            _dentisteReader = new DentisteReader(_configuration);
            _dentisteWriter = new DentisteWriter(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnDentiste()
        {
            //Arrange
            var sut = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(sut);
            //Act
            var result = await _dentisteReader.GetDentisteById(sut.Dentiste_id);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task ReturnNull()
        {
            //Arrange
            var sut = _fixture.Create<Dentiste>();
            //Act
            var result = await _dentisteReader.GetDentisteById(sut.Dentiste_id);
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnDentisteByName()
        {
            //Arrange
            var sut = _fixture.Create<Dentiste>();
            await _dentisteWriter.AddDentiste(sut);
            //Act
            var result = await _dentisteReader.GetDentisteByName(sut.Nom);
            //Assert
            Assert.NotNull(result);
        }
    }
}
