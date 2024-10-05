using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanDT.Models.Repository.Validation;

namespace WebBanDT.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		
		[Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
		public string Slug { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        public double Price { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 thương hiệu ")]
        public int BrandId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 danh mục")]
        public int CategoryId {  get; set; }
        public BrandModel Brand { get; set; }
        public CategoryModel Category { get; set; }
        public string Image { get; set; } 
		[NotMapped]
		[FileExtension]
        [Required(ErrorMessage = "Yêu cầu chọn hình ảnh sản phẩm")]
        public IFormFile? ImageUpload {  get; set; }
	}
}
