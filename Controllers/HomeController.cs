using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AwsAssignmentDemo.Models;
using AwsAssignmentDemo.Services;

namespace AwsAssignmentDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseService _databaseService;

    public HomeController(ILogger<HomeController> logger, DatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    public async Task<IActionResult> Index()
    {
        var uploads = await _databaseService.GetUploadsAsync();

        return View(uploads);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
