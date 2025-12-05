using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminStaticsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminStaticsController(ApplicationDbContext db)
        {
            _db = db;
        }

      
        public IActionResult Index()
        {
          
            ViewBag.TotalRevenue = _db.Orders
                .Where(x => x.Status == "Completed")
                .Sum(x => (decimal?)x.TotalAmount) ?? 0;

           
            ViewBag.TotalOrders = _db.Orders.Count();

         
            ViewBag.TotalUsers = _db.Users.Count();

            return View();
        }



        //public IActionResult OrdersByStatus()
        //{
        //    var data = _db.Orders
        //        .GroupBy(x => x.Status)
        //        .Select(g => new
        //        {
        //            status = g.Key,
        //            count = g.Count()
        //        })
        //        .ToList();

        //    return Json(data);
        //}


        public IActionResult BestSeller()
        {
            var data = _db.OrderItems
                .GroupBy(i => new { i.ProductId, i.Product.Name, i.Product.ImageUrl })
                .Select(g => new
                {
                    productId = g.Key.ProductId,
                    productName = g.Key.Name,
                    image = g.Key.ImageUrl,
                    totalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.totalSold)
                .Take(5)
                .ToList();

            return Json(data);
        }

        
        public IActionResult TopRated()
        {
            var data = _db.Products
                .OrderByDescending(x => x.Rating)
                .Take(5)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Rating
                })
                .ToList();

            return Json(data);
        }
        // doanh thu theo ngày
        public IActionResult RevenueDaily(DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date.AddDays(1).AddSeconds(-1);

           
            var raw = _db.Orders
                .Where(x => x.Status == "Completed"
                         && x.CreatedAt >= s
                         && x.CreatedAt <= e)
                .Select(x => new
                {
                    x.CreatedAt,
                    x.TotalAmount
                })
                .AsEnumerable(); // ep du lieu ve ram de groupby

         
            var data = raw
                .GroupBy(x => x.CreatedAt.ToString("yyyy-MM-dd"))
                .Select(g => new
                {
                    date = g.Key,
                    total = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.date)
                .ToList();

            return Json(data);
        }

        public IActionResult Summary(DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date.AddDays(1).AddSeconds(-1);

            var orders = _db.Orders
                .Where(x => x.CreatedAt >= s && x.CreatedAt <= e);

            var users = _db.Users
                .Where(x => x.CreatedAt >= s && x.CreatedAt <= e);

            var orderItems = _db.OrderItems
                .Where(x => x.Order.CreatedAt >= s && x.Order.CreatedAt <= e);

            return Json(new
            {
                totalRevenue = orders.Where(x => x.Status == "Completed")
                                     .Sum(x => (decimal?)x.TotalAmount) ?? 0,

                totalOrders = orders.Count(),

                totalUsers = users.Count(),

                ordersByStatus = orders
                    .GroupBy(x => x.Status)
                    .Select(g => new { status = g.Key, count = g.Count() })
                    .ToList()
            });
        }
        public IActionResult OrdersByStatusRange(DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date.AddDays(1).AddSeconds(-1);

            var data = _db.Orders
                .Where(x => x.CreatedAt >= s && x.CreatedAt <= e)
                .GroupBy(x => x.Status)
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count()
                })
                .ToList();

            return Json(data);
        }


    }
}
