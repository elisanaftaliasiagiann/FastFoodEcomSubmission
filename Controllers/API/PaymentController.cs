using FastFood.Models.ViewModels;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace FastFood.web.Controllers.API
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetSnapToken([FromBody] MidtransTokenRequest req)
        {
            var order = await _context.OrderHeaders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(o => o.TransId == req.OrderId);

            if (order == null)
                return NotFound("Order tidak ditemukan.");

            var midtransServerKey = _configuration["Midtrans:ServerKey"];
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{midtransServerKey}:"));

            HttpClient httpClient = new HttpClient(); 
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var snapRequest = new
            {
                transaction_details = new
                {
                    order_id = order.TransId,
                    gross_amount = (int)order.OrderTotal
                },
                customer_details = new
                {
                    first_name = order.Name,
                    email = order.Email,
                    phone = order.PhoneNumber
                },
                item_details = order.OrderDetails.Select(d => new
                {
                    id = d.ItemId.ToString(),
                    name = d.Item.Title,
                    price = (int)d.Price,
                    quantity = d.Count
                }).ToList()
            };

            var json = JsonSerializer.Serialize(snapRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://app.sandbox.midtrans.com/snap/v1/transactions", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result); // tampilkan detail error Midtrans

            var parsed = JsonDocument.Parse(result);
            var token = parsed.RootElement.GetProperty("token").GetString();

            return Ok(new { token });
        }
    }
}
