using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
