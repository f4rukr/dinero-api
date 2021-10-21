using Klika.Dinero.Model.DTO.Bank.Response;
using Klika.Dinero.Model.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Klika.Dinero.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly ILogger<BanksController> _logger;

        public BanksController(IBankService bankService, ILogger<BanksController> logger)
        {
            _bankService = bankService;
            _logger = logger;
        }

        /// <summary>
        /// Finds banks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BankListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<BankListResponse>> GetBanks()
        {
            try
            {
                var result = await _bankService.GetBanksAsync().ConfigureAwait(false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Banks");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
