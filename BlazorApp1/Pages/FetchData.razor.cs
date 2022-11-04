namespace BlazorApp1.Pages;

using BlazorApp1.Data;

using Microsoft.AspNetCore.Components;

using WebApplicationMvc.Services;

public partial class FetchData
{
    private WeatherForecast[]? forecasts;

    [Inject]
    public AddressesApiService AddressesApiService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        this.forecasts = await this.ForecastService.GetForecastAsync(DateTime.Now);
    }
}
