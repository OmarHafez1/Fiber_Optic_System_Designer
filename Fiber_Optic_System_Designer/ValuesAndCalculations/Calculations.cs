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
        public string GetEnvironment()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ENVIRONMENT)) != null) return VAL;
            VAL = (systemData.GetValue(values_name.TRANSMISSION_DISTANCE) >= 1.0) ? "outdoor" : "indoor";
            systemData.SetValue(values_name.ENVIRONMENT, VAL);
            return VAL;
        }

        public string GetModulationCode()
        {
            systemData.SetValue(values_name.MODULATION_CODE, "NRZ");
            return "NRZ";
        }

        public double GetNoiseFactorOfPhotodetector()
        {
            return 1;
            /*string photoDetectorType = systemData.GetValue(values_name.PHOTODETECTOR_TYPE).ToLower();
            return (photoDetectorType.Contains("pin") ? "1" : photoDetectorType.Contains("si") ? ".4" : ".1");*/
        }
        public double GetRespositivityOfPhotodetector()
        {
            return .5;
            /*string photoDetectorType = systemData.GetValue(values_name.PHOTODETECTOR_TYPE).ToLower();
            return (photoDetectorType.Contains("pin") ? ".58" : photoDetectorType.Contains("si") ? "100" : ".6");*/
        }
        public double GetReceiverSensitivity()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.RECEIVER_SENSITIVITY)) != null)
            {
                return VAL;
            }
            double snr = systemData.GetValue(values_name.REQUIRED_SNR);
            double k_2 = Math.Pow(10, .1 * snr);
            double F = GetNoiseFactorOfPhotodetector();
            double bitRate = systemData.GetValue(values_name.REQUIRED_BIT_RATE) * 1e6;
            double R = GetRespositivityOfPhotodetector();
            double recSens = 2 * ELECTRON_CHARGE * F * k_2 * bitRate / R;
            recSens = 10 * Math.Log10(recSens / 1e-3);
            systemData.SetValue(values_name.RECEIVER_SENSITIVITY, recSens);
            return recSens;
        }

        public int ChoseDetector()
        {
            if (systemData.GetUsedDetectorIndex() != -1) return systemData.GetUsedDetectorIndex();

            List<string> minAccepablePowerLevelColumn = systemData.GetDetectorListOf("MIN. ACCEPTABLE POWER LEVEL");
            List<string> deviceStructureColumnn = systemData.GetDetectorListOf("DEVICE STRUCTURE");

            List<List<string>> availableDetectors = new List<List<string>>();
            availableDetectors.Add(systemData.GetColumnsNamesOf(systemData.GetDetectorChart()));

            double receiverSens = GetReceiverSensitivity();

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
                string title = "Chose a detector";
                string message = "We have found various detectors available that are suitable for your system. Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableDetectors);
                systemData.setUsedDetectorIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedDetectorIndex(real_index[0]);
            return real_index[0];
        }

        public string GetFiberType()
        {
            string VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_TYPE)) != null) return VAL;
            VAL = systemData.GetValue(values_name.REQUIRED_BIT_RATE) >= 1000 ? "Single mode" : "Multi mode";
            systemData.SetValue(values_name.FIBER_TYPE, VAL);
            return VAL;
        }

        public int ChoseOpticalFiber()
        {
            if (systemData.GetUsedOpticalFiberIndex() != -1) return systemData.GetUsedOpticalFiberIndex();

            List<string> BW_Distacne_Product_Column = systemData.GetOpticaFiberListOf("BANDWIDTH DIST. PROD.");

            List<List<string>> availableOpticalFibers = new List<List<string>>
            {
                systemData.GetColumnsNamesOf(systemData.GetOpticalFiberChart())
            };

            double bandWidthDistProduct = systemData.GetValue(values_name.REQUIRED_BIT_RATE) * systemData.GetValue(values_name.TRANSMISSION_DISTANCE);

            string detector_operating_wave_length = systemData.GetDetectorListOf("WAVELENGTH OF PEAK SENSITIVITY")[ChoseDetector()].Trim().ToLower();
            List<string> optical_fiber_oprating_wave_length = systemData.GetOpticaFiberListOf("OPERATING WAVELENGTH OF MODEL");

            Dictionary<int, int> real_index = new Dictionary<int, int>();

            int tmp_indx = 0;
            for (int i = 0; i < BW_Distacne_Product_Column.Count; i++)
            {
                // i.e. this cell in the chart is empty.
                if (!double.TryParse(BW_Distacne_Product_Column[i], out _)) continue;

                // TODO: IN THE FUTURE UPDATE WE WILL NEED TO CHECK IF THE OPERATING WAVELENGTH OF MODEL OF THIS OPTICAL FIBER
                //       IS THE SAME AS THE WAVELENGTH OF PEAK SENSITIVITY OF THE DETECTOR WE CHOSED !!

                if (double.Parse(BW_Distacne_Product_Column[i]) >= bandWidthDistProduct)
                {
                    foreach (var x in optical_fiber_oprating_wave_length[i].Split(":"))
                    {
                        if (x.ToLower().Trim() == detector_operating_wave_length)
                        {
                            real_index[tmp_indx++] = i;
                            availableOpticalFibers.Add(systemData.GetRowValuesOf(systemData.GetOpticalFiberChart(), i));
                            break;
                        }
                    }
                }
            }

            if (availableOpticalFibers.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable optical fiber for your system. You need optical fiber with BW Distance product >= {bandWidthDistProduct}.");
            }

            if (availableOpticalFibers.Count - 1 > 1)
            {
                string title = "Chose an optical fiber";
                string message = "We have found various optical fibers available that are suitable for your system. Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableOpticalFibers);
                systemData.setUsedOpticalFiberIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedOpticalFiberIndex(real_index[0]);
            return real_index[0];
        }

        public double GetFiberAttenuation()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_ATTENUATION)) != null) return VAL;
            string tmp = systemData.GetOpticaFiberListOf("ATTENUATION")[ChoseOpticalFiber()];
            if (tmp.Contains(':')) VAL = double.Parse(tmp.Split(':')[1]);
            systemData.SetValue(values_name.FIBER_ATTENUATION, VAL);
            return VAL;
        }

        public double GetTotalFiberLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_FIBER_LOSS)) != null) return VAL;
            VAL = GetFiberAttenuation() * systemData.GetValue(values_name.TRANSMISSION_DISTANCE);
            systemData.SetValue(values_name.TOTAL_FIBER_LOSS, VAL);
            return VAL;
        }

        public string GetSourceType()
        {
            string VAL;
            if ((VAL = systemData.GetValue(values_name.SOURCE_TYPE)) != null) return VAL;
            bool isHightSpeed = systemData.GetValue(values_name.REQUIRED_BIT_RATE) >= 1000;
            bool isLongDistance = systemData.GetValue(values_name.TRANSMISSION_DISTANCE) >= 1;
            VAL = (!isHightSpeed && !isLongDistance) ? "LED" : (isHightSpeed && !isLongDistance) ? "LASER/LED" : "LASER";
            systemData.SetValue(values_name.SOURCE_TYPE, VAL);
            return VAL;
        }

        public int ChoseSource()
        {
            if (systemData.GetUsedSourceIndex() != -1) return systemData.GetUsedSourceIndex();

            List<string> SourceTypes = systemData.GetSourceListOf("SOURCE TYPE");
            List<string> SourceOperatingWaveLengths = systemData.GetSourceListOf("OPERATING WAVELENGTH");

            List<List<string>> availableSources = new List<List<string>>
            {
                systemData.GetColumnsNamesOf(systemData.GetSourceChart())
            };

            string DetectorOperatingWavelength = systemData.GetDetectorListOf("WAVELENGTH OF PEAK SENSITIVITY")[ChoseDetector()];

            Dictionary<int, int> real_index = new Dictionary<int, int>();
            int tmp_index = 0;

            for (int i = 0; i < SourceTypes.Count; i++)
            {
                if (GetSourceType().Contains("/") || GetSourceType().ToLower() == SourceTypes[i].ToLower())
                {
                    if (DetectorOperatingWavelength.Trim().ToLower() == SourceOperatingWaveLengths[i].Trim().ToLower())
                    {
                        real_index[tmp_index++] = i;
                        availableSources.Add(systemData.GetRowValuesOf(systemData.GetSourceChart(), i));
                    }
                }
            }

            if (availableSources.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable source for your system. You need {GetSourceType()} source with operating wavelength == {DetectorOperatingWavelength}.");
            }

            if (availableSources.Count - 1 > 1)
            {
                string title = "Chose an optical fiber";
                string message = "We have found various sources available that are suitable for your system. Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableSources);
                systemData.setUsedSourceIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }
            systemData.setUsedSourceIndex(real_index[0]);
            return real_index[0];
        }

        public double GetAverageSourceOutput()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.AVERAGE_SOURCE_OUTPUT)) != null) return VAL;
            double outputPower = double.Parse(systemData.GetSourceListOf("OUTPUT POWER")[ChoseSource()]) / 1000.0;
            VAL = 10 * Math.Log10(outputPower);
            systemData.SetValue(values_name.AVERAGE_SOURCE_OUTPUT, VAL);
            return VAL;
        }

        public string GetSplice()
        {
            string VAL;
            if ((VAL = systemData.GetValue(values_name.SPLICE)) != null) return VAL;
            VAL = GetFiberType().Contains("Single") ? "fusion" : "mechanical";
            systemData.SetValue(values_name.SPLICE, VAL);
            return VAL;
        }

        public int GetNumberOfSplices()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.NUMBER_OF_SPLICES)) != null) return VAL;
            double transmission_distance = systemData.GetValue(values_name.TRANSMISSION_DISTANCE);
            double spool_length = double.Parse(systemData.GetOpticaFiberListOf("SPOOL LENGTH")[ChoseOpticalFiber()]);
            VAL = (int)Math.Ceiling(transmission_distance * 1000 / spool_length) - 1;
            systemData.SetValue(values_name.NUMBER_OF_SPLICES, VAL);
            return VAL;
        }

        public double GetSplicesInsertionLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.SPLICE_INSERTION_LOSS)) != null) return VAL;
            VAL = GetSplice().ToLower().Contains("fusion") ? .1 : .5;
            systemData.SetValue(values_name.SPLICE_INSERTION_LOSS, VAL);
            return VAL;
        }

        public double GetTotalSpliceLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_SPLICE_LOSS)) != null) return VAL;
            VAL = GetSplicesInsertionLoss() * GetNumberOfSplices();
            systemData.SetValue(values_name.TOTAL_SPLICE_LOSS, VAL);
            return VAL;
        }

        public int ChoseConnector()
        {
            if (systemData.GetUsedConnectorIndex() != -1) return systemData.GetUsedConnectorIndex();
            List<string> ConnectorsFiberType = systemData.GetConnectorListOf("FIBER TYPE");
            List<string> ConnectorsFiberSize = systemData.GetConnectorListOf("FIBER SIZE");
            List<List<string>> availableConnnectors = new List<List<string>>
            {
                systemData.GetColumnsNamesOf(systemData.GetConnectorChart())
            };

            string OpticalFiberSize = systemData.GetOpticaFiberListOf("FIBER SIZE")[ChoseOpticalFiber()].Trim();

            Dictionary<int, int> real_index = new Dictionary<int, int>();
            int tmp_index = 0;

            for (int i = 0; i < ConnectorsFiberType.Count; i++)
            {
                if (ConnectorsFiberType[i][0] == GetFiberType()[0])
                {
                    if (OpticalFiberSize == ConnectorsFiberSize[i].Trim())
                    {
                        real_index[tmp_index++] = i;
                        availableConnnectors.Add(systemData.GetRowValuesOf(systemData.GetConnectorChart(), i));
                    }
                }
            }

            if (availableConnnectors.Count - 1 == 0)
            {
                throw new Exception($"Didn't find suitable source for your system. You need {GetFiberType()} fiber with size == {OpticalFiberSize}.");
            }

            if (availableConnnectors.Count - 1 > 1)
            {
                string title = "Chose an connector";
                string message = "We have found various connectors available that are suitable for your system. Please select one:";
                int chosed_index = ChoseTableDialoge.InputDialog(title, message, availableConnnectors);
                systemData.setUsedConnectorIndex(real_index[chosed_index]);
                return real_index[chosed_index];
            }

            systemData.setUsedConnectorIndex(real_index[0]);
            return real_index[0];
        }
        public double GetConnectorsInsertionLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.CONNECTOR_INSERTION_LOSS)) != null) return VAL;
            VAL = double.Parse(systemData.GetConnectorListOf("ATTENUATION")[ChoseConnector()]);
            systemData.SetValue(values_name.CONNECTOR_INSERTION_LOSS, VAL);
            return VAL;
        }

        public double GetTotalConnectorLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_CONNECTOR_LOSS)) != null) return VAL;
            VAL = GetConnectorsInsertionLoss() * systemData.GetValue(values_name.NUMBER_OF_CONNECTORS);
            systemData.SetValue(values_name.TOTAL_CONNECTOR_LOSS, VAL);
            return VAL;
        }
        public double GetTimeDegradationFactor()
        {
            systemData.SetValue(values_name.TIME_DEGRADATION_FACTOR, 3);
            return systemData.GetValue(values_name.TIME_DEGRADATION_FACTOR);
        }
        public double GetEnvDegradationFactor()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ENV_DEGRADATION_FACTOR)) != null) return VAL;
            VAL = GetEnvironment().ToLower().Trim()[0] == 'i' ? 2 : 5;
            systemData.SetValue(values_name.ENV_DEGRADATION_FACTOR, VAL);
            return VAL;
        }

        public double GetTotalAttenuation()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_ATTENUATION)) != null) return VAL;
            VAL = -1 * (GetTotalFiberLoss() + GetTotalSpliceLoss() + GetTotalConnectorLoss() +
                               GetTimeDegradationFactor() + GetEnvDegradationFactor());
            systemData.SetValue(values_name.TOTAL_ATTENUATION, VAL);
            return VAL;
        }

        public double GetTotalLossMargin()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_LOSS_MARGIN)) != null) return VAL;
            VAL = GetAverageSourceOutput() - GetReceiverSensitivity();
            systemData.SetValue(values_name.TOTAL_LOSS_MARGIN, VAL);
            return VAL;
        }
        public dynamic GetExcessPower()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.EXCESS_POWER)) != null) return VAL;
            VAL = GetTotalLossMargin() + GetTotalAttenuation();
            systemData.SetValue(values_name.EXCESS_POWER, VAL);
            return VAL;
        }
        public double GetActualPowerAtReceiver()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ACTUAL_POWER_AT_THE_RECEIVER)) != null) return VAL;
            VAL = GetReceiverSensitivity() + GetExcessPower();
            systemData.SetValue(values_name.ACTUAL_POWER_AT_THE_RECEIVER, VAL);
            return VAL;
        }

        public double GetRequiredSystemRiseTime()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.REQUIRED_SYSTEM_RISE_TIME)) != null) return VAL;
            string tmp = GetModulationCode().Trim().ToLower();
            VAL = (tmp == "nrz" || tmp == "nrzi" || tmp == "miller") ? .7 : .35;
            VAL /= systemData.GetValue(values_name.REQUIRED_BIT_RATE);
            VAL *= 1000;
            systemData.SetValue(values_name.REQUIRED_SYSTEM_RISE_TIME, VAL);
            return VAL;
        }

        public double GetFiberBW()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_BANDWIDTH)) != null) return VAL;
            VAL = double.Parse(systemData.GetOpticaFiberListOf("BANDWIDTH DIST. PROD.")[ChoseOpticalFiber()]) / systemData.GetValue(values_name.TRANSMISSION_DISTANCE);
            systemData.SetValue(values_name.FIBER_BANDWIDTH, VAL);
            return VAL;
        }
        public double GetFiberRiseTime()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_RISE_TIME)) != null) return VAL;
            VAL = .35 * 1000 / GetFiberBW();
            systemData.SetValue(values_name.FIBER_RISE_TIME, VAL);
            return VAL;
        }

        public double GetSourceRiseTime()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.SOURCE_RISE_TIME)) != null) return VAL;
            VAL = double.Parse(systemData.GetSourceListOf("RISE TIME")[ChoseSource()]);
            systemData.SetValue(values_name.SOURCE_RISE_TIME, VAL);
            return VAL;
        }
        public double GetDetectorRiseTime()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.DETECTOR_RISE_TIME)) != null) return VAL;
            VAL = double.Parse(systemData.GetDetectorListOf("RISE TIME")[ChoseDetector()]);
            systemData.SetValue(values_name.DETECTOR_RISE_TIME, VAL);
            return VAL;
        }

        public double GetActualSystemRiseTime()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ACTUAL_SYSTEM_RISE_TIME)) != null) return VAL;
            VAL = 1.1 * Math.Sqrt(Math.Pow(GetFiberRiseTime(), 2) + Math.Pow(GetSourceRiseTime(), 2) + Math.Pow(GetDetectorRiseTime(), 2));
            systemData.SetValue(values_name.ACTUAL_SYSTEM_RISE_TIME, VAL);
            return VAL;
        }
        public double GetActualBitRate()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ACTUAL_BITRATE)) != null) return VAL;

            string tmp = GetModulationCode().Trim().ToLower();
            double x = (tmp == "nrz" || tmp == "nrzi" || tmp == "miller") ? .7 : .35;

            VAL = x / GetActualSystemRiseTime() * 1000;
            systemData.SetValue(values_name.ACTUAL_BITRATE, VAL);
            return VAL;
        }

        public double GetActualSNR()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.ACTUAL_SNR)) != null) return VAL;

            double p = Math.Pow(10, GetActualPowerAtReceiver() / 10.0) * 1e-3;
            VAL = 10 * Math.Log10(p * GetRespositivityOfPhotodetector() / (2 * ELECTRON_CHARGE * GetNoiseFactorOfPhotodetector() * GetActualBitRate() * 1e6));
            systemData.SetValue(values_name.ACTUAL_SNR, VAL);
            return VAL;
        }

    }
}
