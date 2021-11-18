﻿using System;
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
        private static Table _table;
        public FormEx()
        {
            InitializeComponent();
            _table = new Table(DataGridViewEx);
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

        private void ButtonCalcExp_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
