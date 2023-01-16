namespace PaymentSystem.Models.Stripe;

public record AddStripePayment(string CostumerId, string ReceiptEmail, string Description, string Currency, long Amount);