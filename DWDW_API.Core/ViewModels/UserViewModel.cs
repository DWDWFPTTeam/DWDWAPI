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
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public int? RoleId { get; set; }
        public string DeviceToken { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UserAssignViewModel : BaseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public int? RoleId { get; set; }
        public string DeviceToken { get; set; }
        public bool? IsActive { get; set; }
        public string LocationCode { get; set; }
    }

    public class UserCreateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.INVALID_USERNAME_PASSWORD)]
        public string UserName { get; set; }
        [Required(ErrorMessage = ErrorMessages.INVALID_USERNAME_PASSWORD)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = ErrorMessages.FULLNAME_IS_EMPTY)]
        public string FullName { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        public string Phone { get; set; }
        [Required(ErrorMessage = ErrorMessages.BIRTHDAY_IS_EMPTY)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = ErrorMessages.GENDER_IS_EMPTY)]
        [Range(1, 3, ErrorMessage = ErrorMessages.WRONG_GENDER_FORMAT)]
        public int? Gender { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROLE_ID_INVALID)]
        [Range(1, 3, ErrorMessage = ErrorMessages.WRONG_ROLE_FORMAT)]
        public int RoleId { get; set; }
    }
    public class UserUpdateModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.USERID_IS_EMPTY)]
        public int UserId { get; set; }
   
        [Required(ErrorMessage = ErrorMessages.FULLNAME_IS_EMPTY)]
        public string FullName { get; set; }

        [Required(ErrorMessage = ErrorMessages.PHONE_IS_EMPTY)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        public string Phone { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = ErrorMessages.GENDER_IS_EMPTY)]
        [Range(1, 3, ErrorMessage = ErrorMessages.WRONG_GENDER_FORMAT)]
        public int? Gender { get; set; }
        [Required(ErrorMessage = ErrorMessages.ROLE_ID_INVALID)]
        [Range(1, 3, ErrorMessage = ErrorMessages.WRONG_ROLE_FORMAT)]
        public int? RoleId { get; set; }

    }
    public class UserActiveModel : BaseModel
    {
        [Required(ErrorMessage = ErrorMessages.USERID_INVALID)]
        public int UserId { get; set; }
        [Required(ErrorMessage = ErrorMessages.STATUS_INVALID)]
        public bool? IsActive { get; set; }

    }

    public class UserPersonalUpdateModel : BaseModel
    {

        [Required(ErrorMessage = ErrorMessages.FULLNAME_IS_EMPTY)]
        public string FullName { get; set; }
        [Required(ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessages.WRONG_PHONE_FORMAT)]
        public string Phone { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = ErrorMessages.BIRTHDAY_WRONG_DATETIME_FORMAT)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = ErrorMessages.WRONG_GENDER_FORMAT)]
        [Range(1, 3, ErrorMessage = ErrorMessages.GENDER_IS_EMPTY)]
        public int? Gender { get; set; }
    }

    public class UserLoginInfo
    {
        [Required(ErrorMessage = ErrorMessages.USER_NAME_INVALID)]
        public string UserName { get; set; }
        [Required(ErrorMessage = ErrorMessages.INVALID_USERNAME_PASSWORD)]
        [MinLength(1, ErrorMessage = ErrorMessages.PASSWORD_LEN_NOT_VALID)]
        public string Password { get; set; }
        [Required(ErrorMessage = ErrorMessages.WRONG_DEVICETOKEN_FORMAT)]
        public string DeviceToken { get; set; }
    }

    public class TokenResponseModel
    {
        public string AccessToken { get; set; }
    }
    public class UserGetAllViewModel : BaseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public string DeviceToken { get; set; }
        public bool? IsActive { get; set; }
        public RoleViewModel Role { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<LocationUserViewModel> Locations { get; set; }

    }

}
