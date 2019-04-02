using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltSourceApp.Helpers
{
    public class ErrorHelper
    {
        public string ErrorMessage {get; set;}
        public ErrorHelper(string errorMessage) 
        {
            this.ErrorMessage = errorMessage;
        }

        public JObject ToJObject()
        {
            return JObject.FromObject(new { error = ErrorMessage });
        }

        public string ToJSON()
        {
            return JObject.FromObject(new { error = ErrorMessage }).ToString();
        }
    }
}
