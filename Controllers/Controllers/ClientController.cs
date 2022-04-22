using DataAccess.Models;
using DataAccess.Readers.Clients;
using DataAccess.Writers.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IReadClient _reader;
        private readonly IWriteClient _writer;

        public ClientController(IReadClient reader, IWriteClient writer)
        {
            _reader = reader;
            _writer = writer;
        }
        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                return Results.Ok(await _reader.GetAllClients());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            try
            {
                var results = await _reader.GetClientById(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] Client client)
        {

            try
            {
                await _writer.AddClient(client);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] Client client)
        {
            try
            {
                await _writer.UpdateClient(client);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                await _writer.DeleteClient(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

    }
}
