﻿using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Cart;

namespace Exemple
{
    class Program
    {
        static void Main(string[] args)
        {
            var productsList = ReadProductsList().ToArray();

            var cartDetails = GetCartDetails();

            UnvalidatedCart unvalidatedCart = new(productsList, cartDetails);

            ICart result = ValidateCart(unvalidatedCart);

            result.Match(
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenEmptyCart: invalidResult => invalidResult,
                whenInvalidatedCart: invalidResult => invalidResult,
                whenValidatedCart: validatedCart => PaidCart(validatedCart, cartDetails),
                whenPaidCart: paidCart => paidCart
                );

            Console.WriteLine(result);
        }

        private static ICart ValidateCart(UnvalidatedCart unvalidatedCart) =>
            unvalidatedCart.ProductsList.Count == 0 ?
                // if the cart is empty
                new EmptyCart(new List<UnvalidatedProducts>(), "Empty cart") :
                    // if the cart isn't empty
                    (string.IsNullOrEmpty(unvalidatedCart.CartDetails.PaymentAddress.Value) ?
                        // if there is no address
                        new InvalidatedCart(new List<UnvalidatedProducts>(), "Invalid cart") :
                            // if there is an address
                            (unvalidatedCart.CartDetails.PaymentState.Value == 0 ?
                                // if the order hasn't been paid yet
                                new ValidatedCart(new List<ValidatedProducts>(), unvalidatedCart.CartDetails) :
                                    // if the order has been paid
                                    new PaidCart(new List<ValidatedProducts>(), unvalidatedCart.CartDetails, DateTime.Now)));

        private static ICart PaidCart(ValidatedCart validatedResult, CartDetails cartDetails) =>
                new PaidCart(new List<ValidatedProducts>(), cartDetails, DateTime.Now);

        private static List<UnvalidatedProducts> ReadProductsList()
        {
            List<UnvalidatedProducts> productsList = new();

            object? answer;

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

                    UnvalidatedProducts productToBeAdded = new(ProdusID, ProdusCantitate);

                    productsList.Add(productToBeAdded);
                    Console.Write("\n");
                }

            } while (answer != null && !answer.Equals("N"));

            return productsList;
        }

        public static CartDetails GetCartDetails()
        {
            PaymentState paymentState;

            PaymentAddress paymentAddress;

            CartDetails cartDetails;

            string? answer = ReadValue("\nDo you want to proceed to checkout? [Y/N]: ");

            if (answer != null && answer.Contains("Y"))
            {
                var address = ReadValue("\nPlease type your address: ");

                paymentAddress = new PaymentAddress(string.IsNullOrEmpty(address) ? "No address" : address);

                var payment = ReadValue("\nDo you want to pay now? [Y/N]: ");

                paymentState = new PaymentState(payment != null && payment.Contains("Y") ? 1 : 0);
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
