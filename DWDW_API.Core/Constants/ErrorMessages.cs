﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.Constants
{
    public class ErrorMessages
    {
        public const string GET_LIST_FAIL = "Failed to get list";
        public const string SEARCH_FAIL = "Failed to search";
        public const string CREATE_FAIL = "Failed to create";
        public const string UPDATE_FAIL = "Failed to update";
        public const string DELETE_FAIL = "Failed to delete";

        public const string UPDATE_ERROR = "Update failed";
        public const string INSERT_ERROR = "Insert failed";
        public const string DEACTIVE_ERROR = "Deactive failed";

        public const string EMPTY_LIST = "Your list is empty";

        //Role Message
        public const string ROLE_IS_NOT_EXISTED = "Role is not existed";



        //User Message
        public const string BIRTHDAY_WRONG_DATETIME_FORMAT = "Role is not existed";
        public const string PASSWORD_LEN_NOT_VALID = "Password must have more than 1 character";
        public const string INVALID_USERNAME_PASSWORD = "Invalid username password!";
        public const string USER_IS_EXISTED = "User is existed";
        public const string USER_IS_NOT_EXISTED = "User is not existed";
        public const string USERNAME_IS_EXISTED = "Username is existed";
        public const string USERID_IS_EXISTED = "UserId is existed";
        public const string USERID_IS_NOT_EXISTED = "UserID is not existed";
    }
}
