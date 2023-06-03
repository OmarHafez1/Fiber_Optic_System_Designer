﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiber_Optic_System_Designer.Themes
{
    class MyLeftButton
    {
        Panel button;
        Label label;
        int width;
        bool isSelected = false;
        Action deSelectAll;
        public MyLeftButton(Panel button, Label label, Action deSelectAll)
        {
            this.button = button;
            this.label = label;
            this.deSelectAll = deSelectAll;
            setLeftButtonThemes();
        }
 
        public void setLeftButtonThemes()
        {
            label.ForeColor = KColors.KButtonTextColor;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            updateColor();
            updateWidth(button.Parent.Width);
            button.Height = 65;
            PropertyChangedEventHandler pp;
            label.MouseEnter += delegate (object? sender, EventArgs e)
            {
                label.BackColor = KColors.LightenBy(label.BackColor, 10);
            };
            label.MouseLeave += delegate (object? sender, EventArgs e)
            {
                updateColor();
            };
            label.MouseClick += delegate (object? sender, MouseEventArgs e)
            {
                deSelectAll();
                select();
            };

        }
        public void deSelect()
        {
            isSelected = false;
            updateColor();
        }

        public void select()
        {
            isSelected = true;
            updateColor();
        }

        public void updateWidth(int width)
        {
            this.width = width;
            button.Width = width;
        }

        public void updateColor()
        {
            label.BackColor = isSelected ? KColors.KButtonColor_Selected : KColors.KButtonColor_NotSelected;
        }
    }
}