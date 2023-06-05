using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;
namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class DesignSystem
    {
        SystemData systemData;

        public DesignSystem(List<Tuple<values_name, String>> SystemRequirements)
        {
            systemData = new SystemData();
            initializeSystemRequirements(SystemRequirements);
        }
        void initializeSystemRequirements(List<Tuple<values_name, String>> SystemRequirements)
        {
            foreach (var input in SystemRequirements)
            {
                systemData.SetValue(input.Item1, input.Item2);
            }
        }
    }
}
