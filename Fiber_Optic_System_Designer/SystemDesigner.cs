using Fiber_Optic_System_Designer.MyDialogBoxes;
using Fiber_Optic_System_Designer.MyExceptions;
using Fiber_Optic_System_Designer.Themes;
using Fiber_Optic_System_Designer.ValuesAndCalculations;
using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;

namespace Fiber_Optic_System_Designer
{
    // WHEN ADD THE DIGITAL AND ANALOG FEATHUER DON'T FORGET TO MAKE THE BIT RATE AND BAND WIDTD BE THE SAME IN THE SYSTEM DATA VALUES!!
    public partial class SystemDesignPanel : UserControl
    {
        List<Tuple<values_name, TextBox, Label>> FormDataInputsCompination;

        List<Tuple<values_name, dynamic>> SystemRequirements;
        DesignSystem designSystem;
        SystemData systemData;
        public SystemDesignPanel()
        {
            InitializeComponent();

            FormDataInputsCompination = new List<Tuple<values_name, TextBox, Label>>() {
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_BIT_RATE, textBox2, notValid2),
                new Tuple<values_name, TextBox, Label> (values_name.TRANSMISSION_DISTANCE, textBox3, notValid3),
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_BER, textBox4, notValid4),
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_SNR, textBox5, notValid5),
            };

            MyButton designButton = new MyButton(designButtonPanel, DesignButtonLabel,
                delegate ()
                {
                    SystemRequirements = new List<Tuple<values_name, dynamic>>();
                    systemData = new SystemData();
                    if (isDataValid())
                    {
                        bool exceptionCaught = false;
                        try
                        {
                            designSystem = new DesignSystem(SystemRequirements, systemData);
                        }
                        catch (CantDesignTheSystemException ex)
                        {
                            exceptionCaught = true;
                            //MessageBox.Show(ex.Message, "We were unable to design your system.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            DesignAndShow(false);
                        }
                        catch (CantFindSuitableComponentsException ex)
                        {
                            exceptionCaught = true;
                            MessageBox.Show(ex.Message, "We were unable to design your system.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        catch (Exception ex)
                        {
                            exceptionCaught = true;
                            MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        if (!exceptionCaught)
                        {
                            DesignAndShow(true);
                        }
                    }
                }
            );
            new AddButtonTheme(designButton);
        }

        private void DesignAndShow(bool accepted)
        {
            List<List<Tuple<string, List<string>>>> values = new List<List<Tuple<string, List<string>>>>
                            {
                                new List<Tuple<string,  List<string>>>(),
                                new List<Tuple<string,  List<string>>>(),
                                new List<Tuple<string,  List<string>>>(),
                                new List<Tuple<string,  List<string>>>()
                            };

            foreach (var entry in systemData.GetDetectorDictionary())
            {
                values[0].Add(new Tuple<string, List<string>>(entry.Key, new List<string> { entry.Value[systemData.GetUsedDetectorIndex()] }));
            }

            foreach (var entry in systemData.GetConnectorDictionary())
            {
                List<string> lst = new List<string>();
                foreach (var connector in systemData.GetUsedConnectors())
                {
                    lst.Add(entry.Value[connector.Item1]);
                }
                values[1].Add(new Tuple<string, List<string>>(entry.Key, lst));
            }

            List<string> NumberOfUsedConnector = new List<string>();
            foreach (var entry in systemData.GetUsedConnectors())
            {
                NumberOfUsedConnector.Add(entry.Item2.ToString());
            }
            values[1].Add(new Tuple<string, List<string>>("Used connectors.", NumberOfUsedConnector));

            foreach (var entry in systemData.GetOpticalFiberDictionary())
            {
                values[2].Add(new Tuple<string, List<string>>(entry.Key, new List<string> { entry.Value[systemData.GetUsedOpticalFiberIndex()] }));
            }

            foreach (var entry in systemData.GetSourceDictionary())
            {
                values[3].Add(new Tuple<string, List<string>>(entry.Key, new List<string> { entry.Value[systemData.GetUsedSourceIndex()] }));
            }

            List<string> tablesNames = new() { "detector", "connector", "optical fiber", "source" };

            string finalAnalysis = "";
            finalAnalysis += $"Power at receiver = {systemData.GetValue(values_name.ACTUAL_POWER_AT_THE_RECEIVER):0.0000}{systemData.GetData(values_name.ACTUAL_POWER_AT_THE_RECEIVER).getUnit()}" + (accepted ? " >= " : " < ") +
                             $"receiver sensitivity {systemData.GetValue(values_name.RECEIVER_SENSITIVITY):0.0000}{systemData.GetData(values_name.RECEIVER_SENSITIVITY).getUnit()}  " + (accepted ? "\u2714\n" : "\u2718\n");
            finalAnalysis += $"Actual bit rate = {systemData.GetValue(values_name.ACTUAL_BITRATE):0.0000}{systemData.GetData(values_name.ACTUAL_BITRATE).getUnit()}" + (accepted ? " >= " : " < ") +
                             $"required bit rate {systemData.GetValue(values_name.REQUIRED_BIT_RATE):0.0000}{systemData.GetData(values_name.REQUIRED_BIT_RATE).getUnit()}  " + (accepted ? "\u2714\n" : "\u2718\n");
            finalAnalysis += $"Actual system rise time = {systemData.GetValue(values_name.ACTUAL_SYSTEM_RISE_TIME):0.0000}{systemData.GetData(values_name.ACTUAL_SYSTEM_RISE_TIME).getUnit()}" + (accepted ? " <= " : " > ") +
                             $"required system rise time {systemData.GetValue(values_name.REQUIRED_SYSTEM_RISE_TIME):0.0000}{systemData.GetData(values_name.REQUIRED_SYSTEM_RISE_TIME).getUnit()}  " + (accepted ? "\u2714\n" : "\u2718\n");
            finalAnalysis += $"Actual BER = {SNR_BER_Conversion.ConverSNRTOBER(systemData.GetValue(values_name.ACTUAL_SNR))}{systemData.GetData(values_name.ACTUAL_SNR).getUnit()}" + (accepted ? " <= " : " > ") +
                             $"required BER {systemData.GetValue(values_name.REQUIRED_BER)}{systemData.GetData(values_name.REQUIRED_BER).getUnit()}  " + (accepted ? "\u2714\n" : "\u2718\n");
            // Get all data may be bugy ! we will need to rewrite it in the systyemData class
            ShowResults.ShowResultsDialog("Results", systemData.GetAllData(), values, tablesNames, finalAnalysis, accepted);
        }

        bool isDataValid()
        {
            bool isOk = true;
            double val;
            foreach (Tuple<values_name, TextBox, Label> input in FormDataInputsCompination)
            {
                input.Item3.Visible = false;
                if (double.TryParse(input.Item2.Text, out val))
                {
                    SystemRequirements.Add(new Tuple<values_name, dynamic>(input.Item1, val));
                }
                else
                {
                    input.Item3.Visible = true;
                    isOk = false;
                }
            }
            return isOk;
        }

        public void SetSystemRequirements(SystemData values)
        {
            string userInput = textBox2.Text;
            if (double.TryParse(userInput, out _))
            {
                textBox3.Text = userInput;
            }
            else
            {
                textBox4.Text = userInput;
            }
        }

    }
}
