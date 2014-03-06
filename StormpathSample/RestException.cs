using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormpathSample
{
    /**
     * This class wraps an error returned from the Stormpath REST API.
     */
    class RestError : Resource 
    {
        public int code { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public string developerMessage { get; set; }
        public string moreInfo { get; set; }
    }

    /**
     * This exception wraps an error from the Stormpath REST API so we can throw
     * it when an error is received.
     */
    class RestException : Exception
    {
        public RestException(RestError restError)
            // We set the exception "message" with the value of the "developerMessage"
            // from the Stormpath REST API because this field has more information about the error.
            : base(restError.developerMessage)
        {
            code = restError.code;
            status = restError.status;
            // The "message" returned from the Stormpath REST API has little information and is meant to be
            // the message that is shown to a final user, so we call this field "userMessage" is this exception
            // class.
            userMessage = restError.message;
            moreInfo = restError.moreInfo;
        }

        public int code { get; set; }
        public int status { get; set; }
        public string userMessage { get; set; }
        public string moreInfo { get; set; }
    }
}
