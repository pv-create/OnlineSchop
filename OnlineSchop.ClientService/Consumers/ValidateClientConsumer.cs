using MassTransit;
using OnlineSchop.ClientService.Interfaces;
using OnlineSchop.Contracts.Comands;
using OnlineSchop.Contracts.Events.Contracts.Events;

namespace OnlineSchop.ClientService.Consumers
{
    public class ValidateClientConsumer : IConsumer<ValidateClient>
{
    private readonly ILogger<ValidateClientConsumer> _logger;
    private readonly IClientRepository _clientRepository;

    public ValidateClientConsumer(ILogger<ValidateClientConsumer> logger, IClientRepository clientRepository)
    {
        _logger = logger;
        _clientRepository = clientRepository;
    }

    public async Task Consume(ConsumeContext<ValidateClient> context)
    {
        _logger.LogInformation("Validating client: {ClientId}", context.Message.ClientId);

        var isValid = await _clientRepository.ValidateClientAsync(context.Message.ClientId);

        await context.Publish(new ClientValidated
        {
            OrderId = context.Message.OrderId,
            ClientId = context.Message.ClientId,
            IsValid = isValid
        });
    }
}
}