using OnlineSchop.ClientService.Models;

namespace OnlineSchop.ClientService.Interfaces
{
    public interface IClientRepository
{
    Task<bool> ValidateClientAsync(Guid clientId);
    Task<Client> GetClientByIdAsync(Guid clientId);
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task<Client> CreateClientAsync(Client client);
    Task<bool> UpdateClientAsync(Client client);
    Task<bool> DeleteClientAsync(Guid clientId);
}
}