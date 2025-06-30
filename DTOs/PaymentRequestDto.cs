namespace theupskilzapi.DTOs
{
    public class PaymentRequestDto
    {
        public int Amount { get; set; }  // in INR paise
        public string Currency { get; set; } = "INR";
        public string Receipt { get; set; } = Guid.NewGuid().ToString();
    }
}
