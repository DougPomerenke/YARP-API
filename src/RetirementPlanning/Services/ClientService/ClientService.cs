using Microsoft.AspNetCore.Components;
using RetirementPlanning.Models;
using System.Net.Http.Json;

namespace RetirementPlanning.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public ClientService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<Client> Clients { get; set; } = new List<Client>();
        public List<AccountIfo> AccountIfos { get; set; } = new List<AccountIfo>();
        public async Task GetAccountIfos() { }
        public async Task GetClients() { }
        public async Task<Client> GetSingleClient(int id)
        {
            var result = await _http.GetFromJsonAsync<Client>($"api/client/{id}");
            if (result != null)
                return result;
            throw new Exception("Client not found!");
        }
        public async Task CreateClient(Client client) { }
        public async Task UpdateClient(Client client) { }
        public async Task DeleteClient(int id)
        {
            var result = await _http.DeleteAsync($"api/client/{id}");
            await SetClients(result);
        }
        private async Task SetClients(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Client>>();
            Clients = response;
            _navigationManager.NavigateTo("clients");
        }
    }
}
