using DataAccess.Readers.RendezVouss;
using DataAccess.Writers.RendezVouss;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Rdv;

namespace Controllers.Controllers
{
    [Route("api/rendezvous")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly IRdvServices _rdvServices;
        private readonly IReadRendezVous _reader;
        private readonly IWriteRendezVous _writer;

        public RendezVousController(IRdvServices rdvServices, IReadRendezVous reader, IWriteRendezVous writer)
        {
            _rdvServices = rdvServices;
            _reader = reader;
            _writer = writer;
        }
        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                return Results.Ok(await _reader.GetAllRendezVous());
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
                var results = await _reader.GetRendezVousById(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IResult> CreateRdv([FromBody]RdvDto data)
        {
            var dentisteName = data.DentisteName;
            var clientId = data.ClientId;
            var date = data.Date;
            var consultation = data.ConsultationType;
            try
            {
                await _rdvServices.CreateRdv(dentisteName, clientId, consultation, date);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }            

        }
        [HttpPost("cancel/{id}")]
        public async Task<IResult> Post(Guid id, [FromBody]string reason)
        {
            try
            {
                await _rdvServices.CancelRdv(id, reason);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        [HttpPost("payFor/{id}")]
        public async Task<IResult> Post(Guid id)
        {
            try
            {
                await _rdvServices.PayForRdv(id);
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
                await _writer.DeleteRendezVous(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
