using System;

namespace StormpathSample
{
    /**
     * This class serves a the wrapper for an account login attempt.
     */
    class BasicLoginAttempt : Resource
    {
        public BasicLoginAttempt()
        {
            this.type = "basic";
        }
        public string type { get; set; }
        public string value { get; set; }
    }
}
