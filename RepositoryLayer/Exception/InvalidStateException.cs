namespace LDN.Framework.GenericRepository.Exceptions
{
    [Serializable]
    public class InvalidStateException : System.Exception
    {
        public InvalidStateException() : base() { }

        public InvalidStateException(string message) : base(message) { }

        public InvalidStateException(string message, System.Exception innerException) : base(message, innerException) { }

    }
}
