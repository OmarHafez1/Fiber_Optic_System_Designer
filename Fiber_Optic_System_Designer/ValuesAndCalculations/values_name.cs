﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    public enum values_name
    {
        SYSTEM_TYPE,
        REQUIRED_BW,
        REQUIRED_BIT_RATE,
        REQUIRED_BER,
        REQUIRED_SNR,
        NUMBER_OF_CONNECTORS,
        //
        MODULATION_CODE,
        ENVIRONMENT,
        RECEIVER_SENSITIVITY,
        DETECTOR_TYPE,
        FIBER_TYPE,
        FIBER_ATTENUATION,
        TOTAL_FIBER_LOSS,
        SOURCE_TYPE,
        AVERAGE_SOURCE_OUTPUT,
        SPLICE,
        NUMBER_OF_SPLICES,
        SPLICE_INSERTION_LOSS,
        TOTAL_SPLICE_LOSS,
        TYPE_OF_CONNECTORS,
        CONNECTOR_INSERTION_LOSS,
        TOTAL_CONNECTOR_LOSS,
        TIME_DEGRADATION_FACTOR,
        ENV_DEGRADATION_FACTOR,
        TOTAL_ATTENUATION,
        TOTAL_LOSS_MARGIN,
        EXCESS_POWER,
        ACTUAL_POWER_AT_THE_RECEIVER,
        //
        REQUIRED_SYSTEM_RISE_TIME,
        FIBER_BANDWIDTH,
        FIBER_RISE_TIME,
        SOURCE_RISE_TIME,
        DETECTOR_RISE_TIME,
        //
        ACTUAL_SYSTEM_RISE_TIME,
        ACTUAL_BANDWIDTH,
        ACTUAL_BITRATE,
        ACTUAL_SNR,
    }
}