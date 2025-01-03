using Microsoft.EntityFrameworkCore;
using OnlineSchop.ClientService.Data;
using OnlineSchop.ClientService.Interfaces;
using OnlineSchop.ClientService.Models;

namespace OnlineSchop.ClientService.Repositories
{
 
public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(ApplicationDbContext context, ILogger<ClientRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> ValidateClientAsync(Guid clientId)
    {
        try
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                _logger.LogWarning("Client not found: {ClientId}", clientId);
                return false;
            }

            // Проверяем различные условия валидации
            var isValid = client.IsActive && 
                         client.CreditLimit > 0 && 
                         !string.IsNullOrEmpty(client.Email);

            _logger.LogInformation(
                "Client {ClientId} validation result: {IsValid}", 
                clientId, 
                isValid);

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating client: {ClientId}", clientId);
            throw;
        }
    }

    public async Task<Client> GetClientByIdAsync(Guid clientId)
    {
        try
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                _logger.LogWarning("Client not found: {ClientId}", clientId);
            }

            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting client: {ClientId}", clientId);
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        try
        {
            return await _context.Clients
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all clients");
            throw;
        }
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        try
        {
            client.CreatedAt = DateTime.UtcNow;
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Client created: {ClientId}", client.Id);
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client");
            throw;
        }
    }

    public async Task<bool> UpdateClientAsync(Client client)
    {
        try
        {
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == client.Id);

            if (existingClient == null)
            {
                _logger.LogWarning("Client not found for update: {ClientId}", client.Id);
                return false;
            }

            // Обновляем свойства
            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.IsActive = client.IsActive;
            existingClient.CreditLimit = client.CreditLimit;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Client updated: {ClientId}", client.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating client: {ClientId}", client.Id);
            throw;
        }
    }

    public async Task<bool> DeleteClientAsync(Guid clientId)
    {
        try
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                _logger.LogWarning("Client not found for deletion: {ClientId}", clientId);
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Client deleted: {ClientId}", clientId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting client: {ClientId}", clientId);
            throw;
        }
    }
}
}