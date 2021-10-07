namespace Exemple.Domain
{
    public record PaymentState
    {
        public int Value { get; }

        public PaymentState(int value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}