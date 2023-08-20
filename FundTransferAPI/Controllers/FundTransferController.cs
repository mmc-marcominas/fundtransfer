using Microsoft.AspNetCore.Mvc;
using FundTransfer.Services;

namespace FundTransfer.Controllers;

[ApiController]
[Route("api")]
public class FundTransferController : ControllerBase
{
    private readonly ITransactionsService _transferService;
    private readonly ILogger<FundTransferController> _logger;

    public FundTransferController(ITransactionsService transferService, ILogger<FundTransferController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    [HttpPost("fund-transfer")]
    public async Task<IActionResult> TransferFunds(TransactionRequest transaction)
    {
        LogOperation("FundTransferController.TransferFunds - transfer funds requested");
        var transactionId = await _transferService.InitiateTransfer(transaction.GetTransaction());
        LogOperation("FundTransferController.TransferFunds - transfer funds TransactionId delivered");
        return Ok(new { TransactionId = transactionId });
    }

    [HttpGet("fund-transfer/{transactionId}")]
    public async Task<IActionResult> GetTransferStatus(string transactionId)
    {
        LogOperation("FundTransferController.GetTransferStatus - get status requested");
        var statusResponse = await _transferService.GetTransactionStatus(transactionId);
        LogOperation("FundTransferController.GetTransferStatus - get status delivered");
        return Ok(statusResponse);
    }

    private void LogOperation(string operationDetails)
    {
        var requestId = HttpContext.GetRequestId();
        var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        _logger.LogInformation($"Operation: {operationDetails}, IP: {ipAddress}, Request ID: {requestId}");
    }
}
