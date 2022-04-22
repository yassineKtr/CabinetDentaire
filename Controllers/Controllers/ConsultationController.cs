using DataAccess.Models;
using DataAccess.Readers.Consultations;
using DataAccess.Writers.Consultations;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/consultation")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IWriteConsultation _writer;
        private readonly IReadConsultation _reader;

        public ConsultationController(IWriteConsultation writer, IReadConsultation reader)
        {
            _writer = writer;
            _reader = reader;
        }
        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                return Results.Ok(await _reader.GetConsultations());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{type}")]
        public async Task<IResult> Get(string type)
        {
            try
            {
                var results = await _reader.GetConsultationByType(type);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] Consultation consultation)
        {

            try
            {
                await _writer.AddConsultation(consultation);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] Consultation consultation)
        {
            try
            {
                await _writer.UpdateConsultation(consultation);
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
                await _writer.DeleteConsultation(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
