using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabEx
{
    public partial class FormEx : Form
    {
        public FormEx()
        {
            InitializeComponent();
            Table.Init(DataGridViewEx);
        }

        private void FormEx_Load(object sender, EventArgs e)
        {
            DataGridViewEx.CurrentCell = DataGridViewEx[0, 0];
        }

        private void DataGridViewEx_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            string currentCell = Cell.BuildNameCell(e.ColumnIndex, e.RowIndex);

            if (Table.Database[currentCell].Expression == "")
            {
                TextBoxExpression.Text = Table.Database[currentCell].Expression;
            }
            else
            {
                TextBoxExpression.Text = Table.Database[currentCell].Expression;
                DataGridViewEx[e.ColumnIndex, e.RowIndex].Value = Table.Database[currentCell].CellValue.ToString();
            }
        }

        private void ButtonEnterExp_Click(object sender, EventArgs e)
        {
            CalcExpr(Table.CurrCell());
        }
        public void CalcExpr(string currCell)
        {
            Table.UpdateDependencies(currCell);
            int currColumn = Table.Database[currCell].ColumnNumber;
            int currRow = Table.Database[currCell].RowNumber;
            try
            {
                Table.Database[currCell].CellValue = Calculator.Evaluate(TextBoxExpression.Text);
                Table.Database[currCell].Expression = TextBoxExpression.Text;
                DataGridViewEx[currColumn, currRow].Value = Table.Database[currCell].CellValue.ToString();
                if (Table.Database[currCell].DependentCells.Count != 0)
                {
                    Table.RefreshCells(currCell);
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }

        }

        private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalcExpr(Table.CurrCell());

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void ButtonAddRow_Click(object sender, EventArgs e)
        {
            Table.AddRow();
        }

        private void ButtonAddColumn_Click(object sender, EventArgs e)
        {
            Table.AddColumn();
        }

        private void ButtonDelRow_Click(object sender, EventArgs e)
        {
            Table.DelRow();
        }

        private void ButtonDelColumn_Click(object sender, EventArgs e)
        {
            Table.DelColumn();
        }
    }
}
