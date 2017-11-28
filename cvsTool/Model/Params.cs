using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace cvsTool.Model
{
   public class Params
    {
        /// <summary>
        /// Get required argument from configuration file
        /// </summary>
        /// <param name="args">Configuration file key-value collection</param>
        /// <param name="sArgumentName">Argument name (key) from configuration file</param>
        /// <returns>Argument value</returns>
       public string GetRequiredArgument(NameValueCollection args, string sArgumentName)
       {
            string sArgument = args[sArgumentName];
            if (!string.IsNullOrEmpty(sArgument))
            {
                sArgument = sArgument.Trim();
            }
            if (string.IsNullOrEmpty(sArgument))
            {
                throw new Exception(string.Format("Please provide {0} in configuration file", sArgumentName));
            }
            return sArgument;
       }
    }
}
