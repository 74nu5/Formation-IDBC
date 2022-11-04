namespace BlazorApp.Services;

using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using WebApplicationMvc.Models;

public class AddressesApiService
{
    private readonly ILogger<AddressesApiService> logger;

    private readonly HttpClient client;

    public AddressesApiService(ILogger<AddressesApiService> logger, IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.client = httpClientFactory.CreateClient("Addresses");
    }

    public async Task<List<Address>> GetAllAdresses(CancellationToken cancellationToken)
    {
        try
        {
            var message = await this.client.GetAsync(string.Empty, cancellationToken);
            var addresses = await message.Content.ReadFromJsonAsync<IEnumerable<Address>>(cancellationToken: cancellationToken);
            
            return addresses is null ? new() : addresses.ToList();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Erreur lors de la récupération des adresses.");
            return new();
        }
    }

    public async Task<bool> DeleteAddressAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var message = await this.client.DeleteAsync(id.ToString(), cancellationToken);
            return message.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Erreur lors de la récupération des adresses.");
            return false;
        }
    }
}
