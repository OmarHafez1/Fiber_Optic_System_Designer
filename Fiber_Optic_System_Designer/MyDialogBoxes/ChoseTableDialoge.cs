using System.Data;

namespace Fiber_Optic_System_Designer.MyDialogBoxes
{
    internal class ChoseTableDialoge
    {
        private static DataGridView dataGridView;
        private static List<int> ConnectorsCount;

        public static List<int> getConnectosCount() { return ConnectorsCount; }

        public static int InputDialog(string title, string message, List<List<string>> values, List<string> Pmin = null, bool isConnector = false)
        {
            Size size = new Size(1070, 450);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.MaximizeBox = false;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            //

            Label label = new Label();
            label.Location = new Point(10, 10);
            label.Text = message;
            label.Size = new Size(size.Width - 10, 25);
            label.Font = new Font(Control.DefaultFont.Name, 11, FontStyle.Bold);
            inputBox.Controls.Add(label);

            //

            dataGridView = new DataGridView();
            dataGridView.Size = new Size(size.Width - 20, size.Height - 90);
            dataGridView.Location = new Point(10, label.Height + 25);

            DataTable table = new DataTable();

            foreach (string value in values[0])
            {
                table.Columns.Add(value.ToString());
            }

            if (Pmin != null)
            {
                table.Columns.Add("Required Pmin");
            }

            if (isConnector)
            {
                table.Columns.Add("Number of connectors");
            }

            for (int i = 1; i < values.Count; i++)
            {
                if (Pmin != null)
                {
                    values[i].Add(Pmin[i - 1]);
                }
                if (isConnector)
                {
                    values[i].Add("0");
                }
                table.Rows.Add(values[i].ToArray());
            }

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;

            if (!isConnector)
            {
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.AllowDrop = false;
            dataGridView.DefaultCellStyle.Font = new Font(Control.DefaultFont.Name, 12);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(Control.DefaultFont.Name, 8);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 32;
            dataGridView.ColumnHeadersHeight = dataGridView.RowTemplate.Height + 15;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = !isConnector;
            dataGridView.DataSource = table;

            dataGridView.ColumnAdded += delegate (object? sender, DataGridViewColumnEventArgs e)
            {
                dataGridView.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (isConnector && dataGridView.ColumnCount != table.Columns.Count)
                {
                    dataGridView.Columns[dataGridView.ColumnCount - 1].ReadOnly = true;
                }
                if (Pmin != null && dataGridView.ColumnCount == table.Columns.Count)
                {
                    dataGridView.Columns[dataGridView.ColumnCount - 1].DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Bold);
                }
            };
            int rowIndex = 0;
            dataGridView.RowsAdded += delegate (object? sender, DataGridViewRowsAddedEventArgs e)
            {
                if (isConnector)
                {
                    dataGridView.Rows[0].Cells[dataGridView.ColumnCount - 1].Selected = true;
                }
            };

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            inputBox.Controls.Add(dataGridView);

            //

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 90, size.Height - 37);

            if (isConnector)
            {
                okButton.MouseClick += delegate (object? sender, MouseEventArgs e)
                {
                    bool isAllZeros = true;
                    ConnectorsCount = new List<int>();
                    foreach (dynamic row in dataGridView.Rows)
                    {
                        ConnectorsCount.Add(int.Parse(row.Cells[dataGridView.ColumnCount - 1].Value));
                        if (ConnectorsCount.Last() != 0)
                        {
                            isAllZeros = false;
                        }
                    }
                    if (isAllZeros)
                    {

                    }
                };
            }

            inputBox.Controls.Add(okButton);

            inputBox.AcceptButton = okButton;

            DialogResult result = inputBox.ShowDialog();

            return dataGridView.CurrentCell.RowIndex;
        }
    }
}
