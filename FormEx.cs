using System;
using System.IO;
using System.Windows.Forms;

namespace LabEx
{
    public partial class FormEx : Form
    { 
        private const int DefaultColumns = 8;
        private const int DefaultRows = 15;
        public FormEx()
        {
            InitializeComponent();
            Table.InitTable(DefaultColumns, DefaultRows, DataGridViewEx);
        }

        private void DataGridViewEx_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            string currentCell = Cell.BuildCellName(e.ColumnIndex, e.RowIndex);

            TextBoxExpression.Text = Table.Database[currentCell].Expression;
            if (Table.Database[currentCell].CellValue.ToString() == "∞")
            {
                DataGridViewEx[e.ColumnIndex, e.RowIndex].Value = "#DIV/0!";
            }
            else if (TextBoxExpression.Text != "")
            {
                DataGridViewEx[e.ColumnIndex, e.RowIndex].Value = Table.Database[currentCell].CellValue.ToString();
            }
            TextBoxExpression.Focus();
            TextBoxExpression.SelectAll();
        }

        private void ButtonEnterExp_Click(object sender, EventArgs e)
        {
            Table.CalcExpression(DataGridViewEx, TextBoxExpression.Text);
        }
        
        private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Table.CalcExpression(DataGridViewEx, TextBoxExpression.Text);

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void ButtonAddColumn_Click(object sender, EventArgs e)
        {
            Table.AddColumn(DataGridViewEx);
        }

        private void ButtonDelColumn_Click(object sender, EventArgs e)
        {
            if (DataGridViewEx.Columns.Count == 1)
            {
                MessageBox.Show(
                    "Minimum number of columns reached!",
                    "INFO",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            int lastColumn = DataGridViewEx.Columns.Count - 1;
            for (int j = 0; j < DataGridViewEx.Rows.Count; j++)
            {
                if (Table.Database[Cell.BuildCellName(lastColumn, j)].DependentCells.Count != 0 ||
                    DataGridViewEx[lastColumn, j].Value != null)
                {
                    DialogResult result = MessageBox.Show(
                       "Some cells have dependent cells or data. Are you sure you want to delete the line?",
                       "WARNING!",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning,
                       MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        break;
                    }
                }
            }
            Table.DelColumn(lastColumn, DataGridViewEx);
        }

        private void ButtonAddRow_Click(object sender, EventArgs e)
        {
            Table.AddRow(DataGridViewEx);
        }

        private void ButtonDelRow_Click(object sender, EventArgs e)
        {
            if (DataGridViewEx.Rows.Count == 1)
            {
                MessageBox.Show(
                    "Minimum number of rows reached!",
                    "INFO",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            int lastRow = DataGridViewEx.Rows.Count - 1;
            for (int i = 0; i < DataGridViewEx.Columns.Count; i++)
            {
                if (Table.Database[Cell.BuildCellName(i, lastRow)].DependentCells.Count != 0 ||
                    DataGridViewEx[i, lastRow].Value != null)
                {
                    DialogResult result = MessageBox.Show(
                       "Some cells have dependent cells or data. Are you sure you want to delete the line?",
                       "WARNING!",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning,
                       MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        break;
                    }
                }
            }
            Table.DelRow(lastRow, DataGridViewEx);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "TableFile|*.txt";
            saveFileDialog.Title = "Save table file";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                StreamWriter sw = new(fs);
                Table.Save(sw);
                sw.Close();
                fs.Close();
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "TableFile|*.txt";
            openFileDialog.Title = "Open Table File";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            StreamReader sr = new(openFileDialog.FileName);
            Table.ClearTable(DataGridViewEx);
            int columns = int.Parse(sr.ReadLine());
            int rows = int.Parse(sr.ReadLine());
            Table.Open(columns, rows, sr, DataGridViewEx);
            sr.Close();
            DataGridViewEx.Focus();
        }

        private void FormEx_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close the program? Unsaved data will be lost.",
                "WARNING!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            e.Cancel = result == DialogResult.No;
        }
    }
}
