using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
namespace WebBanDT.Models.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel{ Name="Macbook",Slug="macbook",Description="Apple in Large Brand in the world",Status=1};
				CategoryModel pc = new CategoryModel { Name = "PC", Slug = "pc", Description = "Samsung in Large Brand in the world", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple in Large Brand in the world", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "sumsung", Description = "Samsung in Large Brand in the world", Status = 1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Best", Image = "1.jpg", Category = macbook, Brand =apple, Price = 12.00M },
					new ProductModel { Name = "PC", Slug = "pc", Description = "PC is Best", Image = "2.jpg", Category = pc, Brand = samsung, Price = 13.00M }
				);
				_context.SaveChanges();
			}
		}
	}
}
