using Microsoft.AspNetCore.Mvc;
using FundTransfer.Services;

namespace FundTransfer.Controllers;

[ApiController]
[Route("api")]
public class FundTransferController : ControllerBase
{
    private readonly ITransferService _transferService;
    private readonly ILogger<TransferService> _logger;

    public FundTransferController(ITransferService transferService, ILogger<TransferService> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    [HttpPost("fund-transfer")]
    public IActionResult TransferFunds(TransactionRequest transaction)
    {
        var transactionId = _transferService.InitiateTransfer(transaction.GetTransaction());
        return Ok(new { TransactionId = transactionId });
    }

    [HttpGet("fund-transfer/{transactionId}")]
    public IActionResult GetTransferStatus(string transactionId)
    {
        var statusResponse = _transferService.GetTransferStatus(transactionId);
        return Ok(statusResponse);
    }
}
