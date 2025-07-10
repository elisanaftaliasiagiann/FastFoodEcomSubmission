using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace fastFood.web.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        public IFormFile? ImageUrl { get; set; }  // Untuk upload file gambar
    }
}
