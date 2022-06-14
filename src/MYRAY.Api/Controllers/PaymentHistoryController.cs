using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYRAY.Api.Constants;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.PaymentHistory;
using MYRAY.Business.Enums;
using MYRAY.Business.Services;
using MYRAY.DataTier.Entities;

namespace MYRAY.Api.Controllers;
/// <summary>
/// Handle request related to payment history
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes(MediaType.ApplicationJson)]
[Produces(MediaType.ApplicationJson)]
public class PaymentHistoryController : ControllerBase
{
    private readonly IPaymentHistoryService _paymentHistoryService;

    public PaymentHistoryController(IPaymentHistoryService paymentHistoryService)
    {
        _paymentHistoryService = paymentHistoryService;
    }

    /// <summary>
    /// [Authenticated user] Endpoint for get all payment history with condition
    /// </summary>
    /// <param name="searchPaymentHistory">An object contains filter criteria.</param>
    /// <param name="sortingDto">An object contains sorting criteria.</param>
    /// <param name="pagingDto">An object contains paging criteria.</param>
    /// <param name="accountId">Id of account to view</param>
    /// <returns>List of payment history</returns>
    /// <response code="200">Returns the list of payment history.</response>
    /// <response code="204">Returns if list of payment history is empty.</response>
    /// <response code="403">Returns if token is access denied.</response>
    [HttpGet("{accountId}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto.CollectiveResponse<PaymentHistory>),StatusCodes.Status200OK)]
    public Task<IActionResult> GetGuidepost(
        [FromQuery] SearchPaymentHistory searchPaymentHistory,
        [FromQuery] SortingDto<PaymentHistoryEnum.PaymentHistorySortCriteria> sortingDto,
        [FromQuery] PagingDto pagingDto,
        int accountId)
    {
        var result =  _paymentHistoryService.GetPaymentHistory(searchPaymentHistory, pagingDto, sortingDto, accountId);
        if (result == null)
        {
            return Task.FromResult<IActionResult>(NoContent());
        }

        return Task.FromResult<IActionResult>(Ok(result));
    }
    
}