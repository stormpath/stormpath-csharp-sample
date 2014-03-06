using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the authentication result after a login attempt.
     */
    class AuthenticationResult : Resource
    {
        public Account account { get; set; }
    }
}
