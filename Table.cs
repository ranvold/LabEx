using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabEx
{
    internal static class Table
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

        public static void Init(DataGridView dataGridViewEx)
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
        public static string CurrCell()
        {
            int currRow = _dataGridViewEx.CurrentCell.RowIndex;
            int currColumn = _dataGridViewEx.CurrentCell.ColumnIndex;
            return Cell.BuildNameCell(currColumn, currRow);
        }
        public static void UpdateDependencies(string currCell)
        {
            if (_database[currCell].CellDepends.Count != 0)
            {
                foreach (var cellDepends in _database[currCell].CellDepends)
                {
                    foreach (var dependentCells in _database[cellDepends].DependentCells)
                    {
                        if (dependentCells == currCell)
                        {
                            _database[cellDepends].DependentCells.Remove(currCell);
                        }
                        break;
                    }
                }
                _database[currCell].CellDepends.Clear();
            }
        }
        public static void RefreshCells(string currCell)
        {
            foreach (string item in _database[currCell].DependentCells)
            {
                int currColumn = _database[item].ColumnNumber;
                int currRow = _database[item].RowNumber;
                _dataGridViewEx.CurrentCell = _dataGridViewEx[currColumn, currRow];
                UpdateDependencies(item);
                _database[item].CellValue = Calculator.Evaluate(_database[item].Expression);
                _dataGridViewEx[currColumn, currRow].Value = _database[item].CellValue.ToString();
                RefreshCells(item);
                break;
            }
        }
    }
}
