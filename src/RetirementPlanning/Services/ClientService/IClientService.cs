using RetirementPlanning.Models;

namespace RetirementPlanning.Services.ClientService
{
    public interface IClientService
    {
        List<Client> Clients { get; set; }
        List<AccountIfo> AccountIfos { get; set; }
        Task GetAccountIfos();
        Task GetClients();
        Task<RetirementPlanning.Models.Client> GetSingleClient(int id);
        Task CreateClient(RetirementPlanning.Models.Client client);
        Task UpdateClient(RetirementPlanning.Models.Client client);
        Task DeleteClient(int id);
    }
}
