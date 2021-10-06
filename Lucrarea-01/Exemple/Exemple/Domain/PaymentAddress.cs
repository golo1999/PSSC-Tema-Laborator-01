﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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