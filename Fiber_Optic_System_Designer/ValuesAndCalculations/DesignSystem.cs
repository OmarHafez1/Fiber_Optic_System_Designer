using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;
namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class DesignSystem
    {
        SystemData systemData;

        public DesignSystem(List<Tuple<values_name, String>> SystemRequirements, SystemData systemData)
        {
            this.systemData = systemData;
            initializeSystemRequirements(SystemRequirements);
            Calculations calc = new Calculations(systemData);
            /*            calc.GetEnvironment();
                        calc.GetModulationCode();
                        calc.GetNoiseFactorOfPhotodetector();
                        calc.GetRespositivityOfPhotodetector();
                        calc.GetReceiverSensitivity();
                        calc.ChoseDetector();
                        calc.GetFiberType();
                        calc.ChoseOpticalFiber();
                        calc.GetFiberAttenuation();
                        calc.GetTotalFiberLoss();
                        calc.GetSourceType();
                        calc.ChoseSource();
                        calc.GetAverageSourceOutput();
                        calc.GetSplice();
                        calc.GetNumberOfSplices();
                        calc.GetSplicesInsertionLoss();
                        calc.GetTotalSpliceLoss();
                        calc.ChoseConnector();
                        calc.GetConnectorsInsertionLoss();
                        calc.GetTotalConnectorLoss();
                        calc.GetTimeDegradationFactor();
                        calc.GetEnvDegradationFactor();
                        calc.GetTotalAttenuation();
                        calc.GetTotalLossMargin();
                        calc.GetExcessPower();
                        calc.GetActualPowerAtReceiver();
                        calc.GetRequiredSystemRiseTime();
                        calc.GetFiberBW();
                        calc.GetFiberRiseTime();
                        calc.GetSourceRiseTime();
                        calc.GetDetectorRiseTime();
                        calc.GetActualBitRate();*/
            calc.GetActualSNR();
        }
        void initializeSystemRequirements(List<Tuple<values_name, String>> SystemRequirements)
        {
            foreach (var input in SystemRequirements)
            {
                systemData.SetValue(input.Item1, input.Item2);
            }
        }
        public SystemData getSystemData() { return systemData; }
    }
}
