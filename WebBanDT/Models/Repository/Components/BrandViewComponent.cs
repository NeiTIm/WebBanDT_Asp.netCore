using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanDT.Models.Repository.Components
{
	public class BrandViewComponent:ViewComponent
	{
		private readonly DataContext _dataContext;
		public BrandViewComponent(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Brands.ToListAsync());
	}
}
