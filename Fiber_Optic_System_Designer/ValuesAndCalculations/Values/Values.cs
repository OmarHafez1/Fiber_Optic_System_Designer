using Fiber_Optic_System_Designer.ValuesAndCalculations.Values.TMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations.Values
{
    internal class Values
    {
        Dictionary<values_name, Data> getData;
        List<values_name> SystemRequirementsAnalysisValues;
        List<values_name> OpticalPowerBudgetAnalysisValues;
        List<values_name> SystemRiseTimeAnalysisValues;
        List<values_name> ActualSystemSpecificationValues;
        List<values_name> AllValues = new List<values_name>();

        public Values()
        {

            getData = new Dictionary<values_name, Data>() {
                {values_name.SYSTEM_TYPE, new Data("System type", null)},
                {values_name.REQUIRED_BW, new Data("Required BW", null)},
                {values_name.REQUIRED_BIT_RATE, new Data("Required Bit Rate", null)},
                {values_name.REQUIRED_BER, new Data("Required BER", null)},
                {values_name.REQUIRED_SNR, new Data("Required SNR", null)},
                {values_name.NUMBER_OF_CONNECTORS, new Data("Number of connectors", null)},
                //
                {values_name.MODULATION_CODE, new Data("Modulation code", null)},
                {values_name.ENVIRONMENT, new Data("Environment", null)},
                {values_name.RECEIVER_SENSITIVITY, new Data("Receiver sensitivity", null)},
                {values_name.DETECTOR_TYPE, new Data("Detector type", null)},
                {values_name.FIBER_TYPE, new Data("Fiber type", null)},
                {values_name.FIBER_ATTENUATION, new Data("Fiber attenuation", null)},
                {values_name.TOTAL_FIBER_LOSS, new Data("Total fiber loss", null)},
                {values_name.SOURCE_TYPE, new Data("Source type", null)},
                {values_name.AVERAGE_SOURCE_OUTPUT, new Data("Average source output", null)},
                {values_name.SPLICE, new Data("Splice", null)},
                {values_name.NUMBER_OF_SPLICES, new Data("Number of splices", null)},
                {values_name.SPLICE_INSERTION_LOSS, new Data("Splice insertion loss", null)},
                {values_name.TOTAL_SPLICE_LOSS, new Data("Total splice loss", null)},
                {values_name.TYPE_OF_CONNECTORS, new Data("Type of connectors", null)},
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
                values_name.DETECTOR_TYPE,
                values_name.FIBER_TYPE,
                values_name.FIBER_ATTENUATION,
                values_name.TOTAL_FIBER_LOSS,
                values_name.SOURCE_TYPE,
                values_name.AVERAGE_SOURCE_OUTPUT,
                values_name.SPLICE,
                values_name.NUMBER_OF_SPLICES,
                values_name.SPLICE_INSERTION_LOSS,
                values_name.TOTAL_SPLICE_LOSS,
                values_name.TYPE_OF_CONNECTORS,
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

            foreach(values_name val in SystemRequirementsAnalysisValues)
            {
                AllValues.Add(val);
            }

            foreach(values_name val in OpticalPowerBudgetAnalysisValues)
            {
                AllValues.Add(val);
            }

            foreach(values_name val in SystemRiseTimeAnalysisValues)
            {
                AllValues.Add(val);
            }

            foreach(values_name val in  ActualSystemSpecificationValues)
            {
                AllValues.Add(val);
            }
        }
    }
}
