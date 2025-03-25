using System.ComponentModel.DataAnnotations;

namespace Company.Kirollos.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "UserName is Required !!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is Required !!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required !!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d\W]).{8,}$")]
        public string Password { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d\W]).{8,}$")]
        [Required(ErrorMessage = "Confirm Password is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password) , ErrorMessage = "Confirm Doesn't match the password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "IsAgree is Required !!")]
        public bool IsAgree { get; set; }
    }
}
