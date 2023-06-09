using Fiber_Optic_System_Designer.MyExceptions;
using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;
namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class DesignSystem
    {
        SystemData systemData;
        Calculations calc;

        public DesignSystem(List<Tuple<values_name, dynamic>> SystemRequirements, SystemData systemData)
        {
            this.systemData = systemData;
            initializeSystemRequirements(SystemRequirements);
            calc = new Calculations(systemData);
            // will call all other funciton :)

            calc.GetActualSNR();
            calc.GetReceiverSensitivity();
            calc.GetActualPowerAtReceiver();
            calc.GetActualBitRate();
            calc.GetActualSystemRiseTime();
            calc.GetRequiredSystemRiseTime();

            if (!IsValidSystem())
            {
                throw new CantDesignTheSystemException("Due to some errors encountered, we were unable to configure your system. Please try again or contact our support team for assistance in resolving this issue.");
            }
        }

        private bool IsValidSystem()
        {
            if (calc.GetActualPowerAtReceiver() < calc.GetReceiverSensitivity()) return false;
            if (calc.GetActualBitRate() < systemData.GetValue(values_name.REQUIRED_BIT_RATE)) return false;
            if (calc.GetActualSystemRiseTime() > calc.GetRequiredSystemRiseTime()) return false;
            if (SNR_BER_Conversion.ConverSNRTOBER(calc.GetActualSNR()) > systemData.GetValue(values_name.REQUIRED_BER)) return false;
            return true;
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
