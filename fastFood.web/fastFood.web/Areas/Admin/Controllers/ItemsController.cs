using FastFood.Models;
using FastFood.Repository;
using fastFood.web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fastFood.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Items
        public async Task<IActionResult> Index()
        {
            var items = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.SubCategory)
                .ToListAsync();
            return View(items);
        }

        // GET: Admin/Items/Create
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            return View();
        }

        // POST: Admin/Items/Create
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? imageFileName = null;

                if (viewModel.ImageUrl != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    imageFileName = Guid.NewGuid().ToString() + "_" + viewModel.ImageUrl.FileName;
                    var filePath = Path.Combine(uploadsFolder, imageFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageUrl.CopyToAsync(fileStream);
                    }
                }

                var item = new Item
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    CategoryId = viewModel.CategoryId,
                    SubCategoryId = viewModel.SubCategoryId,
                    ImageUrl = "/images/" + imageFileName
                };

                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            return View(viewModel);
        }

        // GET: Admin/Items/GetSubCategory?categoryId=1
        [HttpGet]
        public IActionResult GetSubCategory(int categoryId)
        {
            var subCategories = _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .Select(sc => new { id = sc.Id, title = sc.Title })
                .ToList();

            return Json(subCategories);
        }
    }
}
