using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using Klika.Dinero.Model.DTO.Transaction.Request;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Model.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Klika.Dinero.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService; 
            _logger = logger;
        }
        
        /// <summary>
        /// Parses and inserts list of transactions from csv file
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Upload([FromForm] TransactionUploadRequestDTO dto)
        {
            try
            {
                dto.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var validationResponse = await _transactionService.ValidateCsvTransactionsFileAsync(dto.File).ConfigureAwait(false);

                if (!validationResponse.Succeeded)
                {
                    switch (validationResponse.ErrorCode)
                    {        
                        case ErrorCodes.UnsupportedContentType:
                            return StatusCode(StatusCodes.Status415UnsupportedMediaType, validationResponse.GetErrorResponse());
                        case ErrorCodes.PayloadTooLarge:
                            return StatusCode(StatusCodes.Status413PayloadTooLarge, validationResponse.GetErrorResponse());
                        default:
                            return BadRequest(validationResponse.GetErrorResponse());
                    }
                }
                
                var response = await _transactionService.ParseCsvTransactionsAsync(dto);
                
                if(response.Succeeded)
                    return Ok(response);

                return BadRequest(response.GetErrorResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/Transactions/upload");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Finds transaction categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(TransactionCategoryListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionCategoryListResponse>> GetTransactionCategories()
        {
            try
            {
                var result = await _transactionService.GetTransactionCategoriesAsync().ConfigureAwait(false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Transactions/categories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Adds single transaction to account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TransactionResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TransactionResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<TransactionCategoryListResponse>> CreateTransaction([FromBody] TransactionRequestDTO dto)
        {
            try
            {
                dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                var result = await _transactionService.CreateTransactionAsync(dto).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    switch (result.ErrorCode)
                    {
                        case ErrorCodes.NotFound:
                            return NotFound(result.GetErrorResponse());
                        default:
                            return BadRequest(result.GetErrorResponse());
                    }   
                }
                
                return CreatedAtAction(nameof(GetTransactions), new { transactionId = result.Transaction.Id}, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Transactions/createTransaction");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets transactions with filtering
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TransactionsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionsResponseDTO>> GetTransactions([FromQuery] TransactionParametersRequestDTO request)
        {
            try
            {                
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _transactionService.GetTransactionsByFiltersAsync(request).ConfigureAwait(false);

                if(result.Transactions.Count == 0)
                    return NotFound();

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GET:/Transactions");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets transactions CSV
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ExportTransactions([FromQuery] TransactionCSVExportRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _transactionService.GetTransactionsForExportAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return result.File;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Transactions/export");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Sends transactions CSV to mail
        /// </summary>
        /// <returns></returns>
        [HttpGet("export/to-mail")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult ExportTransactionsToMail([FromQuery] TransactionCSVExportRequestDTO request)
        {
            try
            {
                request.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                BackgroundJob.Enqueue(() => _transactionService.ExportTransactionsToMailAsync(request));

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Transactions/export/to-mail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}