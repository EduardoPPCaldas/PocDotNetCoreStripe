using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Contracts;
using PaymentSystem.Models.Stripe;

namespace PaymentSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeAppService _stripeAppService;

    public StripeController(IStripeAppService stripeAppService)
    {
        _stripeAppService = stripeAppService;
    }

    [HttpPost("customer/add")]
    public async Task<ActionResult<StripeCustomer>> AddStripeCustomer(
        [FromBody] AddStripeCustomer customer,
        CancellationToken cancellationToken)
    {
        var createdCustomer = await _stripeAppService.AddStripeCustomerAsync(customer, cancellationToken);

        return Ok(createdCustomer);
    }

    [HttpPost("payment/add")]
    public async Task<ActionResult<StripePayment>> AddStripePayment(
        [FromBody] AddStripePayment payment,
        CancellationToken cancellationToken)
    {
        var createdPayment = await _stripeAppService.AddStripePaymentAsync(payment, cancellationToken);

        return Ok(createdPayment);
    }
}