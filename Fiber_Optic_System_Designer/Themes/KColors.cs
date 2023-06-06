﻿namespace Fiber_Optic_System_Designer.Themes
{
    internal class KColors
    {
        public static Color KBaseColor = ColorTranslator.FromHtml("#FFFFFF");
        public static Color K2BaseColor = ColorTranslator.FromHtml("#0C134F");
        public static Color KButtonColor = ColorTranslator.FromHtml("#3474d0");
        public static Color KButtonColor_Selected = ColorTranslator.FromHtml("#3474d0");
        public static Color KButtonColor_NotSelected = ColorTranslator.FromHtml("#0c1d4f");
        public static Color KButtonTextColor = ColorTranslator.FromHtml("#FFFFFF");

        public static Color LightenBy(Color color, int percent)
        {
            return ChangeColorBrightness(color, percent / 100.0);
        }

        public static Color DarkenBy(Color color, int percent)
        {
            return ChangeColorBrightness(color, -1 * percent / 100.0);
        }
        public static Color ChangeColorBrightness(Color color, double correctionFactor)
        {
            double red = color.R;
            double green = color.G;
            double blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = ((255 - red) * correctionFactor) + red;
                green = ((255 - green) * correctionFactor) + green;
                blue = ((255 - blue) * correctionFactor) + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }



    }
}