using Fiber_Optic_System_Designer.ValuesAndCalculations;
using System.Data;

namespace Fiber_Optic_System_Designer.MyDialogBoxes
{
    internal class ShowResults
    {
        static Size size;
        public static void ShowResultsDialog(string title, List<Data> results, List<List<Tuple<string, string>>> values, List<string> tablesNames, string finalAnalysis)
        {
            size = new Size(1200, 700);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            //

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size((size.Width / 2) - 80, size.Height - 25);
            dataGridView.Location = new Point(10, 10);
            DataTable table = new DataTable();
            table.Columns.Add("tmp1");
            table.Columns.Add("tmp2");

            foreach (var x in results)
            {
                string tmp = "--";
                if (x.getValue() != null)
                {
                    if (x.getValue() is double)
                    {
                        tmp = String.Format("{0:0.0000}", x.getValue());
                    }
                    else
                    {
                        tmp = x.getValue();
                    }
                }
                table.Rows.Add(x.getName(), tmp);
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

            int usedHeight = 10;

            for (int i = 0; i < values.Count; i++)
            {
                Label label = new Label();
                label.Location = new Point((size.Width / 2) - 55, usedHeight);
                label.Text = $"You will need to use this {tablesNames[i]}:";
                label.Size = new Size(size.Width - 10, 20);
                usedHeight += 20;
                label.Font = new Font(Control.DefaultFont.Name, 11, FontStyle.Bold);
                inputBox.Controls.Add(label);

                DataGridView dataGridView1 = GenerateRightSizeGridTable(values[i]);
                dataGridView1.Location = new Point((size.Width / 2) - 55, usedHeight);
                dataGridView1.Size = new Size((size.Width / 2) + 40, 62);

                usedHeight += 62 + 25;

                inputBox.Controls.Add(dataGridView1);
            }

            //

            usedHeight += 15;
            Label label1 = new Label();
            label1.Location = new Point((size.Width / 2) - 35, usedHeight);
            label1.Size = new Size((size.Width / 2) - 10, 40);
            usedHeight += 40;
            label1.Text = "Final Analysis";
            label1.Font = new Font("Arial", 25, FontStyle.Bold);
            label1.ForeColor = ColorTranslator.FromHtml("#0078D7");
            label1.TextAlign = ContentAlignment.MiddleCenter;
            inputBox.Controls.Add(label1);
            usedHeight += 16;

            Label label2 = new Label();
            label2.Location = new Point((size.Width / 2) - 55, usedHeight);
            label2.Size = new Size((size.Width / 2) + 50, 120);
            label2.Text = finalAnalysis;
            label2.Font = new Font("Arial", 13);
            label2.AutoSize = false;
            label2.AutoEllipsis = true;
            label2.TextAlign = ContentAlignment.TopLeft;
            inputBox.Controls.Add(label2);

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

        private static DataGridView GenerateRightSizeGridTable(List<Tuple<string, string>> values)
        {
            DataGridView dataGridView = new DataGridView();
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#0078D7");
            dataGridView.DefaultCellStyle.ForeColor = Color.White;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowTemplate.Height = 32;
            dataGridView.ColumnHeadersHeight = dataGridView.RowTemplate.Height;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(Control.DefaultFont.Name, 8);
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowDrop = false;
            dataGridView.DefaultCellStyle.Font = new Font(Control.DefaultFont.Name, 12);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ReadOnly = true;

            DataTable table = new DataTable();

            List<string> TMP = new();

            foreach (var x in values)
            {
                table.Columns.Add(x.Item1);
                TMP.Add(x.Item2);
            }
            table.Rows.Add(TMP.ToArray());
            dataGridView.DataSource = table;
            return dataGridView;
        }

    }
}
