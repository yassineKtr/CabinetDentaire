using DataAccess.Writers.Consultations;
using DataAccess.Writers.Dentistes;
using Microsoft.AspNetCore.Mvc;
using Services.DataSeed;

namespace Controllers.Controllers
{
    [Route("api/seed")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly IWriteConsultation _consultationWriter;
        private readonly IWriteDentiste _dentisteWriter;

        public DataSeedController(IWriteConsultation consultationWriter, IWriteDentiste dentisteWriter)
        {
            _consultationWriter = consultationWriter;
            _dentisteWriter = dentisteWriter;
        }
        [HttpPost]
        public async Task<IResult> Post()
        {

            try
            {
                await DataSeed.SeedDentiste( _dentisteWriter);
                await DataSeed.SeedConsultation(_consultationWriter);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
