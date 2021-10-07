using System.Text.RegularExpressions;

namespace Lucrarea01.Domain
{
    public record ProductID
    {
        private static readonly Regex ValidPattern = new("^LO[0-9]{5}$");

        public string Value { get; }

        private ProductID(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductID("Wrong Input");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
