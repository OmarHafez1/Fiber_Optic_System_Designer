namespace Fiber_Optic_System_Designer.MyExceptions
{
    internal class CantDesignTheSystemException : Exception
    {
        public CantDesignTheSystemException() { }
        public CantDesignTheSystemException(string message) : base(message) { }

        public CantDesignTheSystemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
