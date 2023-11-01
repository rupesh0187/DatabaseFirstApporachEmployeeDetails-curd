using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDetails.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeContext EmployeeDB;

        public HomeController(ILogger<HomeController> logger, EmployeeContext EmployeeDB)
        {
            _logger = logger;
            this.EmployeeDB = EmployeeDB;
        }

        public async Task<IActionResult> Index()
        {
            return EmployeeDB.Accounts != null ?
                View(await EmployeeDB.Accounts.ToListAsync()) :
                Problem("Entity set 'EmployeeContext.Accounts' is null.");
        }
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Account std)
        {
            if (ModelState.IsValid)
            {
                EmployeeDB.Accounts?.Add(std);
                EmployeeDB.SaveChanges();
                TempData["insert_success"] = "Inserted..";
                return RedirectToAction("Index", "Home");
            }
            return View(std);

        }
        public IActionResult Details(int? id)
        {
            if (id == null || EmployeeDB.Accounts == null)
            {
                return NotFound();
            }
            var stdData = EmployeeDB.Accounts.FirstOrDefault(s => s.UserId == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || EmployeeDB.Accounts == null)
            {
                return NotFound();
            }
            var stdData = EmployeeDB.Accounts.Find(id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);

        }
        [HttpPost]
        public IActionResult Edit(int? id, Account std)
        {
            if (id != std.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                EmployeeDB.Update(std);
                EmployeeDB.SaveChanges();
                TempData["update_success"] = "Updated..";
                return RedirectToAction("Index", "Home");
            }
            return View(std);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || EmployeeDB.Accounts == null)
            {
                return NotFound();
            }

            var stdData = EmployeeDB.Accounts.FirstOrDefault(s => s.UserId == id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);


        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfimed(int? id)
        {
            var stdData = EmployeeDB.Accounts?.Find(id);
            if (stdData != null)
            {
                EmployeeDB.Accounts?.Remove(stdData);
            }
            EmployeeDB.SaveChanges();
            TempData["delete_success"] = "Deleted..";
            return RedirectToAction("Index", "Home");

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
}