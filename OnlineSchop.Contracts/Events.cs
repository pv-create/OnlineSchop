using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchop.Contracts.Events
{
    namespace Contracts.Events
{
    public record OrderStarted
    {
        public Guid OrderId { get; init; }
        public Guid ClientId { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime OrderDate { get; init; }
    }

    public record ClientValidated
    {
        public Guid OrderId { get; init; }
        public Guid ClientId { get; init; }
        public bool IsValid { get; init; }
    }

    public record OrderValidated
    {
        public Guid OrderId { get; init; }
        public bool IsValid { get; init; }
    }

    public record OrderCompleted
    {
        public Guid OrderId { get; init; }
        public string Status { get; init; } = string.Empty;
    }

    public record OrderFailed
    {
        public Guid OrderId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
}