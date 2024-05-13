using Microsoft.AspNetCore.Mvc;
using SmartTrinity.App.Sales.Services;
using Microsoft.AspNetCore.Authorization;
using SmartTrinity.App.Sales.Core.Models;
using Asp.Versioning;

namespace  SmartTrinity.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesService _salesService;

        public SalesController(ILogger<SalesController> logger, ISalesService salesService)
        {
            _logger = logger;
            _salesService = salesService;
        }

        /// <summary>
        /// Retrieves X quantity of sales 
        /// </summary>
        /// <response code="200">List of sales</response>
        /// <response code="500">Error getting sales</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sale>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDetails), 500)]
        public async Task<ActionResult> GetSales()
        {
            try
            {
                return Ok(await _salesService.GetSales());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ErrorResponseDetails
    {
        public string Message { get; set; }
    }
}