namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class Data
    {
        private string name;
        private dynamic value;

        public Data(string name, dynamic value = null)
        {
            this.name = name;
            this.value = value;
        }

        public string getName() { return name; }
        public dynamic getValue() { return value; }

        public void setName(string name) => this.name = name;
        public void setValue(dynamic value) => this.value = value;

        public override string ToString()
        {
            return name + ": " + (value != null ? value.ToString() : "");
        }
    }
}
