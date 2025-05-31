using cw11.Data;
using cw11.Dbo;
using cw11.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw11.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var trips = await _tripsService.getTripsSortedByDate(page, pageSize);
            return Ok(trips);
        }

        [HttpPost("{IdTrip}/clients")]
        public async Task<IActionResult> Post(int IdTrip, [FromBody] clientPOST clientPOST)
        {
            try
            {
                await _tripsService.PutClientIntoTrip(IdTrip, clientPOST);
                return Created("", "utworzone");
            }
            catch (ConflictEX e)
            {
                return Conflict(e.Message);
            }
            catch (NotFoundEx e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
