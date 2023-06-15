using Fiber_Optic_System_Designer.MyDialogBoxes;
using Fiber_Optic_System_Designer.MyExceptions;
using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public class Calculations
    {
        const double ELECTRON_CHARGE = 1.60217663e-19;
        private string OpticalFiberColumnHeaderName = "BANDWIDTH DIST. PROD.";

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

        public int ChoseDetector()
        {
            if (systemData.GetUsedDetectorIndex() != -1) return systemData.GetUsedDetectorIndex();

            List<string> minAccepablePowerLevelColumn = systemData.GetDetectorListOf("MIN. ACCEPTABLE POWER LEVEL");
            List<string> noiseFactor = systemData.GetDetectorListOf("NOISE FACTOR");
            List<string> responsitivity = systemData.GetDetectorListOf("RESPONSITIVITY");

            List<List<string>> availableDetectors = new List<List<string>>
            {
                systemData.GetColumnsNamesOf(systemData.GetDetectorChart())
            };

            List<string> Pmin = new List<string>();

            Dictionary<int, int> real_index = new Dictionary<int, int>();

            int tmp_index = 0;
            for (int i = 0; i < minAccepablePowerLevelColumn.Count; i++)
            {
                double R = double.Parse(responsitivity[i]);
                double F = double.Parse(noiseFactor[i]);
                double tmp;
                if (double.Parse(minAccepablePowerLevelColumn[i]) <= (tmp = GetReceiverSensitivity(R, F)))
                {
                    real_index[tmp_index++] = i;
                    availableDetectors.Add(systemData.GetRowValuesOf(systemData.GetDetectorChart(), i));
                    Pmin.Add(tmp.ToString("0.0000"));
                }
            }

            if (availableDetectors.Count - 1 == 0)
            {
                throw new CantFindSuitableComponentsException($"We couldn't find a suitable detector for your system.");
            }

            int chosed_index = 0;
            if (availableDetectors.Count - 1 > 1)
            {
                string title = "Please choose a detector:";
                string message = "We have identified several detectors that are compatible with your system. Please select one from the following options:";
                chosed_index = ChoseTableDialoge.InputDialog(title, message, availableDetectors, Pmin);
            }

            systemData.SetValue(values_name.NOISE_FACTOR, double.Parse(noiseFactor[real_index[chosed_index]]));
            systemData.SetValue(values_name.PHOTODETECTOR_RESPONSITIVITY, double.Parse(responsitivity[real_index[chosed_index]]));
            systemData.setUsedDetectorIndex(real_index[chosed_index]);
            return real_index[chosed_index];
        }

        public double GetReceiverSensitivity(double ResponsitivityOfPhotodetector, double NoiseFactor)
        {
            double snr = systemData.GetValue(values_name.REQUIRED_SNR);
            double k_2 = Math.Pow(10, .1 * snr);
            double bitRate = systemData.GetValue(values_name.REQUIRED_BIT_RATE) * 1e6;
            double recSens = 2 * ELECTRON_CHARGE * NoiseFactor * k_2 * bitRate / ResponsitivityOfPhotodetector;
            recSens = 10 * Math.Log10(recSens / 1e-3);
            return recSens;
        }

        public double GetReceiverSensitivity()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.RECEIVER_SENSITIVITY)) != null)
            {
                return VAL;
            }
            VAL = GetReceiverSensitivity(GetResponsitivityOfPhotodetector(), GetNoiseFactorOfPhotodetector());
            systemData.SetValue(values_name.RECEIVER_SENSITIVITY, VAL);
            return VAL;
        }
        public double GetNoiseFactorOfPhotodetector()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.NOISE_FACTOR)) != null) return VAL;
            throw new ValueNotSet("Didn't find the noise factor of the the detector");
        }
        public double GetResponsitivityOfPhotodetector()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.PHOTODETECTOR_RESPONSITIVITY)) != null) return VAL;
            throw new ValueNotSet("Didn't find the responsitivity of the the detector");
        }


        public string GetFiberType()
        {
            string VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_TYPE)) != null) return VAL;
            VAL = systemData.GetValue(values_name.REQUIRED_BIT_RATE) >= 1000 ? "Single mode" : "Multi mode";
            systemData.SetValue(values_name.FIBER_TYPE, VAL);
            return VAL;
        }

        public double GetDispersion()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.DISPERSION)) != null) return VAL;
            double dispersion = .187 * 1000 / (systemData.GetValue(values_name.REQUIRED_BIT_RATE) * systemData.GetValue(values_name.TRANSMISSION_DISTANCE) * 4.0);
            systemData.SetValue(values_name.DISPERSION, dispersion);
            return dispersion;
        }

        private bool Is_Single_Mode()
        {
            return GetFiberType().Trim().ToLower()[0] == 's';
        }

        private bool Is_Multi_Mode()
        {
            return GetFiberType().Trim().ToLower()[0] == 'm';
        }

        private void UpdateOpticalFiberColumnHeaderName()
        {
            if (Is_Multi_Mode())
            {
                OpticalFiberColumnHeaderName = "BANDWIDTH DIST. PROD.";
            }
            else if (Is_Single_Mode())
            {
                OpticalFiberColumnHeaderName = "DISPERSION";
            }
        }

        private string getOpticalFiberColumnHeaderName()
        {
            UpdateOpticalFiberColumnHeaderName();
            return OpticalFiberColumnHeaderName;
        }

        public int ChoseOpticalFiber()
        {
            if (systemData.GetUsedOpticalFiberIndex() != -1) return systemData.GetUsedOpticalFiberIndex();

            List<string> Dispersion_BW_Distacne_Product_Column;
            double Dispersion_BW_Distacne_Product;
            if (Is_Multi_Mode())
            {
                Dispersion_BW_Distacne_Product_Column = systemData.GetOpticaFiberListOf(getOpticalFiberColumnHeaderName());
                Dispersion_BW_Distacne_Product = systemData.GetValue(values_name.REQUIRED_BIT_RATE) * systemData.GetValue(values_name.TRANSMISSION_DISTANCE);
            }
            else if (Is_Single_Mode())
            {
                Dispersion_BW_Distacne_Product_Column = systemData.GetOpticaFiberListOf(getOpticalFiberColumnHeaderName());
                Dispersion_BW_Distacne_Product = GetDispersion();
            }
            else
            {
                throw new Exception("Fiber type is not valid.");
            }

            string detector_operating_wave_length = systemData.GetDetectorListOf("WAVELENGTH OF PEAK SENSITIVITY")[ChoseDetector()].Trim().ToLower();
            List<string> optical_fiber_oprating_wave_length = systemData.GetOpticaFiberListOf("OPERATING WAVELENGTH OF MODEL");

            List<List<string>> availableOpticalFibers = new List<List<string>>
            {
                systemData.GetColumnsNamesOf(systemData.GetOpticalFiberChart())
            };

            Dictionary<int, int> real_index = new Dictionary<int, int>();

            int tmp_indx = 0;
            for (int i = 0; i < Dispersion_BW_Distacne_Product_Column.Count; i++)
            {
                if (Dispersion_BW_Distacne_Product_Column[i] == null || !double.TryParse(Dispersion_BW_Distacne_Product_Column[i], out _)) continue;

                if (double.Parse(Dispersion_BW_Distacne_Product_Column[i]) >= Dispersion_BW_Distacne_Product)
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
                string tmp = Is_Multi_Mode() ? "BW distance product" : "dispersion";
                throw new CantFindSuitableComponentsException($"We couldn't find a suitable optical fiber for your system. You require an optical fiber with a {tmp} >= {Dispersion_BW_Distacne_Product}.");
            }

            int chosed_index = 0;
            if (availableOpticalFibers.Count - 1 > 1)
            {
                string title = "Please choose an optical fiber:";
                string message = "We have identified several optical fibers that are compatible with your system. Please select one from the following options:";
                chosed_index = ChoseTableDialoge.InputDialog(title, message, availableOpticalFibers);
            }

            systemData.setUsedOpticalFiberIndex(real_index[chosed_index]);
            return real_index[chosed_index];
        }

        public double GetFiberAttenuation()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.FIBER_ATTENUATION)) != null) return VAL;
            string tmp = systemData.GetOpticaFiberListOf("ATTENUATION")[ChoseOpticalFiber()];
            if (tmp.Contains(':')) VAL = double.Parse(tmp.Split(':')[1]);
            else VAL = double.Parse(tmp);
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
                throw new CantFindSuitableComponentsException($"We couldn't find a suitable source for your system. You require {GetSourceType()} source with operating wavelength == {DetectorOperatingWavelength}.");
            }

            if (availableSources.Count - 1 > 1)
            {
                string title = "Please select a source:";
                string message = "We have identified several sources that are suitable for your system. Please choose one from the available sources:";
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

        public List<Tuple<int, int>> ChoseConnectors()
        {
            if (systemData.GetUsedConnectors() != null) return systemData.GetUsedConnectors();
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
                throw new CantFindSuitableComponentsException($"We couldn't find a suitable connector for your system. You require {GetFiberType()} fiber with size == {OpticalFiberSize}.");
            }

            string title = "Please chose the connectors:";
            string message = "The following connectors are compatible with your system. Please chose the connectors:";

            ChoseTableDialoge.InputDialog(title, message, availableConnnectors, isConnector: true);

            List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

            int cnt = 0;
            int number_of_connectors = 0;
            foreach (int x in ChoseTableDialoge.getConnectosCount())
            {
                if (x != 0)
                {
                    tuples.Add(new Tuple<int, int>(real_index[cnt], x));
                    number_of_connectors += x;
                }
                cnt++;
            }
            systemData.SetValue(values_name.NUMBER_OF_CONNECTORS, number_of_connectors);
            systemData.setUsedConnectorIndex(tuples);
            return tuples;
        }

        /*        public double GetConnectorsInsertionLoss()
                {
                    dynamic VAL;
                    if ((VAL = systemData.GetValue(values_name.CONNECTOR_INSERTION_LOSS)) != null) return VAL;
                    var connectors = ChoseConnectors();
                    foreach (var x in connectors)
                    {
                        VAL = double.Parse(systemData.GetConnectorListOf("ATTENUATION")[ChoseConnector()]);
                    }
                    systemData.SetValue(values_name.CONNECTOR_INSERTION_LOSS, VAL);
                    return VAL;
                }*/

        public double GetTotalConnectorLoss()
        {
            dynamic VAL;
            if ((VAL = systemData.GetValue(values_name.TOTAL_CONNECTOR_LOSS)) != null) return VAL;
            var connectors_attenuation = systemData.GetConnectorListOf("ATTENUATION");
            var connectors = ChoseConnectors();
            foreach (var x in connectors)
            {
                VAL = x.Item2 * double.Parse(connectors_attenuation[x.Item1]);
            }
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

            VAL = double.Parse(systemData.GetOpticaFiberListOf(getOpticalFiberColumnHeaderName())[ChoseOpticalFiber()]) / systemData.GetValue(values_name.TRANSMISSION_DISTANCE);
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
            VAL = 10 * Math.Log10(p * GetResponsitivityOfPhotodetector() / (2 * ELECTRON_CHARGE * GetNoiseFactorOfPhotodetector() * GetActualBitRate() * 1e6));
            systemData.SetValue(values_name.ACTUAL_SNR, VAL);
            return VAL;
        }
    }
}
