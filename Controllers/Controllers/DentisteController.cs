using DataAccess.Models;
using DataAccess.Readers.Dentists;
using DataAccess.Writers.Dentistes;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/dentiste")]
    [ApiController]
    public class DentisteController : ControllerBase
    {
        private readonly IWriteDentiste _writer;
        private readonly IReadDentiste _reader;

        public DentisteController(IWriteDentiste dentisteWriter, IReadDentiste dentisteReader)
        {
            _writer = dentisteWriter;
            _reader = dentisteReader;
        }

        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                return Results.Ok(await _reader.GetDentistes());
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
                var results = await _reader.GetDentisteById(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] Dentiste dentiste)
        {

            try
            {
                await _writer.AddDentiste(dentiste);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] Dentiste dentiste)
        {
            try
            {
                await _writer.UpdateDentiste(dentiste);
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
                await _writer.DeleteDentiste(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        [HttpGet("dentiste/{name}")]
        public async Task<IResult> GetDentisteByName(string name)
        {

            try
            {
                var results = await _reader.GetDentisteByName(name);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

    }

}
