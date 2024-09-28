using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Models.Repository;

namespace WebBanDT.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context) 
		{ 
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			CategoryModel category= _dataContext.Categories.Where(p=>p.Slug == Slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);

			return View(await productsByCategory.OrderByDescending(p=>p.Id).ToListAsync());
		}
	}
}
