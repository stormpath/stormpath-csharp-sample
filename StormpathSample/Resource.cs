using System;

namespace StormpathSample
{
    /**
     * This class is the base for all Stormpath resource wrappers.
     */
    abstract class Resource
    {
        public string href { get; set; }
    }
}
