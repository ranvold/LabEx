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


        //The main program method for working with expressions.
        private void CalcExpr()
        {
            int currColumn = DataGridViewEx.CurrentCell.ColumnIndex;
            int currRow = DataGridViewEx.CurrentCell.RowIndex;
            string currCell = Cell.BuildCellName(currColumn, currRow);
            Table.UpdateDependencies(currCell);
            try
            {
                Table.Database[currCell].CellValue = Calculator.Evaluate(TextBoxExpression.Text);
                Table.Database[currCell].Expression = TextBoxExpression.Text;
                if (Table.Database[currCell].CellValue.ToString() == "∞")
                {
                    DataGridViewEx[currColumn, currRow].Value = "#DIV/0!";
                }
                else if (TextBoxExpression.Text != "")
                {
                    DataGridViewEx[currColumn, currRow].Value = Table.Database[currCell].CellValue.ToString();
                }
                else
                {
                    DataGridViewEx[currColumn, currRow].Value = null;
                }
                if (Table.Database[currCell].DependentCells.Count != 0)
                {
                    Table.RefreshCells(currCell, DataGridViewEx);
                }
            }
            catch(StackOverflowException)
            {
                MessageBox.Show(
                    "Reccurence is present! Please try to enter a valid expression.",
                    "WARNING!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            }
            catch
            {
                MessageBox.Show(
                    "Invalid expression entered, supported operations: " +
                    "+, -, *, /, ^, inc, dec, nmax(x1, x2,..., xN), nmin(x1, x2,...xN)(N>=1). " +
                    "And also rational numbers. " +
                    "When introducing cells, use uppercase Latin letters, for example A0+B1.",
                    "INFO",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
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
            CalcExpr();
        }
        
        private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalcExpr();

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
                StreamWriter sw = new StreamWriter(fs);
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
            int.TryParse(sr.ReadLine(), out int columns);
            int.TryParse(sr.ReadLine(), out int rows);
            Table.Open(columns, rows, sr, DataGridViewEx);
            sr.Close();
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
