using System;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public class InvalidCollaboration : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidCollaboration()
        { }

        public InvalidCollaboration(string message) : base(message)
        { }

        public InvalidCollaboration(string message, Exception inner) : base(message, inner)
        { }

        protected InvalidCollaboration(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}