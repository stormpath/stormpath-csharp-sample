using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the Stormpath account resource.
     */
    class Account : Resource
    {
        public string givenName { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string username { get; set; }

        // The reason why the password has a getter is only for the purposes of this sample application
        // because the REST API never returns the account passwords.
        public string password { get; set; }
        public string fullName { get; set; }
        public string middleName { get; set; }
        public string status { get; set; }
        public Directory directory { get; set; }
    }
}
