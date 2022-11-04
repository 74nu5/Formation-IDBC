namespace WebApplicationMvc.Services;

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

    public async Task<IEnumerable<Address>?> GetAllAdresses(CancellationToken cancellationToken)
    {
        try
        {
            var message = await this.client.GetAsync(string.Empty, cancellationToken);
            return await message.Content.ReadFromJsonAsync<IEnumerable<Address>>(cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Erreur lors de la récupération des adresses.");
            return new List<Address>();
        }
    }


}
