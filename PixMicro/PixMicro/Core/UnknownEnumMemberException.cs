using System;

namespace PixMicro.Core
{
    public class UnknownEnumMemberException : Exception
    {
        public UnknownEnumMemberException(Enum unrecognisedMember)
            : base($"Unhandled enum member: {unrecognisedMember}")
        {
        }
    }
}
