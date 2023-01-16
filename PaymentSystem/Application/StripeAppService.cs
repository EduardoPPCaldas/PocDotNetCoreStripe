using PaymentSystem.Contracts;
using PaymentSystem.Models.Stripe;
using Stripe;

namespace PaymentSystem.Application;

public class StripeAppService : IStripeAppService
{
    private readonly ChargeService _chargeService;
    private readonly CustomerService _customerService;
    private readonly TokenService _tokenService;

    public StripeAppService(
        ChargeService chargeService,
        CustomerService customerService,
        TokenService tokenService)
    {
        _chargeService = chargeService;
        _customerService = customerService;
        _tokenService = tokenService;
    }
    
    public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken cancellationToken)
    {
        var tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = customer.Name,
                Number = customer.CreditCard.CardNumber,
                ExpYear = customer.CreditCard.ExpirationYear,
                ExpMonth = customer.CreditCard.ExpirationMonth,
                Cvc = customer.CreditCard.Cvc
            }
        };

        var stripeToken = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

        var customerOptions = new CustomerCreateOptions
        {
            Name = customer.Name,
            Email = customer.Email,
            Source = stripeToken.Id
        };

        var createdCustomer = await _customerService.CreateAsync(customerOptions, null, cancellationToken);

        return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
    }

    public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken cancellationToken)
    {
        var paymentOptions = new ChargeCreateOptions
        {
            Customer = payment.CostumerId,
            ReceiptEmail = payment.ReceiptEmail,
            Description = payment.Description,
            Currency = payment.Currency,
            Amount = payment.Amount
        };

        var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, cancellationToken);

        return new StripePayment(
            createdPayment.CustomerId,
            createdPayment.ReceiptEmail,
            createdPayment.Description,
            createdPayment.Currency,
            createdPayment.Amount,
            createdPayment.Id);
    }
}