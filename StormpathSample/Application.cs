using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the Stormpath application resource.
     */
    class Application : Resource
    {
        public string name { get; set; }

        public string description { get; set; }

        public string status { get; set; }
    }
}
