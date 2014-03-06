using System;
using System.Collections.Generic;

namespace StormpathSample
{
    /**
     * This class is the base for all collection resources.
     */
    abstract class CollectionResource<T> : Resource
    {
        public List<T> items { get; set; }
    }
}
