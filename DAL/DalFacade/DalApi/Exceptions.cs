using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// Object was not found OR does not exist (CRUD - Drop the "C")
    /// </summary>
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ObjectNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// ID = Object.ID in ADD function (CRUD - Drop the "RUD")
    /// </summary>
    [Serializable]
    public class DoubleFoundException : Exception
    {
        public DoubleFoundException() { }
        public DoubleFoundException(string message) : base(message) { }
        public DoubleFoundException(string message, Exception inner) : base(message, inner) { }
        protected DoubleFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }

    [Serializable]
    public class LoadingException : Exception
    {
        private string? filePath;
        public LoadingException() { }
        public LoadingException(string? message) : base(message) { }
        public LoadingException(string? message, Exception? innerException) : base(message, innerException) { }
        public LoadingException(string path, string message, Exception inner) => filePath = path;
        protected LoadingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class InfoMissException : Exception
    {
        private readonly int id;

        public InfoMissException(int _id) : base()
        {
            id = _id;
        }

        public InfoMissException(int id, string message) : base(message)
        {
            this.id = id;
        }

        public InfoMissException(int _id, string message, Exception innerException) : base(message, innerException)
        {
            id = _id;
        }
    }
}
