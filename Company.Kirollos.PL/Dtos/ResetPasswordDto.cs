using System.ComponentModel.DataAnnotations;

namespace Company.Kirollos.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d\W]).{8,}$")]
        public string NewPassword { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d\W]).{8,}$")]
        [Required(ErrorMessage = "Confirm Password is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Doesn't match the password")]
        public string ConfirmPassword { get; set; }
    }
}
