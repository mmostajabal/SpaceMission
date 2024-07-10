using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceMissionServices.Contracts.Temperature;
using SpaceMissionShared.DTO;
using SpaceMissionShared.Validation;

namespace SpaceMissionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceMissionController : ControllerBase
    {
        private ITemperature _temperatureSrv;
        /// <summary>
        /// SpaceMissionController
        /// </summary>
        /// <param name="temperatureSrv"></param>
        public SpaceMissionController(ITemperature temperatureSrv)
        {
            _temperatureSrv = temperatureSrv;
        }
        /// <summary>
        /// AddTemperature
        /// </summary>
        /// <param name="temperatureDTO"></param>
        /// <returns></returns>
        [HttpPost("AddTemperature")]
        public async Task<IActionResult> AddTemperature([FromBody] TemperatureDTO temperatureDTO)
        {
             var validator = new TemperatureValidator();
             var validationResult = validator.Validate(temperatureDTO);

             if (!validationResult.IsValid)
             {
                 return BadRequest(validationResult.Errors);
             }

             try
             {
                 bool result = _temperatureSrv.AddTemperature(temperatureDTO).GetAwaiter().GetResult();

                 return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// GetLatestTemperature
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLatestTemperature")]
        public async Task<IActionResult> GetLatestTemperature()
        {
            return Ok(_temperatureSrv.LastTemperature().GetAwaiter().GetResult());
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetLatestTemperature(int Id = 0)
        {
            return Ok(_temperatureSrv.GetAllTemperature(Id).GetAwaiter().GetResult());
        }

    }
}
