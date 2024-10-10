using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Models.Repository;

namespace WebBanDT.Areas.Admin.Controllers
{
  
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {

            if (ModelState.IsValid)//neu tinh trang cua cac model chuan 
            {
                //them du lieu
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Đã có trong database");
                    return View(brand);
                }


                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có 1 vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMassage = string.Join("\n", errors);
                return BadRequest(errorMassage);
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            if (ModelState.IsValid)//neu tinh trang cua cac model chuan 
            {
                //them du lieu
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(brand);
                }


                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = " Cập nhật thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có 1 vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMassage = string.Join("\n", errors);
                return BadRequest(errorMassage);
            }
            return View(brand);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thương hiêju đã xóa";
            return RedirectToAction("Index");
        }
    }
}
