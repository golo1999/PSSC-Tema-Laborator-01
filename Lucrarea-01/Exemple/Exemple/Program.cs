using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Cart;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var productsList = ReadProductsList().ToArray();

            var cartDetails = ReadDetails();

            UnvalidatedCart unvalidatedCart = new(productsList, cartDetails);

            ICart result = checkCart(unvalidatedCart);

            result.Match(
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenEmptyCart: invalidResult => invalidResult,
                whenInvalidatedCart: invalidResult => invalidResult,
                whenValidatedCart: validatedCart => paidCart(validatedCart, cartDetails, DateTime.Now),
                whenPaidCart: paidCart => paidCart
                );

            Console.WriteLine(result);
        }

        private static ICart checkCart(UnvalidatedCart unvalidatedCart) => ((unvalidatedCart.ProductsList.Count == 0) ?
            new EmptyCart(new List<UnvalidatedProduct>(), "Empty cart") :
                ((string.IsNullOrEmpty(unvalidatedCart.CartDetails.PaymentAddress.Value)) ?
                    new InvalidatedCart(new List<UnvalidatedProduct>(), "Invalid cart") :
                        ((unvalidatedCart.CartDetails.PaymentState.Value == 0) ?
                            new ValidatedCart(new List<ValidatedProduct>(), unvalidatedCart.CartDetails) :
                                new PaidCart(new List<ValidatedProduct>(), unvalidatedCart.CartDetails, DateTime.Now))));

        private static ICart paidCart(ValidatedCart validatedResult, CartDetails cartDetails, DateTime PublishedDate) =>
                new PaidCart(new List<ValidatedProduct>(), cartDetails, DateTime.Now);

        private static List<UnvalidatedProduct> ReadProductsList()
        {
            List<UnvalidatedProduct> productsList = new();

            object? answer = null;

            do
            {
                answer = ReadValue("Do you want to add a new product? [Y/N]: ");

                if (answer != null && answer.Equals("Y"))
                {
                    var ProdusID = ReadValue("\nProduct id: ");

                    if (string.IsNullOrEmpty(ProdusID))
                    {
                        break;
                    }

                    var ProdusCantitate = ReadValue("Product quantity: ");

                    if (string.IsNullOrEmpty(ProdusCantitate))
                    {
                        break;
                    }

                    UnvalidatedProduct productToBeAdded = new(ProdusID, ProdusCantitate);

                    productsList.Add(productToBeAdded);
                    Console.Write("\n");
                }

            } while (answer != null && !answer.Equals("N"));

            return productsList;
        }

        public static CartDetails ReadDetails()
        {
            PaymentState paymentState;

            PaymentAddress paymentAddress;

            CartDetails cartDetails;

            string? answer = ReadValue("\nDo you want to proceed to checkout? [Y/N]: ");

            if (answer != null && answer.Contains("Y"))
            {
                var address = ReadValue("\nPlease type your address: ");

                if (string.IsNullOrEmpty(address))
                {
                    paymentAddress = new PaymentAddress("No address");
                }
                else
                {
                    paymentAddress = new PaymentAddress(address);
                }

                var payment = ReadValue("\nDo you want to pay now? [Y/N]: ");

                if (payment != null && payment.Contains("Y"))
                {
                    paymentState = new PaymentState(1);
                }
                else
                {
                    paymentState = new PaymentState(0);
                }
            }
            else
            {
                paymentAddress = new PaymentAddress("No address");
                paymentState = new PaymentState(0);
            }

            cartDetails = new CartDetails(paymentAddress, paymentState);

            Console.Write("\n");

            return cartDetails;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
