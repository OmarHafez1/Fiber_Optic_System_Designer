using Fiber_Optic_System_Designer.ReadJSON;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations.Values
{
    public class SystemData
    {

        private int DataPrecision = 15;

        private Dictionary<values_name, Data> ValuesDictionary;
        private List<values_name> SystemRequirementsAnalysisValues;
        private List<values_name> OpticalPowerBudgetAnalysisValues;
        private List<values_name> SystemRiseTimeAnalysisValues;
        private List<values_name> ActualSystemSpecificationValues;
        private List<values_name> AllValuesNames;
        private List<Data> AllData;

        private List<List<Tuple<string, string>>> DetectorChart;
        private List<List<Tuple<string, string>>> SourceChart;
        private List<List<Tuple<string, string>>> OpticalFiberChart;
        private List<List<Tuple<string, string>>> ConnectorChart;

        private Dictionary<string, List<string>> DetectorDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> SourceDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> OpticalFiberDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> ConnectorDictionary = new Dictionary<string, List<string>>();

        private int UsedDetectorIndex = -1, UsedSourceIndex = -1, UsedOpticalFiberIndex = -1;
        private List<Tuple<int, int>> UsedConnectors;

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
                {values_name.SYSTEM_TYPE, new Data("System type", "Digital")},
                {values_name.REQUIRED_BW, new Data("Required BW")},
                {values_name.REQUIRED_BIT_RATE, new Data("Required Bit Rate", "Mbps")},
                {values_name.REQUIRED_BER, new Data("Required BER")},
                {values_name.REQUIRED_SNR, new Data("Required SNR", "dB")},
                {values_name.TRANSMISSION_DISTANCE, new Data("Transmissionn Distance", "Km")},
                {values_name.NUMBER_OF_CONNECTORS, new Data("Number of connectors")},
                {values_name.PHOTODETECTOR_TYPE, new Data("Photodetector type")},
                {values_name.NOISE_FACTOR, new Data()},
                {values_name.PHOTODETECTOR_RESPONSITIVITY, new Data()},
                {values_name.DISPERSION, new Data()},
                //
                {values_name.MODULATION_CODE, new Data("Modulation code")},
                {values_name.ENVIRONMENT, new Data("Environment")},
                {values_name.RECEIVER_SENSITIVITY, new Data("Receiver sensitivity", "dBm")},
                {values_name.FIBER_TYPE, new Data("Fiber type")},
                {values_name.FIBER_ATTENUATION, new Data("Fiber attenuation", "dBm")},
                {values_name.TOTAL_FIBER_LOSS, new Data("Total fiber loss", "dBm")},
                {values_name.SOURCE_TYPE, new Data("Source type")},
                {values_name.AVERAGE_SOURCE_OUTPUT, new Data("Average source output", "dBm")},
                {values_name.SPLICE, new Data("Splice")},
                {values_name.NUMBER_OF_SPLICES, new Data("Number of splices")},
                {values_name.SPLICE_INSERTION_LOSS, new Data("Splice insertion loss", "dB/splice")},
                {values_name.TOTAL_SPLICE_LOSS, new Data("Total splice loss", "dB")},
               // {values_name.CONNECTOR_INSERTION_LOSS, new Data("Connector insertion loss", "dB/connector")},
                {values_name.TOTAL_CONNECTOR_LOSS, new Data("Total connector loss", "dB")},
                {values_name.TIME_DEGRADATION_FACTOR, new Data("Time degradation factor", "dB")},
                {values_name.ENV_DEGRADATION_FACTOR, new Data("Env. Degradation factor", "dB")},
                {values_name.TOTAL_ATTENUATION, new Data("Total attenuation", "dB")},
                {values_name.TOTAL_LOSS_MARGIN, new Data("Total loss margin", "dBm")},
                {values_name.EXCESS_POWER, new Data("Excess power", "dBm")},
                {values_name.ACTUAL_POWER_AT_THE_RECEIVER, new Data("Actual power at the receiver", "dBm")},
                //
                {values_name.REQUIRED_SYSTEM_RISE_TIME, new Data("Required system rise time", "ns")},
                {values_name.FIBER_BANDWIDTH, new Data("Fiber bandwidth", "MHz")},
                {values_name.FIBER_RISE_TIME, new Data("Fiber rise time", "ns")},
                {values_name.SOURCE_RISE_TIME, new Data("Source rise time", "ns")},
                {values_name.DETECTOR_RISE_TIME, new Data("Detector rise time", "ns")},
                //
                {values_name.ACTUAL_SYSTEM_RISE_TIME, new Data("Actual system rise time", "ns")},
                {values_name.ACTUAL_BANDWIDTH, new Data("Actual bandwidth")},
                {values_name.ACTUAL_BITRATE, new Data("Actual bit rate", "Mbps")},
                {values_name.ACTUAL_SNR, new Data("Actual SNR", "dB")},
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
              //  values_name.CONNECTOR_INSERTION_LOSS,
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

            AllValuesNames = new List<values_name>();
            AllValuesNames.AddRange(SystemRequirementsAnalysisValues);
            AllValuesNames.AddRange(OpticalPowerBudgetAnalysisValues);
            AllValuesNames.AddRange(SystemRiseTimeAnalysisValues);
            AllValuesNames.AddRange(ActualSystemSpecificationValues);
        }

        private void BuildChartDictionarys(List<List<Tuple<string, string>>> chart, Dictionary<string, List<string>> dictionary)
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

        public void SetDataPrecision(int DataPrecision)
        {
            this.DataPrecision = Math.Max(Math.Min(DataPrecision, 9), 0);
        }

        public Data GetData(values_name name)
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

        public dynamic GetValue(values_name name)
        {
            if (ValuesDictionary.TryGetValue(name, out var data))
            {
                if (data.getValue() != null)
                    if (double.TryParse(data.getValue().ToString(), out double val))
                    {
                        return val;
                    }
                return data.getValue();
            }
            else
            {
                throw new ArgumentException("Invalid value name");
            }

        }
        public void SetValue(values_name name, dynamic data)
        {
            if (ValuesDictionary.ContainsKey(name))
            {
                if (double.TryParse(data.ToString(), out double val))
                {
                    ValuesDictionary[name].setValue(val);
                }
                else
                {
                    ValuesDictionary[name].setValue(data);
                }
            }
            else
            {
                throw new ArgumentException("Invalid value name");
            }
        }

        public List<string> GetDetectorListOf(string name)
        {
            return DetectorDictionary[name];
        }
        public List<string> GetSourceListOf(string name)
        {
            return SourceDictionary[name];
        }
        public List<string> GetConnectorListOf(string name)
        {
            return ConnectorDictionary[name];
        }
        public List<string> GetOpticaFiberListOf(string name)
        {
            return OpticalFiberDictionary[name];
        }

        public List<values_name> GetAllValuesNames()
        {
            return AllValuesNames;
        }

        public List<Data> GetAllData()
        {
            AllData = new List<Data> { };
            foreach (var x in GetAllValuesNames())
            {
                AllData.Add(GetData(x));
            }
            return AllData;
        }

        public List<List<Tuple<string, string>>> GetDetectorChart()
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

        public List<string> GetColumnsNamesOf(List<List<Tuple<string, string>>> chartData)
        {
            List<string> columnsNames = new List<string>();
            foreach (var val in chartData[0])
            {
                columnsNames.Add(val.Item1);
            }
            return columnsNames;
        }

        public List<string> GetRowValuesOf(List<List<Tuple<string, string>>> chartData, int index)
        {
            List<string> rowValues = new List<string>();
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
        public List<Tuple<int, int>> GetUsedConnectors()
        {
            return UsedConnectors;
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
        public void setUsedConnectorIndex(List<Tuple<int, int>> vals) => UsedConnectors = vals;
        public void setUsedOpticalFiberIndex(int indx) => UsedOpticalFiberIndex = indx;
        public void setUsedSourceIndex(int indx) => UsedSourceIndex = indx;

        public Dictionary<string, List<string>> GetDetectorDictionary()
        {
            return DetectorDictionary;
        }

        public Dictionary<string, List<string>> GetSourceDictionary()
        {
            return SourceDictionary;
        }

        public Dictionary<string, List<string>> GetOpticalFiberDictionary()
        {
            return OpticalFiberDictionary;
        }

        public Dictionary<string, List<string>> GetConnectorDictionary()
        {
            return ConnectorDictionary;
        }

    }

}
