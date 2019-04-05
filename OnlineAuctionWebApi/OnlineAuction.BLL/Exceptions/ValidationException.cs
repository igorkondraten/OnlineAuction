using System;
using System.Collections.Generic;

namespace OnlineAuction.BLL.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> ErrorsList { get; protected set; }

        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, IEnumerable<string> errors) : base(message)
        {
            ErrorsList = errors;
        }
        public ValidationException(string message, Exception inner) : base(message, inner) { }
        protected ValidationException(
            System.Runtime.Serialization.SerializationInfo si,
            System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
