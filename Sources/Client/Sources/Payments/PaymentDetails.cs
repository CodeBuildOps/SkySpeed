namespace SkySpeed.Payments
{
    internal class PaymentDetails
    {
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationDate { get; set; }
        public string CardHolderName { get; set; }

        public PaymentDetails(string paymentMethod, string cardNumber, string amount, string expirationMonth, string expirationYear, string cardHolderName)
        {
            PaymentMethod = paymentMethod;
            CardNumber = cardNumber;
            Amount = amount;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
            ExpirationDate = $"{expirationMonth} {expirationYear}";
            CardHolderName = cardHolderName;
        }
    }
}
