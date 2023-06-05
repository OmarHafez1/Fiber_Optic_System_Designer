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

        List<Tuple<values_name, String>> SystemRequirements;

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
                    DesignSystem designSystem;
                    SystemRequirements = new List<Tuple<values_name, String>>();
                    if (isDataValid())
                    {
                        designSystem = new DesignSystem(SystemRequirements);
                        SystemData systemData = designSystem.getSystemData();
                        Calculations calc = new Calculations(designSystem.getSystemData());
                        string result = systemData.GetValue(values_name.ENVIRONMENT).getValue() + "\n";
                        calc.GetEnvironment();
                        result += systemData.GetValue(values_name.ENVIRONMENT).getValue() + "\n";
                        foreach (values_name v in systemData.GetAllValues())
                        {
                            result += systemData.GetValue(v) + "\n\n\n  ||  ";
                        }
                        ShowResults.ShowResultsDialog("test", result);
                    }
                }
            );
            new AddButtonTheme(designButton);
        }

        bool isDataValid()
        {
            bool isOk = true;
            float val;
            foreach (Tuple<values_name, TextBox, Label> input in FormDataInputsCompination)
            {
                input.Item3.Visible = false;
                if (float.TryParse(input.Item2.Text, out val))
                {
                    SystemRequirements.Add(new Tuple<values_name, String>(input.Item1, input.Item2.Text));
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

            float floatValue;
            if (float.TryParse(userInput, out floatValue))
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


/*
 
 
System.ArgumentException: Invalid value name
   at Fiber_Optic_System_Designer.ValuesAndCalculations.Values.SystemData.SetValue(values_name name, String data) in O:\FIber_Optic_System_Designer\Fiber_Optic_System_Designer\ValuesAndCalculations\Values\SystemData.cs:line 182
   at Fiber_Optic_System_Designer.ValuesAndCalculations.DesignSystem.initializeSystemRequirements(List`1 SystemRequirements) in O:\FIber_Optic_System_Designer\Fiber_Optic_System_Designer\ValuesAndCalculations\DesignSystem.cs:line 19
   at Fiber_Optic_System_Designer.ValuesAndCalculations.DesignSystem..ctor(List`1 SystemRequirements) in O:\FIber_Optic_System_Designer\Fiber_Optic_System_Designer\ValuesAndCalculations\DesignSystem.cs:line 11
   at Fiber_Optic_System_Designer.SystemDesigner.<.ctor>b__2_0() in O:\FIber_Optic_System_Designer\Fiber_Optic_System_Designer\SystemDesigner.cs:line 36
   at Fiber_Optic_System_Designer.Themes.MyButton.<>c__DisplayClass3_0.<.ctor>b__0(Object sender, MouseEventArgs e) in O:\FIber_Optic_System_Designer\Fiber_Optic_System_Designer\Themes\MyButton.cs:line 15
   at System.Windows.Forms.Control.OnMouseClick(MouseEventArgs e)
   at System.Windows.Forms.Control.WmMouseUp(Message& m, MouseButtons button, Int32 clicks)
   at System.Windows.Forms.Control.WndProc(Message& m)
   at System.Windows.Forms.Label.WndProc(Message& m)
   at System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
   at System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, WM msg, IntPtr wparam, IntPtr lparam)

 
 */