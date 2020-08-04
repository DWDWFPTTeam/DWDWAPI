using System;
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
        public const string START_DATE_REQUIRED = "Start date is required"; 
        public const string END_DATE_REQUIRED = "End date is required";
        public const string INVALID_DATE_FORMAT = "Invalid date format! Date format is'yyyy-MM-dd'";

        public const string INVALID_USERID_OR_LOCATIONID = "User Id or Location Id is invalid";

        //Role Message
        public const string ROLE_IS_NOT_EXISTED = "Role is not existed";
        public const string ROLE_IS_EXISTED = "Role is existed";
        public const string ROLE_IS_NOT_MANAGER = "Role is not manager";
        public const string INVALID_ROLE_FOR_THIS_ACTION = "Invalid Role";

        //Device Message
        public const string DEVICE_LIST_EMPTY = "This device is not existed";
        public const string DEVICE_IS_EXISTED = "Device is existed";
        public const string DEVICE_IS_NOT_EXISTED = "Device is not existed";

        //Room Message
        public const string ROOM_IS_NOT_EXISTED = "Room is not existed";
        public const string ROOM_USER_NOT_EXISTED = "This room doesn't belong to the current manager";
        public const string ROOM_IS_EXISTED = "Room is existed";
        public static string ROOM_NOT_FOUND = "Room is not found";

        //RoomDevice Message
        public const string WRONG_DATETIME_FORMAT = "Invalid date";

        //Shift Message
        public const string INVALID_MANAGER = "This shift doesn't belong to the current manager";
        public const string SHIFT_IS_NOT_EXISTED = "Shift is not existed";

        //User Message
        public const string BIRTHDAY_WRONG_DATETIME_FORMAT = "Role is not existed";
        public const string PASSWORD_LEN_NOT_VALID = "Password must have more than 1 character";
        public const string INVALID_USERNAME_PASSWORD = "Invalid username password!";
        public const string USER_IS_EXISTED = "User is existed";
        public const string USER_IS_NOT_EXISTED = "User is not existed";
        public const string USERNAME_IS_EXISTED = "Username is existed";
        public const string USERID_IS_EXISTED = "UserId is existed";
        public const string USERID_IS_NOT_EXISTED = "UserID is not existed";
        public const string WRONG_PHONE_FORMAT = "Wrong phone number format";
        public const string GENDER_IS_NOT_EXISTED = "Gender is not existed";
        public static string MANAGER_NOT_FOUND = "Cannot found the manager";


        //Location Message
        public const string LOCATION_IS_NOT_EXISTED = "Location is not existed";
        public const string LOCATION_IS_EXISTED = "Location is existed";
        public const string LOCATION_IS_NOT_BELONG_TO_MANAGER = "Location Is not belong to manager";
        public const string LOCATION_IS_NOT_BELONG_TO_WORKER = "Location Is not belong to worker";
        public const string LOCATION_DEVICE_EMPTY = "There is no device in this location";

        public const string LOCATION_USER_NOT_EXISTED = "This location does not belong to the current manager";
        public const string DEVICE_IS_NOT_BELONG_ROOM = "This device is not belong this this room";

        //Notification Message
        public const string NOTI_ID_IS_NOT_EXISTED = "NotificationId is not exitsted";
    }
}
