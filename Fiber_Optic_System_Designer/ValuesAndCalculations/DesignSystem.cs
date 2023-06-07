using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;
namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class DesignSystem
    {
        SystemData systemData;

        public DesignSystem(List<Tuple<values_name, dynamic>> SystemRequirements, SystemData systemData)
        {
            this.systemData = systemData;
            initializeSystemRequirements(SystemRequirements);
            Calculations calc = new Calculations(systemData);
            // will call all other funciton :)
            calc.GetActualSNR();
        }
        void initializeSystemRequirements(List<Tuple<values_name, dynamic>> SystemRequirements)
        {
            foreach (var input in SystemRequirements)
            {
                systemData.SetValue(input.Item1, input.Item2);
            }
        }
    }
}
