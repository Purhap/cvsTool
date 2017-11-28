using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using cvsTool.Model;

namespace cvsTool.Model
{
    public class LoginParams : Params
    {
        public string Login
        {
            get
            {
                return mLogin;
            }
        }
        private string mLogin;

        public string Password
        {
            get
            {
                return mPassword;
            }
        }
        private string mPassword;

        public string URL
        {
            get
            {
                return mURL;
            }
        }
        private string mURL;

        public string Connection
        {
            get
            {
                return mConnection;
            }
        }
        private string mConnection;

        public string SessionID
        {
            get
            {
                return mSessionID;
            }
        }
        private string mSessionID;

        public string Pin
        {
            get
            {
                return mPin;
            }
        }
        private string mPin;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="args"></param>
        public LoginParams(NameValueCollection args)
        {
            mLogin = GetRequiredArgument(args, "Login");
            mPassword = GetRequiredArgument(args, "Password");
            mURL = GetRequiredArgument(args, "URL");
            if (!string.IsNullOrEmpty(mURL))
            {
                if (!mURL.EndsWith("Hosts.jsp", StringComparison.OrdinalIgnoreCase))
                {
                    mURL += "/Hosts.jsp";
                }
            }
            mConnection = GetRequiredArgument(args, "Connection");
            mSessionID = args["SessionID"];
            mPin = args["Pin"];
        }

        
    }
}
