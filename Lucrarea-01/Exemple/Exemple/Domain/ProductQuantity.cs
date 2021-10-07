using System.Text.RegularExpressions;

namespace Lucrarea01.Domain
{
    public record  ProductQuantity
    {
        private static readonly Regex ValidPattern = new("^LO[0-9]{5}$");
        
        public string Value { get; }

        private ProductQuantity(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductQuantity("Wrong Input");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
