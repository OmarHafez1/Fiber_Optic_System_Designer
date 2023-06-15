namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class Data
    {
        private string name;
        private dynamic value;
        private string unit;
        public Data(string name = null, string unit = null, dynamic value = null)
        {
            this.name = name;
            this.value = value;
            this.unit = unit;
        }

        public string getName() { return name; }
        public dynamic getValue() { return value; }
        public dynamic getUnit() { return unit; }

        public void setName(string name) => this.name = name;
        public void setValue(dynamic value) => this.value = value;
        public void setUnit(string unit) => this.unit = unit;

        public override string ToString()
        {
            return name + ": " + (value != null ? value.ToString() : "");
        }
    }
}
