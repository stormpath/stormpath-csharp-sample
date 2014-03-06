using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the Stormpath directory resource.
     */
    class Directory : Resource
    {
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    }
}
