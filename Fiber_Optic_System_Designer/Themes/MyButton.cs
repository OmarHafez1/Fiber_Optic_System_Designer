namespace Fiber_Optic_System_Designer.Themes
{
    internal class MyButton
    {
        public Panel button_panel;
        public Label button_label;
        public Action action;
        public MyButton(Panel button, Label label, Action action)
        {
            this.button_panel = button;
            this.button_label = label;
            this.action = action;
            button_label.MouseClick += delegate (object? sender, MouseEventArgs e)
            {
                action();
            };
        }

    }
}
