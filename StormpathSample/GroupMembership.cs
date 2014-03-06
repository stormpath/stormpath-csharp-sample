using System;

namespace StormpathSample
{
    /**
     * This class is a wrapper for the Stormpath group membership resource.
     */
    class GroupMembership : Resource
    {
        public Group group { get; set; }
        public Account account { get; set; }
    }
}
