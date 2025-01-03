using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace OnlineSchop.OrderService.Saga
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? IsClientValidated { get; set; }
        public bool? IsOrderValidated { get; set; }
        public string FailureReason { get; set; } = string.Empty;
    }
}