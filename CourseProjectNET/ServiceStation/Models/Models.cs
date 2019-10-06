using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceStation.Models
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина строки должна быть от 4 до 50 символов")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string PasswordHash { get; set; }
    }

    public class RegisterModel
    {
        [Key]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string PasswordHash { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина строки должна быть от 4 до 50 символов")]
        [Display(Name = "ФИО")]
        public string UserName { get; set; }

        [RegularExpression(@"^(\+380)[0-9]{9}", ErrorMessage = "Введите номер  телефона в формате +380ХХХХХХХХХ, где Х - цифры")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public static string CreateHash(string pass)
        {
            return pass.GetHashCode().ToString();
        }
    }
}