using ASP_LABS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_LABS.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Lab_1()
        {
            ViewBag.Lab = "Лабораторная работа 2";
            List<ListDemo> list = new List<ListDemo>
            {
                new ListDemo() { Id = 1, Name = "name1" },
                new ListDemo() { Id = 2, Name = "name2" },
                new ListDemo() { Id = 3, Name = "name3" }
            };
            ViewBag.List = new SelectList(list, "Id", "Name");


            return View();
        }


    }
}
