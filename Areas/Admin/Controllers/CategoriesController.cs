using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace fastFood.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listFromDb = context.Categories.ToList().Select(
                x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Name = x.Name
                }).ToList();

            return View(listFromDb);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CategoryViewModel category = new CategoryViewModel();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var model = new Category
            {
                Title = vm.Title,
                Name = vm.Name
            };

            context.Categories.Add(model);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = context.Categories
                .Where(x => x.Id == id)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Name = x.Name
                }).FirstOrDefault();

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var categoryFromDb = context.Categories.FirstOrDefault(x => x.Id == vm.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.Title = vm.Title;
                categoryFromDb.Name = vm.Name;

                context.Categories.Update(categoryFromDb);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Ambil kategori beserta sub-kategori-nya
            var category = context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefault(x => x.Id == id);

            if (category != null)
            {
                // Hapus semua sub-kategori dulu
                if (category.SubCategories != null && category.SubCategories.Any())
                {
                    context.SubCategories.RemoveRange(category.SubCategories);
                }

                // Baru hapus kategori
                context.Categories.Remove(category);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
