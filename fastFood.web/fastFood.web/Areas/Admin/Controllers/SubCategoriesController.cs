using FastFood.Models;
using FastFood.Repository;
using fastFood.web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fastFood.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var subCategories = context.SubCategories.Include(x => x.Category).ToList();
            return View(subCategories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = context.Categories.ToList(); // ambil data kategori

            if (categories == null || !categories.Any())
            {
                ViewBag.category = new List<SelectListItem>(); // biar gak null
            }
            else
            {
                ViewBag.category = new SelectList(categories, "Id", "Title");
            }

            return View(new SubCategoryViewModel());
        }



        [HttpPost]
        public IActionResult Create(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new SubCategory
                {
                    Title = vm.Title,
                    CategoryId = vm.CategoryId
                };

                context.SubCategories.Add(model);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.category = new SelectList(context.Categories, "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            SubCategoryViewModel vm = new SubCategoryViewModel();
            var subCategory = context.SubCategories
                                     .Where(x => x.Id == id)
                                     .FirstOrDefault();

            if (subCategory != null)
            {
                vm.Id = subCategory.Id;
                vm.Title = subCategory.Title;
                vm.CategoryId = subCategory.CategoryId;

                ViewBag.category = new SelectList(context.Categories, "Id", "Title", subCategory.CategoryId);
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new SubCategory
                {
                    Title = vm.Title,
                    CategoryId = vm.CategoryId
                };

                context.SubCategories.Add(model);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.category = new SelectList(context.Categories, "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subCategory = context.SubCategories
                                     .FirstOrDefault(x => x.Id == id);

            if (subCategory != null)
            {
                context.SubCategories.Remove(subCategory);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
