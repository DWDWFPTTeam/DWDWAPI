﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.Constants
{
    public class Constant
    {
        //configuaration 
        public const string B_CONNECTION_STRING = "Server=.;Database=DWDB;Trusted_Connection=True;User Id=sa;Password=1";
        public const string JWT_DESCRIPTION = @"JWT Authorization header using the Bearer scheme.  
                            Enter 'Bearer' [space] and then your token in the text input below.
                            Example: 'Bearer 12345abcdef'";
        public const string AUTHORIZATION = "Authorization";
        public const string BEARER = "Bearer";
        public const string OAUTH2 = "oauth2";
        public const string PROJECTNAME = "DWDW API";

        //roles
        public const string ADMIN = "1";
        public const string MANAGER = "2";
        public const string WORKER = "3";

        public const string DEVICE_HOANG_MOBILETOKEN = "cAF8JeveS9av5pIdQtge0-:APA91bGvzkAno7ycM_fIzqwEjhIUTBy-la9u71_" +
               "vYocHFhnnuGIO0PyfAMU2ph0cae6YuRGpYTAnbw9KtcKgN-aENmED3Bz4KLHnjrpU9HgfRHhBcTBP_" +
               "gbd41-tcsMD4kC9Vl0dnHC2";
    }
}
