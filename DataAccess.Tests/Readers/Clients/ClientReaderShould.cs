﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using DataAccess.Models;
using DataAccess.Readers.Clients;
using DataAccess.Writers.Clients;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DataAccess.Tests.Readers.Clients
{
    public class ClientReaderShould
    {
        private readonly IWriteClient _clientWriter;
        private readonly IReadClient _clientReader;
        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        public ClientReaderShould()
        {
            _configuration = TestHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 17));
            _clientReader = new ClientReader(_configuration);
            _clientWriter = new ClientWriter(_configuration);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnClient()
        {
            //Arrange
            var sut = _fixture.Create<Client>();
            await _clientWriter.AddClient(sut);
            //Act
            var result = await _clientReader.GetClientById(sut.Client_id);
            //Assert
            Assert.Equal(sut.Client_id, result.Client_id);
        }
        [Fact]
        public async Task ReturnNull()
        {
            //Arrange
            var sut = _fixture.Create<Client>();
            //Act
            var result = await _clientReader.GetClientById(sut.Client_id);
            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task<IEnumerable<Client>> ReturnClients()
        {
            //Arrange
            var sut = _fixture.CreateMany<Client>();
            foreach (var client in sut)
            {
                await _clientWriter.AddClient(client);
            }
            //Act
            var result = await _clientReader.GetAllClients();
            //Assert
            Assert.Equal(sut.Count(), result.Count());
            return result;
        }
    }
}
