using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ObjectNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DoubleFoundException : Exception
    {
        public DoubleFoundException() { }
        public DoubleFoundException(string message) : base(message) { }
        public DoubleFoundException(string message, Exception inner) : base(message, inner) { }
        protected DoubleFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class NullPropertyException : Exception
    {
        public NullPropertyException() { }
        public NullPropertyException(string message) : base(message) { }
        public NullPropertyException(string message, Exception inner) : base(message, inner) { }
        protected NullPropertyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FormatIsIncorrectException : Exception
    {
        public FormatIsIncorrectException() { }
        public FormatIsIncorrectException(string message) : base(message) { }
        public FormatIsIncorrectException(string message, Exception inner) : base(message, inner) { }
        protected FormatIsIncorrectException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ObjectStockOverflowException : Exception
    {
        public ObjectStockOverflowException() { }
        public ObjectStockOverflowException(string message) : base(message) { }
        public ObjectStockOverflowException(string message, Exception inner) : base(message, inner) { }
        protected ObjectStockOverflowException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CouldNotDeleteObjectException : Exception
    {
        public CouldNotDeleteObjectException() { }
        public CouldNotDeleteObjectException(string message) : base(message) { }
        public CouldNotDeleteObjectException(string message, Exception inner) : base(message, inner) { }
        protected CouldNotDeleteObjectException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DatesNotChronologicalException : Exception
    {
        public DatesNotChronologicalException() { }
        public DatesNotChronologicalException(string message) : base(message) { }
        public DatesNotChronologicalException(string message, Exception inner) : base(message, inner) { }
        protected DatesNotChronologicalException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
