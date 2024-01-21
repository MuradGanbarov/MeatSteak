using MeatSteak.DAL;
using MeatSteak.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MeatSteak.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new()
            {
                Employees = await _context.Employees.Include(e=>e.Positions).ToListAsync(),
            };
            return View(vm);
        }

    }
}