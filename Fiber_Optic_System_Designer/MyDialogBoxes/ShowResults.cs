using Fiber_Optic_System_Designer.ValuesAndCalculations;
using System.Data;

namespace Fiber_Optic_System_Designer.MyDialogBoxes
{
    internal class ShowResults
    {
        public static void ShowResultsDialog(string title, List<Data> results)
        {
            Size size = new Size(1200, 700);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size((size.Width / 2) - 10, size.Height - 25);
            dataGridView.Location = new Point(10, 10);
            DataTable table = new DataTable();
            table.Columns.Add("tmp1");
            table.Columns.Add("tmp2");

            foreach (var x in results)
            {
                table.Rows.Add(x.getName(), x.getValue() == null ? "--" : x.getValue());
            }

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.AllowDrop = false;
            dataGridView.DefaultCellStyle.Font = new Font(Control.DefaultFont.Name, 12);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 32;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = true;
            dataGridView.DataSource = table;
            inputBox.Controls.Add(dataGridView);

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
