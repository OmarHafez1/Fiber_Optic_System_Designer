using Fiber_Optic_System_Designer.MyDialogBoxes;
using Fiber_Optic_System_Designer.Themes;
using Fiber_Optic_System_Designer.ValuesAndCalculations;
using Fiber_Optic_System_Designer.ValuesAndCalculations.Values;

namespace Fiber_Optic_System_Designer
{
    // WHEN ADD THE DIGITAL AND ANALOG FEATHUER DON'T FORGET TO MAKE THE BIT RATE AND BAND WIDTD BE THE SAME IN THE SYSTEM DATA VALUES!!
    public partial class SystemDesigner : UserControl
    {
        List<Tuple<values_name, TextBox, Label>> FormDataInputsCompination;

        List<Tuple<values_name, dynamic>> SystemRequirements;
        DesignSystem designSystem;
        SystemData systemData;
        public SystemDesigner()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;

            FormDataInputsCompination = new List<Tuple<values_name, TextBox, Label>>() {
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_BIT_RATE, textBox2, notValid2),
                new Tuple<values_name, TextBox, Label> (values_name.TRANSMISSION_DISTANCE, textBox3, notValid3),
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_BER, textBox4, notValid4),
                new Tuple<values_name, TextBox, Label> (values_name.REQUIRED_SNR, textBox5, notValid5),
                new Tuple<values_name, TextBox, Label> (values_name.NUMBER_OF_CONNECTORS, textBox6, notValid6),
            };

            MyButton designButton = new MyButton(designButtonPanel, DesignButtonLabel,
                delegate ()
                {
                    SystemRequirements = new List<Tuple<values_name, dynamic>>();
                    if (isDataValid())
                    {
                        systemData = new SystemData();
                        designSystem = new DesignSystem(SystemRequirements, systemData);
                        ShowResults.ShowResultsDialog("Results", systemData.GetAllData());
                    }
                }
            );
            new AddButtonTheme(designButton);
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
