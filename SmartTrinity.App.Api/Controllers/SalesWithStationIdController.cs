using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.App.Migrations.Core.Models;
using SmartTrinity.App.Migrations.Services;

namespace SmartTrinity.App.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    public class SalesWithStationIdController : ControllerBase
    {

        private readonly ISalesService _salesService;
        
        public SalesWithStationIdController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Retrieves X quantity of sales 
        /// </summary>
        /// <response code="200">List of sales</response>
        /// <response code="500">Error getting sales</response>
        [HttpGet]
        [Route("GetSalesWithStationIdAsync")]
        [ProducesResponseType(typeof(IEnumerable<Sale>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDetails), 500)]
        public async Task<ActionResult> GetSalesWithStationIdAsync()
        {
            try
            {
                return Ok(await _salesService.GetSalesWithStationIdAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class ErrorResponseDetails
        {
            public string Message { get; set; }
        }
    }
}
