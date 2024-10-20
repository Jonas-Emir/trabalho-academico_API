using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    public class ProdutosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
