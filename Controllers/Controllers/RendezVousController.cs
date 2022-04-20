using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Rdv;

namespace Controllers.Controllers
{
    [Route("api/rendezvous")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly IRdvServices _rdvServices;

        public RendezVousController(IRdvServices rdvServices)
        {
            _rdvServices = rdvServices;
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
    }
}
