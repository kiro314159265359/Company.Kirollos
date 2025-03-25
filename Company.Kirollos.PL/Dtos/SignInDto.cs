using System.ComponentModel.DataAnnotations;

namespace Company.Kirollos.PL.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d\W]).{8,}$")]
        public string Password { get; set; }

        public bool RemmemberMe { get; set; }
    }
}
