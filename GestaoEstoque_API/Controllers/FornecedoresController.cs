using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    public class FornecedoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
