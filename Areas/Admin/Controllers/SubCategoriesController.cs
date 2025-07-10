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
            ViewBag.category = new SelectList(context.Categories.ToList(), "Id", "Title");
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
            var subCategory = context.SubCategories.FirstOrDefault(x => x.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            var vm = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                CategoryId = subCategory.CategoryId
            };

            ViewBag.category = new SelectList(context.Categories, "Id", "Title", subCategory.CategoryId);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var subCategory = context.SubCategories.FirstOrDefault(x => x.Id == vm.Id);

                if (subCategory == null)
                {
                    return NotFound();
                }

                subCategory.Title = vm.Title;
                subCategory.CategoryId = vm.CategoryId;

                context.SubCategories.Update(subCategory);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.category = new SelectList(context.Categories, "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        // GET: Tampilkan halaman konfirmasi delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var subCategory = context.SubCategories
                                     .Include(x => x.Category)
                                     .FirstOrDefault(x => x.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: Hapus subcategory dan item yang terkait
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var subCategory = context.SubCategories.FirstOrDefault(x => x.Id == id);

            if (subCategory != null)
            {
                // Hapus semua Item yang terkait
                var relatedItems = context.Items.Where(i => i.SubCategoryId == id).ToList();
                if (relatedItems.Any())
                {
                    context.Items.RemoveRange(relatedItems);
                }

                // Hapus SubCategory
                context.SubCategories.Remove(subCategory);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
