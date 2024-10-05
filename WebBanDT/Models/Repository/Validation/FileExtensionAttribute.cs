using System.ComponentModel.DataAnnotations;

namespace WebBanDT.Models.Repository.Validation
{
    public class FileExtensionAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);//vi du la 12345.jpg
                string[] extensions = { "jpg", "png","jpeg" };//chuỗi hình ảnh có kí tự đuôi là
                bool result=extension.Any(x=>extension.EndsWith(x));
                if(!result)
                {
                    return new ValidationResult("Cho phep extensions la duoi  jpg png jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
