namespace BlazorApp1.Pages;

using BlazorApp.Services;

using Microsoft.AspNetCore.Components;

using WebApplicationMvc.Models;

public partial class FetchData
{
    private CancellationTokenSource? tokenSource;

    private List<Address>? addresses;

    private bool? errorDelete;

    [Inject]
    public AddressesApiService AddressesApiService { get; set; } = null!;

    [Inject]    
    public NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        this.NavigationManager.LocationChanged += (_, _) => this.tokenSource?.Cancel();
        await this.LoadData();
    }

    private async Task LoadData()
    {
        this.tokenSource = new(TimeSpan.FromSeconds(3));
        this.addresses = await this.AddressesApiService.GetAllAdresses(this.tokenSource.Token);
    }

    private async Task DeleteAddressAsync(int id)
    {
        CancellationTokenSource token = new(TimeSpan.FromSeconds(3));
        this.errorDelete = await this.AddressesApiService.DeleteAddressAsync(id, token.Token);
        if (this.errorDelete is true)
            _ = this.addresses?.RemoveAll(address => address.Id == id);
    }
}
