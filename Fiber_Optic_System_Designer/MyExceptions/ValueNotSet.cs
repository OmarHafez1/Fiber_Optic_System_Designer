namespace Fiber_Optic_System_Designer.MyExceptions
{
    internal class ValueNotSet : Exception
    {
        public ValueNotSet() { }
        public ValueNotSet(string message) : base(message) { }

        public ValueNotSet(string message, Exception innerException) : base(message, innerException) { }
    }
}
