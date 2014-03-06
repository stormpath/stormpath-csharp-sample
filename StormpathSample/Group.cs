using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the Stormpath group resource.
     */
    class Group : Resource
    {
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public Directory directory { get; set; }
    }
}
