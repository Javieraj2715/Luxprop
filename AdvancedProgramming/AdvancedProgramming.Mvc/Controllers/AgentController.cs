using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

[Authorize(Roles = "Agent,Admin")]
public class AgentController : Controller
{
    // Views/Agent/Index.cshtml
    public IActionResult Index() => View("~/Views/Agent/Index.cshtml");

    // Views/Dashboard/Dashboard.cshtml
    public IActionResult Dashboard() => View("~/Views/Dashboard/Dashboard.cshtml");

    // Views/Documents/Documents.cshtml
    public IActionResult Documents() => View("~/Views/Documents/Documents.cshtml");

    // Views/Map/Map.cshtml
    public IActionResult Map() => View("~/Views/Map/Map.cshtml");
}
