using MassTransit;
using OnlineSchop.Contracts.Events.Contracts.Events;
using OnlineSchop.OrderService.Saga;
using OnlineSchop.Contracts.Comands;

namespace OrderService.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        private readonly ILogger<OrderStateMachine> _logger;

        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            Event(() => OrderStarted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => ClientValidated, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderValidated, x => x.CorrelateById(m => m.Message.OrderId));

            Initially(
                When(OrderStarted)
                    .Then(context =>
                    {
                        _logger.LogInformation("Starting new order saga: {OrderId}", context.Message.OrderId);
                        
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.ClientId = context.Message.ClientId;
                        context.Saga.TotalAmount = context.Message.TotalAmount;
                        context.Saga.OrderDate = context.Message.OrderDate;
                    })
                    .TransitionTo(AwaitingClientValidation)
                    .Publish(context => new ValidateClient
                    {
                        OrderId = context.Message.OrderId,
                        ClientId = context.Message.ClientId
                    })
            );

            During(AwaitingClientValidation,
                When(ClientValidated)
                    .Then(context => _logger.LogInformation(
                        "Client validation completed for order: {OrderId}, IsValid: {IsValid}", 
                        context.Message.OrderId, 
                        context.Message.IsValid))
                    .If(context => context.Message.IsValid,
                        binder => binder
                            .Then(context =>
                            {
                                context.Saga.IsClientValidated = true;
                            })
                            .TransitionTo(AwaitingOrderValidation)
                            .Publish(context => new ValidateOrder
                            {
                                OrderId = context.Message.OrderId,
                                TotalAmount = context.Saga.TotalAmount
                            }))
                    .If(context => !context.Message.IsValid,
                        binder => binder
                            .Then(context =>
                            {
                                context.Saga.FailureReason = "Client validation failed";
                                _logger.LogWarning(
                                    "Client validation failed for order: {OrderId}", 
                                    context.Message.OrderId);
                            })
                            .TransitionTo(Failed)
                            .Publish(context => new OrderFailed
                            {
                                OrderId = context.Message.OrderId,
                                Reason = "Client validation failed"
                            }))
            );

            During(AwaitingOrderValidation,
                When(OrderValidated)
                    .Then(context => _logger.LogInformation(
                        "Order validation completed for order: {OrderId}, IsValid: {IsValid}", 
                        context.Message.OrderId, 
                        context.Message.IsValid))
                    .If(context => context.Message.IsValid,
                        binder => binder
                            .Then(context =>
                            {
                                context.Saga.IsOrderValidated = true;
                            })
                            .TransitionTo(Completed)
                            .Publish(context => new OrderCompleted
                            {
                                OrderId = context.Message.OrderId,
                                Status = "Completed"
                            }))
                    .If(context => !context.Message.IsValid,
                        binder => binder
                            .Then(context =>
                            {
                                context.Saga.FailureReason = "Order validation failed";
                                _logger.LogWarning(
                                    "Order validation failed for order: {OrderId}", 
                                    context.Message.OrderId);
                            })
                            .TransitionTo(Failed)
                            .Publish(context => new OrderFailed
                            {
                                OrderId = context.Message.OrderId,
                                Reason = "Order validation failed"
                            }))
            );

            SetCompletedWhenFinalized();
        }

        // Состояния
        public State AwaitingClientValidation { get; private set; }
        public State AwaitingOrderValidation { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        // События
        public Event<OrderStarted> OrderStarted { get; private set; }
        public Event<ClientValidated> ClientValidated { get; private set; }
        public Event<OrderValidated> OrderValidated { get; private set; }
    }
}