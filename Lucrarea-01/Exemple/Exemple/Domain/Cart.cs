using CSharp.Choices;
using System;
using System.Collections.Generic;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record UnvalidatedCart(IReadOnlyCollection<UnvalidatedProducts> ProductsList, CartDetails CartDetails) : ICart;

        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedProducts> ProductsList, string Message) : ICart;

        public record EmptyCart(IReadOnlyCollection<UnvalidatedProducts> ProductsList, string Message) : ICart;

        public record ValidatedCart(IReadOnlyCollection<ValidatedProducts> ProductsList, CartDetails CartDetails) : ICart;

        public record PaidCart(IReadOnlyCollection<ValidatedProducts> ProductsList, CartDetails CartDetails, DateTime PaidDate) : ICart;
    }
}