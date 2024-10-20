using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    public class EstoqueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
