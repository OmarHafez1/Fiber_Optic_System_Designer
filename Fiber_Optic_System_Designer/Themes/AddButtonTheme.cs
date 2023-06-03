using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiber_Optic_System_Designer.Themes;

namespace Fiber_Optic_System_Designer.Themes
{
    class AddButtonTheme
    {
        MyButton button;
        
        public AddButtonTheme(MyButton button) { 
            this.button = button;
            button.button_label.TextAlign = ContentAlignment.MiddleCenter;
            button.button_label.ForeColor = KColors.KButtonTextColor;
            button.button_label.AutoSize = false;
            button.button_label.TextAlign = ContentAlignment.MiddleCenter;
            button.button_label.Dock = DockStyle.Fill;
            button.button_label.Padding = new Padding(10, 0, 0, 0);

            updateColor();
            button.button_label.MouseEnter += delegate (object? sender, EventArgs e)
            {
                button.button_label.BackColor = KColors.LightenBy(button.button_label.BackColor, 10);
            };
            button.button_label.MouseLeave += delegate (object? sender, EventArgs e)
            {
                updateColor();
            };
        }

        void updateColor()
        {
            button.button_label.BackColor = KColors.KButtonColor;
        }



    }
}
