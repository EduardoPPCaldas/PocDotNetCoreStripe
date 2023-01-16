namespace PaymentSystem.Models.Stripe;

public record StripePayment(
    string CustomerId,
    string ReceiptId,
    string Description,
    string Currency,
    long Amount,
    string PaymentId
);