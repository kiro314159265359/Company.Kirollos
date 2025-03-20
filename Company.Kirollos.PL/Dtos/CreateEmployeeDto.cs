using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Kirollos.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }
        [Range(22,60 , ErrorMessage ="Age is must be between 20 , 60")]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress , ErrorMessage = "Invalid syntax")]
        public string Email { get; set; }
        [RegularExpression("[0-9]{1,5}( [a-zA-Z.]*){1,4},?( [a-zA-Z]*){1,3},? [a-zA-Z]{2},? [0-9]{5}" , ErrorMessage = "Invalid Syntax")]
        public string Adress { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Hire Date")]
        public DateTime HiringDate { get; set; }
        public DateTime CreateAt { get; set; }
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
    }
}
