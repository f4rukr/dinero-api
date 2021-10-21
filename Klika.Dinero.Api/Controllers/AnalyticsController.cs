using Klika.Dinero.Model.DTO.Analytic.Request;
using Klika.Dinero.Model.DTO.Analytic.Response;
using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Model.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Klika.Dinero.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticService _analyticService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticService analyticService, ILogger<AnalyticsController> logger)
        {
            _analyticService = analyticService;
            _logger = logger;
        }

        /// <summary>
        /// Finds monthly income and expense
        /// </summary>
        /// <returns></returns>
        [HttpGet("expenses")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IncomeExpenseResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<IncomeExpenseResponseDTO>> GetIncomeExpense([FromQuery] AnalyticRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _analyticService.GetIncomeExpenseAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Analytics/expenses");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Finds monthly expense by categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("expenses/by-category")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PieCharListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<PieCharListResponse>> GetExpenseByCtegories([FromQuery] AnalyticRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _analyticService.GetExpenseByCategoriesAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Analytics/expenses/by-category");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Finds list of expenses for every day of the requested month
        /// </summary>
        /// <returns></returns>
        [HttpGet("expenses/daily-month")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ExpenseDailyMonthListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ExpenseDailyMonthListResponse>> GetExpenseDailyMonth([FromQuery] AnalyticRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
                var result = await _analyticService.GetExpenseDailyMonthAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Analytics/expenses/daily-month");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Finds list of all expenses for the given date 
        /// </summary>
        /// <returns></returns>
        [HttpGet("expenses/daily")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ExpenseDailyListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ExpenseDailyListResponse>> GetExpenseDaily([FromQuery] AnalyticRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              
                var result = await _analyticService.GetExpenseDailyAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Analytics/expenses/daily");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
