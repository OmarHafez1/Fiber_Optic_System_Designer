namespace Fiber_Optic_System_Designer.MyExceptions
{
    internal class CantFindSuitableComponentsException : Exception
    {
        public CantFindSuitableComponentsException() { }
        public CantFindSuitableComponentsException(string message) : base(message) { }

        public CantFindSuitableComponentsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
