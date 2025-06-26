using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    Title = x.Title
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
            Category model = new Category();
            model.Title = vm.Title;
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
                    Title = x.Title
                }).FirstOrDefault();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var categoryFromDb = context.Categories.FirstOrDefault(x => x.Id == vm.Id);
                if (categoryFromDb != null)
                {
                    categoryFromDb.Title = vm.Title;
                    context.Categories.Update(categoryFromDb);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(vm); // Tambahkan ini supaya tidak error CS0161
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
