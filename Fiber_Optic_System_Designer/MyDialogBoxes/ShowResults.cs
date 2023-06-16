using Fiber_Optic_System_Designer.ValuesAndCalculations;
using Microsoft.Office.Interop.Excel;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Font = System.Drawing.Font;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;

namespace Fiber_Optic_System_Designer.MyDialogBoxes
{
    internal class ShowResults
    {
        static Size size;
        public static void ShowResultsDialog(string title, List<Data> results, List<List<Tuple<string, List<string>>>> values, List<string> tablesNames, string finalAnalysis, bool accepted)
        {
            size = new Size(1290, 700);
            Form inputBox = new Form();

            inputBox.MaximizeBox = false;

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            //

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size((size.Width / 2) - 70, size.Height - 25);
            dataGridView.Location = new Point(10, 10);
            DataTable table = new DataTable();
            table.Columns.Add("tmp1");
            table.Columns.Add("tmp2");

            foreach (var x in results)
            {
                if (x.getName() == null) continue;
                string tmp = "--";
                if (x.getValue() != null)
                {
                    if (x.getValue() is double)
                    {
                        tmp = String.Format("{0:0.0000}", x.getValue()) + "  " + x.getUnit();
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
            dataGridView.ColumnHeadersHeight = dataGridView.RowTemplate.Height + 15;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = true;
            dataGridView.DataSource = table;

            dataGridView.ColumnAdded += delegate (object? sender, DataGridViewColumnEventArgs e)
            {
                dataGridView.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
            };

            inputBox.Controls.Add(dataGridView);

            //


            Panel panel = new Panel();
            panel.Size = new Size((size.Width / 2) + 80, size.Height - 40);
            panel.Location = new Point((size.Width / 2) - 45, 0);
            panel.AutoScroll = true;
            inputBox.Controls.Add(panel);

            // 


            int usedHeight = 10;
            int usedWidth = (size.Width / 2) + 25;
            for (int i = 0; i < values.Count; i++)
            {
                Label label = new Label();
                label.Location = new Point(0, usedHeight);
                label.Text = $"You will need to use these {tablesNames[i]}s:";
                label.Size = new Size(usedWidth, 20);
                usedHeight += 20;
                label.Font = new Font(Control.DefaultFont.Name, 11, FontStyle.Bold);
                panel.Controls.Add(label);

                DataGridView dataGridView1 = GenerateRightSizeGridTable(values[i]);
                dataGridView1.Location = new Point(0, usedHeight);
                dataGridView1.Size = new Size(usedWidth, (32 * (values[i][0].Item2.Count + 1)) + 2);

                usedHeight += (32 * (values[i][0].Item2.Count + 1)) + 2 + 25;

                panel.Controls.Add(dataGridView1);
            }

            //

            usedHeight += 15;
            Label label1 = new Label();
            label1.Location = new Point(20, usedHeight);
            label1.Size = new Size(usedWidth, 40);
            usedHeight += 40;
            label1.Text = "Final Analysis";
            label1.Font = new Font("Arial", 25, FontStyle.Bold);
            label1.ForeColor = ColorTranslator.FromHtml("#0078D7");
            label1.TextAlign = ContentAlignment.MiddleCenter;
            panel.Controls.Add(label1);
            usedHeight += 20;

            Label label2 = new Label();
            label2.Location = new Point(0, usedHeight);
            label2.Size = new Size(usedWidth, 120);
            label2.Text = finalAnalysis;
            label2.Font = new Font("Arial", 13);
            label2.AutoSize = true;
            label2.AutoEllipsis = true;
            if (!accepted)
            {
                label2.ForeColor = Color.Red;
            }
            label2.TextAlign = ContentAlignment.TopLeft;
            panel.Controls.Add(label2);

            //

            Button printButton = new Button();
            printButton.Name = "saveButton";
            printButton.Size = new Size(75, 23);
            printButton.Text = "&Save";
            printButton.Location = new Point(size.Width - 180, size.Height - 37);
            inputBox.Controls.Add(printButton);
            printButton.MouseClick += delegate (object? sender, MouseEventArgs e)
            {
                saveToExcel(results, values, tablesNames, finalAnalysis);
            };

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

        private static void saveToExcel(List<Data> results, List<List<Tuple<string, List<string>>>> values, List<string> tablesNames, string finalAnalysis)
        {
            Excel.Application oXL = new Excel.Application(); ;
            _Workbook oWB = oXL.Workbooks.Add("");
            Excel._Worksheet oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            oXL.Visible = false;

            for (int i = 0; i < results.Count; i++)
            {
                string tmp = "--";
                if (results[i].getValue() != null)
                {
                    if (results[i].getValue() is double)
                    {
                        tmp = String.Format("{0:0.0000}", results[i].getValue()) + "  " + results[i].getUnit();
                    }
                    else
                    {
                        tmp = results[i].getValue();
                    }
                }
                oSheet.Cells[i + 1, 1] = results[i].getName();
                oSheet.Cells[i + 1, 2] = tmp;
            }

            oSheet.get_Range("A1", "A1").ColumnWidth = 30;
            oSheet.get_Range("B1", "B1").ColumnWidth = 25;

            int x, y = 1;
            for (int i = 0; i < values.Count; i++)
            {
                x = 4;
                oSheet.Cells[y, x++] = $"{tablesNames[i]}:";
                foreach (var val in values[i])
                {
                    oSheet.Cells[y, x] = val.Item1;
                    char ch = (char)('A' + x - 1);
                    oSheet.get_Range(ch + y.ToString(), ch + y.ToString()).Font.Bold = true;
                    for (int j = 0; j < val.Item2.Count; j++)
                    {
                        oSheet.Cells[y + j + 1, x] = val.Item2[j];
                    }
                    x++;
                }
                y += values[i][0].Item2.Count;
                y++;
            }
            y = 1;
            oSheet.Cells[y, 14] = "Final Analysis";
            foreach (string w in finalAnalysis.Split('\n'))
            {
                oSheet.Cells[y++, 15] = w;
            }

            oSheet.get_Range("D1", "D1").ColumnWidth = 14;
            for (char ch = 'E'; ch <= 'L'; ch++)
            {
                oSheet.get_Range(ch + "1", ch + "1").ColumnWidth = 18;
            }

            oSheet.get_Range("N1", "N1").ColumnWidth = 13;
            oSheet.get_Range("O1", "O1").ColumnWidth = 80;

            oSheet.get_Range("A1", "A40").Font.Bold = true;
            oSheet.get_Range("D1", "D40").Font.Bold = true;
            oSheet.get_Range("N1", "N1").Font.Bold = true;

            for (int i = 1; i <= 40; i++)
            {
                oSheet.get_Range($"A{i}", $"Z{i}").HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog1.FileName = "Optical_Fiber_Design_Results.xlsx";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                oWB.SaveAs(saveFileDialog1.FileName,
                 XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                                false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }

            oWB.Close();
            oXL.Quit();
        }

        private static DataGridView GenerateRightSizeGridTable(List<Tuple<string, List<string>>> values)
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
            dataGridView.ColumnAdded += delegate (object? sender, DataGridViewColumnEventArgs e)
            {
                dataGridView.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
            };
            DataTable table = new DataTable();

            List<List<string>> TMP = new();

            foreach (var x in values)
            {
                table.Columns.Add(x.Item1);
                for (int i = 0; i < x.Item2.Count; i++)
                {
                    if (TMP.Count < i + 1) TMP.Add(new List<string>());
                    if (TMP[i] == null) TMP[i] = new();
                    TMP[i].Add(x.Item2[i]);
                }
            }
            foreach (var x in TMP)
            {
                table.Rows.Add(x.ToArray());
            }
            dataGridView.DataSource = table;
            return dataGridView;
        }

    }
}
