namespace MojTaxi.Client.Models
{
    public class PaymentCard
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string BrandIcon { get; init; } = "\ue870"; // Material icon unicode
        public string BrandName { get; init; } = "Card";
        public string MaskedNumber { get; init; } = "**** **** **** 4242";
        public string CardHolder { get; init; } = "Edin Begović";
        public string Expiry { get; init; } = "12/27";
        public bool IsDefault { get; set; } = false;
    }
}
