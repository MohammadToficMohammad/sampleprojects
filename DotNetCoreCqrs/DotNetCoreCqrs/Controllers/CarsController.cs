using DotNetCoreCqrs.CQRS;
using DotNetCoreCqrs.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private ICqrsAggregate cqrsCommandsHandler;
        private ICqrsProjection cqrsQueriesHandler;

        public CarsController(ICqrsAggregate _cqrsCommandsHandler, ICqrsProjection _cqrsQueriesHandler) 
        {
            cqrsCommandsHandler = _cqrsCommandsHandler;
            cqrsQueriesHandler = _cqrsQueriesHandler;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            CqrsQuery query = new CqrsQuery(null,CQRSQUERYTYPE.GETALLCARS);
            return new OkObjectResult(await cqrsQueriesHandler.HandleAsync<IList<Car>>(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var eventData = new EventData(new List<object>() { id });
            CqrsQuery query = new CqrsQuery(eventData, CQRSQUERYTYPE.GETCAR);
            return new OkObjectResult(await cqrsQueriesHandler.HandleAsync<Car>(query));
        }


        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            var eventData = new EventData(new List<object>() { car });
            CqrsCommand cmd = new CqrsCommand(eventData, CQRSCOMMANDTYPE.ADDCAR);
            await cqrsCommandsHandler.HandleAsync(cmd);
            return Ok("CAR added");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var eventData = new EventData(new List<object>() { id });
            CqrsCommand cmd = new CqrsCommand(eventData, CQRSCOMMANDTYPE.DELETECAR);
            await cqrsCommandsHandler.HandleAsync(cmd);
            return Ok("CAR deleted");
        }
    }
}
