namespace Fiber_Optic_System_Designer.MyDialogBoxes
{
    internal class ShowResults
    {
        public static void ShowResultsDialog(string title, string message)
        {
            Size size = new Size(1200, 600);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, size.Height - 25);
            textBox.Text = message;
            textBox.Multiline = true;
            inputBox.Controls.Add(textBox);

            //

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 90, size.Height - 37);
            inputBox.Controls.Add(okButton);

            inputBox.AcceptButton = okButton;

            DialogResult result = inputBox.ShowDialog();

        }

    }
}
