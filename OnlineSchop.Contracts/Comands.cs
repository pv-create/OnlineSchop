using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchop.Contracts.Comands
{
    public record ValidateClient
    {
        public Guid OrderId { get; init; }
        public Guid ClientId { get; init; }
    }

    public record ValidateOrder
    {
        public Guid OrderId { get; init; }
        public decimal TotalAmount { get; init; }
    }
}