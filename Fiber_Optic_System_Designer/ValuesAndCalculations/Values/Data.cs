namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class Data
    {
        private String name, value;
        public Data(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public String getName() { return name; }
        public String getValue() { return value; }

        public void setName(String name) { this.name = name; }
        public void setValue(String value) { this.value = value; }

        public override String ToString()
        {
            return name + ": " + value;
        }
    }
}
