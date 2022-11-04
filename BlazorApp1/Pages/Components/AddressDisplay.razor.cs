namespace BlazorApp1.Pages.Components;

using Microsoft.AspNetCore.Components;

using WebApplicationMvc.Models;

public partial class AddressDisplay
{
    [Parameter]
    public Address Address { get; set; } = new();

    [Parameter]
    public EventCallback<int> DeleteCallback { get; set; }

    private async void DeleteClick()
    {
        await this.DeleteCallback.InvokeAsync(this.Address.Id);
    }
}
