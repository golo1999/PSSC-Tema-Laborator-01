namespace Exemple.Domain
{
    public record PaymentAddress
    {
        public string Value { get; }

        public PaymentAddress(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}