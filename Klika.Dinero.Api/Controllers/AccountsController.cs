using Klika.Dinero.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Response;
using Klika.Dinero.Model.DTO.Account.Response;
using Klika.Dinero.Model.DTO.Account.Request;

namespace Klika.Dinero.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Creates account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AccountResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateAccount([FromBody] AccountRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _accountService.CreateAccountAsync(request).ConfigureAwait(false);

                if (!result.Succeeded)
                    switch (result.ErrorCode)
                    {
                        case ErrorCodes.NotFound:
                            return NotFound(result.GetErrorResponse());
                        case ErrorCodes.InvalidFormat:
                            return BadRequest(result.GetErrorResponse());
                        case ErrorCodes.AlreadyExist:
                            return Conflict(result.GetErrorResponse());
                    }

                return CreatedAtAction(nameof(GetAccount), new {accountNumber = result.AccountNumber}, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/Accounts");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Finds account by accountNumber
        /// </summary>
        /// <param accountNumber="accountNumber"></param>
        /// <returns></returns>
        [HttpGet("{accountNumber}")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AccountResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountResponseDTO>> GetAccount(string accountNumber)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _accountService.GetAccountAsync(accountNumber, userId).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Accounts/{accountNumber}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Finds accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AccountListResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountListResponse>> GetAccounts()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _accountService.GetAccountsAsync(userId).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Accounts");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes account by accountNumber
        /// </summary>
        /// <param accountNumber="accountNumber"></param>
        /// <returns></returns>
        [HttpDelete("{accountNumber}")]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAccount(string accountNumber)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _accountService.DeleteAccountAsync(accountNumber, userId).ConfigureAwait(false);

                if (!result.Succeeded)
                    return NotFound(result.GetErrorResponse());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DELETE:/Accounts/{accountNumber}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
