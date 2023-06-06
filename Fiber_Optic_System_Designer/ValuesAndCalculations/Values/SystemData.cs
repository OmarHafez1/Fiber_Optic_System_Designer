using Fiber_Optic_System_Designer.ReadJSON;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations.Values
{
    public class SystemData
    {
        private Dictionary<values_name, Data> ValuesDictionary;
        private List<values_name> SystemRequirementsAnalysisValues;
        private List<values_name> OpticalPowerBudgetAnalysisValues;
        private List<values_name> SystemRiseTimeAnalysisValues;
        private List<values_name> ActualSystemSpecificationValues;
        private List<values_name> AllValues;

        private List<List<Tuple<String, String>>> DetectorChart;
        private List<List<Tuple<String, String>>> SourceChart;
        private List<List<Tuple<String, String>>> OpticalFiberChart;
        private List<List<Tuple<String, String>>> ConnectorChart;

        private Dictionary<string, List<string>> DetectorDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> SourceDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> OpticalFiberDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> ConnectorDictionary = new Dictionary<string, List<string>>();


        private int UsedDetectorIndex = -1, UsedSourceIndex = -1, UsedOpticalFiberIndex = -1, UsedConnectorIndex = -1;

        /*
                private List<int> AvailableDetectors;
                private List<int> AvailableSources;
                private List<int> AvailableOpticalFibers;
                private List<int> AvailableConnectors;
        */

        public SystemData()
        {
            DetectorChart = ReadJSONFile.Read_JSON_File("JSON_Files.user_charts.detector.json");
            SourceChart = ReadJSONFile.Read_JSON_File("JSON_Files.user_charts.source.json");
            OpticalFiberChart = ReadJSONFile.Read_JSON_File("JSON_Files.user_charts.optical_fiber.json");
            ConnectorChart = ReadJSONFile.Read_JSON_File("JSON_Files.user_charts.connector.json");

            BuildChartDictionarys(DetectorChart, DetectorDictionary);
            BuildChartDictionarys(SourceChart, SourceDictionary);
            BuildChartDictionarys(OpticalFiberChart, OpticalFiberDictionary);
            BuildChartDictionarys(ConnectorChart, ConnectorDictionary);

            ValuesDictionary = new Dictionary<values_name, Data>() {
                {values_name.SYSTEM_TYPE, new Data("System type", null)},
                {values_name.REQUIRED_BW, new Data("Required BW", null)},
                {values_name.REQUIRED_BIT_RATE, new Data("Required Bit Rate", null)},
                {values_name.REQUIRED_BER, new Data("Required BER", null)},
                {values_name.REQUIRED_SNR, new Data("Required SNR", null)},
                {values_name.TRANSMISSION_DISTANCE, new Data("Transmissionn Distance", null)},
                {values_name.NUMBER_OF_CONNECTORS, new Data("Number of connectors", null)},
                {values_name.PHOTODETECTOR_TYPE, new Data("Photodetector type", null)},
                //
                {values_name.MODULATION_CODE, new Data("Modulation code", null)},
                {values_name.ENVIRONMENT, new Data("Environment", null)},
                {values_name.RECEIVER_SENSITIVITY, new Data("Receiver sensitivity", null)},
                {values_name.FIBER_TYPE, new Data("Fiber type", null)},
                {values_name.FIBER_ATTENUATION, new Data("Fiber attenuation", null)},
                {values_name.TOTAL_FIBER_LOSS, new Data("Total fiber loss", null)},
                {values_name.SOURCE_TYPE, new Data("Source type", null)},
                {values_name.AVERAGE_SOURCE_OUTPUT, new Data("Average source output", null)},
                {values_name.SPLICE, new Data("Splice", null)},
                {values_name.NUMBER_OF_SPLICES, new Data("Number of splices", null)},
                {values_name.SPLICE_INSERTION_LOSS, new Data("Splice insertion loss", null)},
                {values_name.TOTAL_SPLICE_LOSS, new Data("Total splice loss", null)},
                {values_name.CONNECTOR_INSERTION_LOSS, new Data("Connector insertion loss", null)},
                {values_name.TOTAL_CONNECTOR_LOSS, new Data("Total connector loss", null)},
                {values_name.TIME_DEGRADATION_FACTOR, new Data("Time degradation factor", null)},
                {values_name.ENV_DEGRADATION_FACTOR, new Data("Env. Degradation factor", null)},
                {values_name.TOTAL_ATTENUATION, new Data("Total attenuation", null)},
                {values_name.TOTAL_LOSS_MARGIN, new Data("Total loss margin", null)},
                {values_name.EXCESS_POWER, new Data("Excess power", null)},
                {values_name.ACTUAL_POWER_AT_THE_RECEIVER, new Data("Actual power at the receiver", null)},
                //
                {values_name.REQUIRED_SYSTEM_RISE_TIME, new Data("Required system rise time", null)},
                {values_name.FIBER_BANDWIDTH, new Data("Fiber bandwidth", null)},
                {values_name.FIBER_RISE_TIME, new Data("Fiber rise time", null)},
                {values_name.SOURCE_RISE_TIME, new Data("Source rise time", null)},
                {values_name.DETECTOR_RISE_TIME, new Data("Detector rise time", null)},
                //
                {values_name.ACTUAL_SYSTEM_RISE_TIME, new Data("Actual system rise time", null)},
                {values_name.ACTUAL_BANDWIDTH, new Data("Actual bandwidth", null)},
                {values_name.ACTUAL_BITRATE, new Data("Actual bit rate", null)},
                {values_name.ACTUAL_SNR, new Data("Actual SNR", null)},
            };

            SystemRequirementsAnalysisValues = new List<values_name>() {
                values_name.SYSTEM_TYPE,
                values_name.REQUIRED_BW,
                values_name.REQUIRED_BIT_RATE,
                values_name.REQUIRED_BER,
                values_name.REQUIRED_SNR,
                values_name.NUMBER_OF_CONNECTORS
            };

            OpticalPowerBudgetAnalysisValues = new List<values_name>() {
                values_name.MODULATION_CODE,
                values_name.ENVIRONMENT,
                values_name.RECEIVER_SENSITIVITY,
                values_name.FIBER_TYPE,
                values_name.FIBER_ATTENUATION,
                values_name.TOTAL_FIBER_LOSS,
                values_name.SOURCE_TYPE,
                values_name.AVERAGE_SOURCE_OUTPUT,
                values_name.SPLICE,
                values_name.NUMBER_OF_SPLICES,
                values_name.SPLICE_INSERTION_LOSS,
                values_name.TOTAL_SPLICE_LOSS,
                values_name.CONNECTOR_INSERTION_LOSS,
                values_name.TOTAL_CONNECTOR_LOSS,
                values_name.TIME_DEGRADATION_FACTOR,
                values_name.ENV_DEGRADATION_FACTOR,
                values_name.TOTAL_ATTENUATION,
                values_name.TOTAL_LOSS_MARGIN,
                values_name.EXCESS_POWER,
                values_name.ACTUAL_POWER_AT_THE_RECEIVER,
            };

            SystemRiseTimeAnalysisValues = new List<values_name>() {
                values_name.REQUIRED_SYSTEM_RISE_TIME,
                values_name.FIBER_BANDWIDTH,
                values_name.FIBER_RISE_TIME,
                values_name.SOURCE_RISE_TIME,
                values_name.DETECTOR_RISE_TIME,
            };

            ActualSystemSpecificationValues = new List<values_name>() {
                values_name.ACTUAL_SYSTEM_RISE_TIME,
                values_name.ACTUAL_BANDWIDTH,
                values_name.ACTUAL_BITRATE,
                values_name.ACTUAL_SNR,
            };

            AllValues = new List<values_name>();
            AllValues.AddRange(SystemRequirementsAnalysisValues);
            AllValues.AddRange(OpticalPowerBudgetAnalysisValues);
            AllValues.AddRange(SystemRiseTimeAnalysisValues);
            AllValues.AddRange(ActualSystemSpecificationValues);
        }

        private void BuildChartDictionarys(List<List<Tuple<String, String>>> chart, Dictionary<String, List<String>> dictionary)
        {
            foreach (var kvp in chart)
            {
                foreach (var kvp2 in kvp)
                {
                    if (!dictionary.ContainsKey(kvp2.Item1))
                    {
                        dictionary[kvp2.Item1] = new List<string>();
                    }

                    dictionary[kvp2.Item1].Add(kvp2.Item2);
                }
            }
        }

        public Data GetValue(values_name name)
        {
            if (ValuesDictionary.TryGetValue(name, out var data))
            {
                return data;
            }
            else
            {
                throw new ArgumentException("Invalid value name");
            }
        }

        public void SetValue(values_name name, String data)
        {
            if (ValuesDictionary.ContainsKey(name))
            {
                ValuesDictionary[name].setValue(data);
            }
            else
            {
                throw new ArgumentException("Invalid value name");
            }
        }

        public List<String> GetDetectorListOf(String name)
        {
            return DetectorDictionary[name];
        }
        public List<String> GetSourceListOf(String name)
        {
            return SourceDictionary[name];
        }
        public List<String> GetConnectorListOf(String name)
        {
            return ConnectorDictionary[name];
        }
        public List<String> GetOpticaFiberListOf(String name)
        {
            return OpticalFiberDictionary[name];
        }

        public List<values_name> GetAllValues()
        {
            return AllValues;
        }

        public List<List<Tuple<String, String>>> GetDetectorChart()
        {
            return DetectorChart;
        }

        public List<List<Tuple<string, string>>> GetSourceChart()
        {
            return SourceChart;
        }

        public List<List<Tuple<string, string>>> GetOpticalFiberChart()
        {
            return OpticalFiberChart;
        }

        public List<List<Tuple<string, string>>> GetConnectorChart()
        {
            return ConnectorChart;
        }

        public List<String> GetColumnsNamesOf(List<List<Tuple<String, String>>> chartData)
        {
            List<String> columnsNames = new List<String>();
            foreach (var val in chartData[0])
            {
                columnsNames.Add(val.Item1);
            }
            return columnsNames;
        }

        public List<String> GetRowValuesOf(List<List<Tuple<String, String>>> chartData, int index)
        {
            List<String> rowValues = new List<String>();
            foreach (var val in chartData[index])
            {
                rowValues.Add(val.Item2);
            }
            return rowValues;
        }

        public int GetUsedDetectorIndex()
        {
            return UsedDetectorIndex;
        }
        public int GetUsedConnectorIndex()
        {
            return UsedConnectorIndex;
        }
        public int GetUsedOpticalFiberIndex()
        {
            return UsedOpticalFiberIndex;
        }
        public int GetUsedSourceIndex()
        {
            return UsedSourceIndex;
        }
        public void setUsedDetectorIndex(int indx) => UsedDetectorIndex = indx;
        public void setUsedConnectorIndex(int indx) => UsedConnectorIndex = indx;
        public void setUsedOpticalFiberIndex(int indx) => UsedOpticalFiberIndex = indx;
        public void setUsedSourceIndex(int indx) => UsedSourceIndex = indx;

        /*
                public List<int> GetUsedDetectors()
                {
                    return AvailableDetectors;
                }
                public List<int> GetUsedSources()
                {
                    return AvailableSources;
                }
                public List<int> GetUsedConnectors()
                {
                    return AvailableConnectors;
                }
                public List<int> GetUsedOpticalFibers()
                {
                    return AvailableOpticalFibers;
                }

                public void SetUsedDetectors(List<int> usedDetectors)
                {
                    this.AvailableDetectors = usedDetectors;
                }
                public void SetUsedConnectors(List<int> usedConnectors)
                {
                    this.AvailableConnectors = usedConnectors;
                }
                public void SetUsedSources(List<int> usedSources)
                {
                    this.AvailableSources = usedSources;
                }
                public void SetUsedOpticalFibers(List<int> usedOpticalFibers)
                {
                    this.AvailableOpticalFibers = usedOpticalFibers;
                }

                public void removeUsedDetector(int indx)
                {
                    AvailableDetectors.Remove(indx);
                }

                public void removeUsedSource(int indx)
                {
                    AvailableSources.Remove(indx);
                }

                public void removeUsedConnector(int indx)
                {
                    AvailableConnectors.Remove(indx);
                }

                public void removeUsedOpticalFiber(int indx)
                {
                    AvailableOpticalFibers.Remove(indx);
                }
        */

    }

}
