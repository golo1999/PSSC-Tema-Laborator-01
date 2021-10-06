using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record UnvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> ProductsList, CartDetails CartDetails) : ICart;

        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> ProductsList, string reason) : ICart;

        public record EmptyCart(IReadOnlyCollection<UnvalidatedProduct> ProductsList, string reason) : ICart;

        public record ValidatedCart(IReadOnlyCollection<ValidatedProduct> ProductsList, CartDetails CartDetails) : ICart;

        public record PaidCart(IReadOnlyCollection<ValidatedProduct> ProductsList, CartDetails CartDetails, DateTime PublishedDate) : ICart;
    }
}