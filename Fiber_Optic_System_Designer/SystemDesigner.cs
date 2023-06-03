using Fiber_Optic_System_Designer.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fiber_Optic_System_Designer
{
    public partial class SystemDesigner : UserControl
    {
        public SystemDesigner()
        {
            InitializeComponent();

            MyButton designButton = new MyButton(designButtonPanel, DesignButtonLabel, DesignSystem);
            new AddButtonTheme(designButton);

        }

        public void DesignSystem()
        {

        }
    }

    /*    private void TxtID_TextChanged(object sender, EventArgs e)
        {
            Control ctrl = (sender as Control);

            string value = string.Concat(ctrl
              .Text
              .Where(c => c >= '0' && c <= '9'));

            if (value != ctrl.Text)
                ctrl.Text = value;
    */
}
