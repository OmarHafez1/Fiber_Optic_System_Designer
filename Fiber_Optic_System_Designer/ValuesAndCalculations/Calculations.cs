using Fiber_Optic_System_Designer.MyDialogBoxes;
using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class Calculations
    {
        const double ELECTRON_CHARGE = 1.6e-19;

        SystemData systemData;
        public Calculations(SystemData systemData)
        {
            this.systemData = systemData;
        }
        public String GetEnvironment()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ENVIRONMENT).getValue()) != null) return TMP;
            TMP = (double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue()) >= 1.0) ? "outdoor" : "indoor";
            systemData.SetValue(values_name.ENVIRONMENT, TMP);
            return TMP;
        }

        public String GetModulationCode()
        {
            systemData.SetValue(values_name.MODULATION_CODE, "NRZ");
            return "NRZ";
        }

        public String GetNoiseFactorOfPhotodetector()
        {
            return "1";
            /*String photoDetectorType = systemData.GetValue(values_name.PHOTODETECTOR_TYPE).getValue().ToLower();
            return (photoDetectorType.Contains("pin") ? "1" : photoDetectorType.Contains("si") ? ".4" : ".1");*/
        }
        public String GetRespositivityOfPhotodetector()
        {
            return ".5";
            /*String photoDetectorType = systemData.GetValue(values_name.PHOTODETECTOR_TYPE).getValue().ToLower();
            return (photoDetectorType.Contains("pin") ? ".58" : photoDetectorType.Contains("si") ? "100" : ".6");*/
        }
        public String GetReceiverSensitivity()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.RECEIVER_SENSITIVITY).getValue()) != null)
            {
                return TMP;
            }
            double snr = double.Parse(systemData.GetValue(values_name.REQUIRED_SNR).getValue());
            double k_2 = Math.Pow(10, .1 * snr);
            double F = double.Parse(GetNoiseFactorOfPhotodetector());
            double bitRate = double.Parse(systemData.GetValue(values_name.REQUIRED_BIT_RATE).getValue()) * 1e6;
            double R = double.Parse(GetRespositivityOfPhotodetector());
            double recSens = 2 * ELECTRON_CHARGE * F * k_2 * bitRate / R;
            recSens = 10 * Math.Log10(recSens / 1e-3);
            TMP = recSens.ToString();
            systemData.SetValue(values_name.RECEIVER_SENSITIVITY, TMP);
            return TMP;
        }

        public int ChoseDetector()
        {
            if (systemData.GetUsedDetectorIndex() != -1) return systemData.GetUsedDetectorIndex();

            List<string> minAccepablePowerLevelColumn = systemData.GetDetectorListOf("MIN. ACCEPTABLE POWER LEVEL");
            List<string> deviceStructureColumnn = systemData.GetDetectorListOf("DEVICE STRUCTURE");

            List<List<String>> availableDetectors = new List<List<string>>();
            availableDetectors.Add(systemData.GetColumnsNamesOf(systemData.GetDetectorChart()));

            double receiverSens = double.Parse(GetReceiverSensitivity());

            Dictionary<int, int> real_index = new Dictionary<int, int>();

            int tmp_index = 0;
            for (int i = 0; i < minAccepablePowerLevelColumn.Count; i++)
            {
                // TODO: HERE WE ASSUEMED THE PIN DEVICES. BUT I THE FUTURE WE WILL NEED TO EXTEND IT TO ACCEPT OTHER TYPES.
                if (double.Parse(minAccepablePowerLevelColumn[i]) <= receiverSens && deviceStructureColumnn[i].Trim() == "PIN")
                {
                    real_index[tmp_index++] = i;
                    availableDetectors.Add(systemData.GetRowValuesOf(systemData.GetDetectorChart(), i));
                }
            }

            if (availableDetectors.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable detector for your system. You need Detector with sensitivity <= {receiverSens}.");
            }

            if (availableDetectors.Count - 1 > 1)
            {
                String title = "Chose a detector";
                String message = "We have found different available detectors sutable for your system, Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableDetectors);
                systemData.setUsedDetectorIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedDetectorIndex(real_index[0]);
            return real_index[0];
        }

        public String GetFiberType()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.FIBER_TYPE).getValue()) != null) return TMP;
            TMP = double.Parse(systemData.GetValue(values_name.REQUIRED_BIT_RATE).getValue()) >= 1000 ? "Single Mode" : "Multi Mode";
            systemData.SetValue(values_name.FIBER_TYPE, TMP);
            return TMP;
        }

        public int ChoseOpticalFiber()
        {
            if (systemData.GetUsedOpticalFiberIndex() != -1) return systemData.GetUsedOpticalFiberIndex();

            List<string> BW_Distacne_Product_Column = systemData.GetOpticaFiberListOf("BANDWIDTH DIST. PROD.");

            List<List<String>> availableOpticalFibers = new List<List<string>>();
            availableOpticalFibers.Add(systemData.GetColumnsNamesOf(systemData.GetOpticalFiberChart()));

            double bandWidthDistProduct = double.Parse(systemData.GetValue(values_name.REQUIRED_BIT_RATE).getValue()) * double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue());

            Dictionary<int, int> real_index = new Dictionary<int, int>();

            int tmp_indx = 0;
            for (int i = 0; i < BW_Distacne_Product_Column.Count; i++)
            {
                if (!double.TryParse(BW_Distacne_Product_Column[i], out double result)) continue;
                // TODO: IN THE FUTURE UPDATE WE WILL NEED TO CHECK IF THE OPERATING WAVELENGTH OF MODEL OF THIS OPTICAL FIBER
                //       IS THE SAME AS THE WAVELENGTH OF PEAK SENSITIVITY OF THE DETECTOR WE CHOSED !!
                if (double.Parse(BW_Distacne_Product_Column[i]) >= bandWidthDistProduct)
                {
                    real_index[tmp_indx++] = i;
                    availableOpticalFibers.Add(systemData.GetRowValuesOf(systemData.GetOpticalFiberChart(), i));
                }
            }

            if (availableOpticalFibers.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable optical fiber for your system. You need optical fiber with BW Distance product >= {bandWidthDistProduct}.");
            }

            if (availableOpticalFibers.Count - 1 > 1)
            {
                String title = "Chose an optical fiber";
                String message = "We have found different available optical fibers sutable for your system, Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableOpticalFibers);
                systemData.setUsedOpticalFiberIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedOpticalFiberIndex(real_index[0]);
            return real_index[0];

        }

        public String GetFiberAttenuation()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.FIBER_ATTENUATION).getValue()) != null) return TMP;
            TMP = systemData.GetOpticaFiberListOf("ATTENUATION")[ChoseOpticalFiber()];
            if (TMP.Contains(':')) TMP = TMP.Split(':')[1];
            systemData.SetValue(values_name.FIBER_ATTENUATION, TMP);
            return TMP;
        }

        public String GetTotalFiberLoss()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.TOTAL_FIBER_LOSS).getValue()) != null) return TMP;
            double value = double.Parse(GetFiberAttenuation()) * double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue());
            TMP = value.ToString();
            systemData.SetValue(values_name.TOTAL_FIBER_LOSS, TMP);
            return TMP;
        }

        public String GetSourceType()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.SOURCE_TYPE).getValue()) != null) return TMP;
            bool isHightSpeed = double.Parse(systemData.GetValue(values_name.REQUIRED_BIT_RATE).getValue()) >= 1000;
            bool isLongDistance = double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue()) >= 1;
            TMP = (!isHightSpeed && !isLongDistance) ? "LED" : (isHightSpeed && !isLongDistance) ? "LASER/LED" : "LASER";
            systemData.SetValue(values_name.SOURCE_TYPE, TMP);
            return TMP;
        }

        public int ChoseSource()
        {
            if (systemData.GetUsedSourceIndex() != -1) return systemData.GetUsedSourceIndex();
            List<string> SourceType = systemData.GetSourceListOf("SOURCE TYPE");
            List<string> OperatingWaveLength = systemData.GetSourceListOf("OPERATING WAVELENGTH");
            List<List<String>> availableSources = new List<List<string>>();
            availableSources.Add(systemData.GetColumnsNamesOf(systemData.GetSourceChart()));

            List<int> OpticalFiberOpratingWavelength = new List<int>();
            String operatingWaveLengthOfOpticalFiber = systemData.GetOpticaFiberListOf("OPERATING WAVELENGTH OF MODEL")[ChoseOpticalFiber()];

            foreach (String str in operatingWaveLengthOfOpticalFiber.Split(":"))
            {
                OpticalFiberOpratingWavelength.Add(int.Parse(str));
            }

            Dictionary<int, int> real_index = new Dictionary<int, int>();
            int tmp_index = 0;

            for (int i = 0; i < SourceType.Count; i++)
            {
                bool ok = false;
                if (GetSourceType().Contains("/") || GetSourceType() == SourceType[i])
                {
                    foreach (int x in OpticalFiberOpratingWavelength)
                    {
                        foreach (String y in OperatingWaveLength[i].Split(":"))
                        {
                            if (x == int.Parse(y)) { ok = true; break; }
                        }
                        if (ok) break;
                    }
                }
                if (ok)
                {
                    real_index[tmp_index++] = i;
                    availableSources.Add(systemData.GetRowValuesOf(systemData.GetSourceChart(), i));
                }
            }

            if (availableSources.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable source for your system. You need {GetSourceType()} source with operating wavelength == {operatingWaveLengthOfOpticalFiber}.");
            }

            if (availableSources.Count - 1 > 1)
            {
                String title = "Chose an optical fiber";
                String message = "We have found different available Sources sutable for your system, Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableSources);
                systemData.setUsedSourceIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedSourceIndex(real_index[0]);
            return real_index[0];
        }

        public String GetAverageSourceOutput()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.AVERAGE_SOURCE_OUTPUT).getValue()) != null) return TMP;
            double outputPower = double.Parse(systemData.GetSourceListOf("OUTPUT POWER")[ChoseSource()]) / 1000.0;
            TMP = (10 * Math.Log10(outputPower)).ToString();
            systemData.SetValue(values_name.AVERAGE_SOURCE_OUTPUT, TMP);
            return TMP;
        }

        public String GetSplice()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.SPLICE).getValue()) != null) return TMP;
            TMP = GetFiberType().Contains("Single") ? "fusion" : "mechanical";
            systemData.SetValue(values_name.SPLICE, TMP);
            return TMP;
        }

        public String GetNumberOfSplices()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.NUMBER_OF_SPLICES).getValue()) != null) return TMP;
            double transmission_distance = double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue());
            double spool_length = double.Parse(systemData.GetOpticaFiberListOf("SPOOL LENGTH")[ChoseOpticalFiber()]);
            TMP = ((int)Math.Ceiling(transmission_distance / spool_length)).ToString();
            systemData.SetValue(values_name.NUMBER_OF_SPLICES, TMP);
            return TMP;
        }

        public String GetSplicesInsertionLoss()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.SPLICE_INSERTION_LOSS).getValue()) != null) return TMP;
            TMP = (GetSplice().ToLower().Contains("fusion") ? .1 : .5).ToString();
            systemData.SetValue(values_name.SPLICE_INSERTION_LOSS, TMP);
            return TMP;
        }

        public String GetTotalSpliceLoss()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.TOTAL_SPLICE_LOSS).getValue()) != null) return TMP;
            TMP = (double.Parse(GetSplicesInsertionLoss()) * double.Parse(GetNumberOfSplices())).ToString();
            systemData.SetValue(values_name.TOTAL_SPLICE_LOSS, TMP);
            return TMP;
        }

        public int ChoseConnector()
        {
            if (systemData.GetUsedConnectorIndex() != -1) return systemData.GetUsedConnectorIndex();
            List<string> ConnectorsFiberType = systemData.GetConnectorListOf("FIBER TYPE");
            List<string> ConnectorsFiberSize = systemData.GetConnectorListOf("FIBER SIZE");
            List<List<String>> availableConnnectors = new List<List<string>>();
            availableConnnectors.Add(systemData.GetColumnsNamesOf(systemData.GetConnectorChart()));

            String fiberSize = systemData.GetOpticaFiberListOf("FIBER SIZE")[ChoseOpticalFiber()].Trim();

            Dictionary<int, int> real_index = new Dictionary<int, int>();
            int tmp_index = 0;

            for (int i = 0; i < ConnectorsFiberType.Count; i++)
            {
                if (ConnectorsFiberType[i][0] == GetFiberType()[0])
                {
                    if (fiberSize == ConnectorsFiberSize[i].Trim())
                    {
                        real_index[tmp_index++] = i;
                        availableConnnectors.Add(systemData.GetRowValuesOf(systemData.GetConnectorChart(), i));
                    }
                }
            }

            if (availableConnnectors.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable source for your system. You need {GetFiberType} fiber with size == {fiberSize}.");
            }

            if (availableConnnectors.Count - 1 > 1)
            {
                String title = "Chose an connector";
                String message = "We have found different available connectors sutable for your system, Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableConnnectors);
                systemData.setUsedSourceIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedSourceIndex(real_index[0]);
            return real_index[0];
        }
        public String GetConnectorsInsertionLoss()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.CONNECTOR_INSERTION_LOSS).getValue()) != null) return TMP;
            TMP = systemData.GetConnectorListOf("ATTENUATION")[ChoseConnector()];
            systemData.SetValue(values_name.CONNECTOR_INSERTION_LOSS, TMP);
            return TMP;
        }

        public String GetTotalConnectorLoss()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.TOTAL_CONNECTOR_LOSS).getValue()) != null) return TMP;
            TMP = (double.Parse(GetConnectorsInsertionLoss()) * int.Parse(systemData.GetValue(values_name.NUMBER_OF_CONNECTORS).getValue())).ToString();
            systemData.SetValue(values_name.TOTAL_CONNECTOR_LOSS, TMP);
            return TMP;
        }
        public String GetTimeDegradationFactor()
        {
            systemData.SetValue(values_name.TIME_DEGRADATION_FACTOR, "3");

            return systemData.GetValue(values_name.TIME_DEGRADATION_FACTOR).getValue();
        }
        public String GetEnvDegradationFactor()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ENV_DEGRADATION_FACTOR).getValue()) != null) return TMP;
            TMP = GetEnvironment().ToLower().Trim()[0] == 'i' ? "2" : "5";
            systemData.SetValue(values_name.ENV_DEGRADATION_FACTOR, TMP);
            return TMP;
        }

        public String GetTotalAttenuation()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.TOTAL_ATTENUATION).getValue()) != null) return TMP;
            double res = double.Parse(GetTotalFiberLoss()) + double.Parse(GetTotalSpliceLoss()) + double.Parse(GetTotalConnectorLoss());
            res += double.Parse(GetTimeDegradationFactor()) + double.Parse(GetEnvDegradationFactor());
            TMP = res.ToString();
            systemData.SetValue(values_name.TOTAL_ATTENUATION, TMP);
            return TMP;
        }

        public String GetTotalLossMargin()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.TOTAL_LOSS_MARGIN).getValue()) != null) return TMP;
            TMP = (double.Parse(GetAverageSourceOutput()) - double.Parse(GetReceiverSensitivity())).ToString();
            systemData.SetValue(values_name.TOTAL_LOSS_MARGIN, TMP);
            return TMP;
        }
        public String GetExcessPower()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.EXCESS_POWER).getValue()) != null) return TMP;
            TMP = (double.Parse(GetTotalLossMargin()) + double.Parse(GetTotalAttenuation())).ToString();
            systemData.SetValue(values_name.EXCESS_POWER, TMP);
            return TMP;
        }
        public String GetActualPowerAtReceiver()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ACTUAL_POWER_AT_THE_RECEIVER).getValue()) != null) return TMP;
            TMP = (double.Parse(GetReceiverSensitivity()) + double.Parse(GetExcessPower())).ToString();
            systemData.SetValue(values_name.ACTUAL_POWER_AT_THE_RECEIVER, TMP);
            return TMP;
        }

        public String GetRequiredSystemRiseTime()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.REQUIRED_SYSTEM_RISE_TIME).getValue()) != null) return TMP;
            String tmp = GetModulationCode().Trim().ToLower();
            TMP = (tmp == "nrz" || tmp == "nrzi" || tmp == "miller") ? ".7" : ".35";
            systemData.SetValue(values_name.REQUIRED_SYSTEM_RISE_TIME, TMP);
            return TMP;
        }

        public String GetFiberBW()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.FIBER_BANDWIDTH).getValue()) != null) return TMP;
            TMP = (double.Parse(systemData.GetOpticaFiberListOf("BANDWIDTH DIST. PROD.")[ChoseOpticalFiber()]) / double.Parse(systemData.GetValue(values_name.TRANSMISSION_DISTANCE).getValue())).ToString();
            systemData.SetValue(values_name.FIBER_BANDWIDTH, TMP);
            return TMP;
        }
        public String GetFiberRiseTime()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.FIBER_RISE_TIME).getValue()) != null) return TMP;
            TMP = (.35 / double.Parse(GetFiberBW())).ToString();
            systemData.SetValue(values_name.FIBER_RISE_TIME, TMP);
            return TMP;
        }

        public String GetSourceRiseTime()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.SOURCE_RISE_TIME).getValue()) != null) return TMP;
            TMP = systemData.GetSourceListOf("RISE TIME")[ChoseSource()].ToString();
            systemData.SetValue(values_name.SOURCE_RISE_TIME, TMP);
            return TMP;
        }
        public String GetDetectorRiseTime()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.DETECTOR_RISE_TIME).getValue()) != null) return TMP;
            TMP = systemData.GetDetectorListOf("RISE TIME")[ChoseDetector()].ToString();
            systemData.SetValue(values_name.DETECTOR_RISE_TIME, TMP);
            return TMP;
        }

        public String GetActualSystemRiseTime()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ACTUAL_SYSTEM_RISE_TIME).getValue()) != null) return TMP;
            double res = 1.1 * Math.Sqrt(Math.Pow(double.Parse(GetFiberRiseTime()), 2) + Math.Pow(double.Parse(GetSourceRiseTime()), 2) + Math.Pow(double.Parse(GetDetectorRiseTime()), 2));
            TMP = res.ToString();
            systemData.SetValue(values_name.ACTUAL_SYSTEM_RISE_TIME, TMP);
            return TMP;
        }
        public String GetActualBitRate()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ACTUAL_BITRATE).getValue()) != null) return TMP;

            double res = double.Parse(GetRequiredSystemRiseTime()) / double.Parse(GetActualSystemRiseTime()) * 1000;
            TMP = res.ToString();
            systemData.SetValue(values_name.ACTUAL_BITRATE, TMP);
            return TMP;
        }

        public String GetActualSNR()
        {
            String TMP;
            if ((TMP = systemData.GetValue(values_name.ACTUAL_SNR).getValue()) != null) return TMP;

            double p = Math.Pow(10, double.Parse(GetActualPowerAtReceiver()) / 10.0) * 10000;
            double snr = 10 * Math.Log10(p * double.Parse(GetRespositivityOfPhotodetector()) / (2 * ELECTRON_CHARGE * double.Parse(GetNoiseFactorOfPhotodetector()) * double.Parse(GetActualBitRate())));
            TMP = snr.ToString();
            systemData.SetValue(values_name.ACTUAL_SNR, TMP);
            return TMP;
        }

    }
}
