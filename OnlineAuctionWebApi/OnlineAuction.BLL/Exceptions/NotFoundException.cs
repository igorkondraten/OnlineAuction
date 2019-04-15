using System;

namespace OnlineAuction.BLL.Exceptions
{
    /// <summary>
    /// The exception is thrown when requested entity doesn't exist in database.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string str) : base(str) { }
        public NotFoundException(string str, Exception inner) : base(str, inner) { }
        protected NotFoundException(
            System.Runtime.Serialization.SerializationInfo si,
            System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
