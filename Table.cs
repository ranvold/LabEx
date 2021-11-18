using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabEx
{
    internal class Table
    {
        private const int DefaultRows = 15;
        private const int DefaultColumns = 8;
        private static int _rows = 0;
        private static int _columns = 0;
        private static DataGridView _dataGridViewEx;
        private static Dictionary<string, Cell> _database = new();

        public static Dictionary<string, Cell> Database
        {
            get { return _database; }
            set { _database = value; }
        }

        public Table(DataGridView dataGridViewEx)
        {
            _dataGridViewEx = dataGridViewEx;
            for (int i = 0; i < DefaultColumns; i++)
            {
                DataGridViewColumn defColumn = new();
                defColumn.HeaderText = Converter.To26System(i);
                defColumn.Name = defColumn.HeaderText;
                defColumn.CellTemplate = new Cell();
                _dataGridViewEx.Columns.Add(defColumn);
                ++_columns;
            }
            for (int i = 0; i < DefaultRows; i++)
            {
                DataGridViewRow defRow = new();
                defRow.HeaderCell.Value = i.ToString();
                _dataGridViewEx.Rows.Add(defRow);
                ++_rows;
            }

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Cell cell = new(i, j);
                    _database.Add(cell.Name, cell);
                }
            }
        }
    }
}
