using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.Shared.Core.Services;
using Microsoft.AspNetCore.Authorization;
using SmartTrinity.App.Migrations.Core.Services;
using SmartTrinity.App.Sales.Services;
using SmartTrinity.App.Sales.Core.Models.Filters;

namespace SmartTrinity.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesService _salesService;
        private readonly ISalesMigrator _salesMigrator;
        private readonly ISmartSalesService _smartSalesService;

        public SalesController(ILogger<SalesController> logger, ISalesService salesService, ISmartSalesService smartSalesService)
        {
            _logger = logger;
            _salesService = salesService;
            //_salesMigrator = salesMigrator;
            _smartSalesService = smartSalesService;
        }

        /// <summary>
        /// Retrieves X quantity of sales 
        /// </summary>
        /// <response code="200">List of sales</response>
        /// <response code="500">Error getting sales</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sale>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDetails), 500)]
        public async Task<ActionResult> GetSales([FromQuery] SaleQueryFilter saleQueryFilter)
        {
            try
            {
                return Ok(await _smartSalesService.GetSales(saleQueryFilter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<ActionResult> Test()
        {
            await _salesMigrator.MigrateSalesToCentral();
            return Ok("Complete test...");
        }
    }

    public class ErrorResponseDetails
    {
        public string Message { get; set; }
    }
}