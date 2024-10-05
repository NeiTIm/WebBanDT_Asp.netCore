using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebBanDT.Models;
using WebBanDT.Models.Repository;

namespace WebBanDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnviroment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p=>p.Id).Include(p=>p.Category).Include(p=>p.Brand).ToListAsync());
        }
        [HttpGet]
        public  ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories,"Id","Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name",product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name",product.BrandId);
            if (ModelState.IsValid)//neu tinh trang cua cac model chuan 
            {
                //them du lieu
                product.Slug = product.Name.Replace(" ", "-");
                var slug=await _dataContext.Products.FirstOrDefaultAsync(p=>p.Slug==product.Slug);
                if(slug!=null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/products");//luu anh vua update vao wwwroot/meida/products
                                                                                                      //  string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;//chuoi random cai nay co the bo
                    string imageName =product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }


                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có 1 vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMassage = string.Join("\n", errors);
                return BadRequest(errorMassage);
            }
            
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            var existed_product= _dataContext.Products.Find(product.Id);//tim san pham theo id product
            if (ModelState.IsValid)//neu tinh trang cua cac model chuan 
            {
               
                product.Slug = product.Name.Replace(" ", "-");
              
                if (product.ImageUpload != null)
                {

                    string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/products");//luu anh vua update vao wwwroot/meida/products
                                                                                                      //  string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;//chuoi random cai nay co the bo
                    string imageName = product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    //xoa anh cu
                    string oldfileImage = Path.Combine(uploadDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfileImage))
                        {
                            System.IO.File.Delete(oldfileImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleteing the product image");
                    }
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;
                   
                }
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;
                  
                _dataContext.Update(existed_product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
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

        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if(product == null)
            {
                return NotFound();
            }
            string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/products");
            string oldfileImage = Path.Combine(uploadDir, product.Image);
            try
            {
                if (System.IO.File.Exists(oldfileImage))
                {
                  System.IO.File.Delete(oldfileImage);
                }
            }catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleteing the product image");
            }
            //if (!string.Equals(product.Image, "noimage.jpg"))
            //{ 
            //    string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "media/products");
            //    string oldfileImage = Path.Combine(uploadDir, product.Image);
            //    if(System.IO.File.Exists(oldfileImage))
            //    {
            //        System.IO.File.Delete(oldfileImage);
            //    }
            //}
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
    }
}
