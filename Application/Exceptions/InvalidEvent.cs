using System;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public class InvalidEvent : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidEvent()
        { }

        public InvalidEvent(string message) : base(message)
        { }

        public InvalidEvent(string message, Exception inner) : base(message, inner)
        { }

        protected InvalidEvent(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}