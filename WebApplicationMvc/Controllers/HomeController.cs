namespace WebApplicationMvc.Controllers;

using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using WebApplicationMvc.Models;
using WebApplicationMvc.Services;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    private readonly AddressesApiService addressesApiService;

    public HomeController(ILogger<HomeController> logger, AddressesApiService addressesApiService)
    {
        this.logger = logger;
        this.addressesApiService = addressesApiService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var addresses = await this.addressesApiService.GetAllAdresses(cancellationToken);
        return this.View(addresses);
    }

    public IActionResult Privacy()
        => this.View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}
