namespace Fiber_Optic_System_Designer
{
    internal class SNR_BER_Conversion
    {
        public static double ConverSNRTOBER(double snr)
        {
            return Math.Pow(1e-9, snr / 21.2);
        }
    }
}
