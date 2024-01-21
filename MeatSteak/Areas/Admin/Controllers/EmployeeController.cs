using MeatSteak.Areas.Admin.Models.Utilities.Enums;
using MeatSteak.Areas.Admin.Models.Utilities.Extentions;
using MeatSteak.Areas.Admin.ViewModels;
using MeatSteak.DAL;
using MeatSteak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeatSteak.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Employees.CountAsync();
            List<Employee> employees = await _context.Employees.Include(e => e.Positions).Skip(page * 3).Take(3).ToListAsync();

            PaginationVM<Employee> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = employees
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            EmployeeCreateVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync(),

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            bool check = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if (!check)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "This position doesn't exists");
                return View(vm);
            }
            if (!vm.Photo.IsValidType(FileType.Image))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Photo should be image type");
                return View(vm);
            }

            if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Photo can be less or equal 5mb");
                return View(vm);
            }

            Employee employee = new()
            {
                Name = vm.Name,
                Dribbble = vm.Dribbble,
                Facebook = vm.Facebook,
                Twitter = vm.Twitter,
                PositionId = vm.PositionId,
                ImageURL = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "images")
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee existed = await _context.Employees.Include(e=>e.Positions).FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            EmployeeUpdateVM vm = new()
            {
                Name = existed.Name,
                Facebook = existed.Facebook,
                Dribbble = existed.Dribbble,
                Twitter = existed.Twitter,
                PositionId = existed.PositionId,
                ImageURL = existed.ImageURL,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,EmployeeUpdateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Employee existed = await _context.Employees.Include(e=>e.Positions).FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            bool check = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if (!check)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "This position doesn't exists");
                return View(vm);
            }
            if (vm.Photo is not null)
            {
                if (!vm.Photo.IsValidType(FileType.Image))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Photo should be image type");
                    return View(vm);
                }

                if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Photo can be less or equal 5mb");
                    return View(vm);
                }
                existed.ImageURL.Delete(_env.WebRootPath, "assets", "images");
                existed.ImageURL = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "images");
            }

            existed.Name = vm.Name;
            existed.Dribbble = vm.Dribbble;
            existed.Facebook = vm.Facebook;
            existed.Twitter = vm.Twitter;
            existed.PositionId = vm.PositionId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee existed = await _context.Employees.Include(e => e.Positions).FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            existed.ImageURL.Delete(_env.WebRootPath, "assets", "images");
            _context.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


    }
}
