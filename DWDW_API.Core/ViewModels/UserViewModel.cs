using DWDW_API.Core.Constants;
using DWDW_API.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class UserViewModel : BaseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UserCreateModel : BaseModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        public string Phone { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public int? Gender { get; set; }
    }
    public class UserUpdateModel : BaseModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        public string Phone { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = ErrorMessages.GENDER_IS_NOT_EXISTED)]
        public int? Gender { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = ErrorMessages.ROLE_IS_NOT_EXISTED)]
        public int? RoleId { get; set; }

    }

    public class UserLoginInfo
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = ErrorMessages.PASSWORD_LEN_NOT_VALID)]
        public string Password { get; set; }
    }


}
