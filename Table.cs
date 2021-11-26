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

        public static DataGridView DataGridViewEx
        {
            get { return _dataGridViewEx; }
            set { _dataGridViewEx = value; }
        }
        public static Dictionary<string, Cell> Database
        {
            get { return _database; }
            set { _database = value; }
        }
        public static int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
        public static int Columns
        { 
            get { return _columns; }
            set { _columns = value; }
        }

        public static void Init(DataGridView dataGridViewEx)
        {
            DataGridViewEx = dataGridViewEx;
            for (int i = 0; i < DefaultColumns; i++)
            {
                DataGridViewColumn defColumn = new();
                defColumn.HeaderText = Converter.To26System(i);
                defColumn.Name = defColumn.HeaderText;
                defColumn.CellTemplate = new Cell();
                DataGridViewEx.Columns.Add(defColumn);
                ++Columns;
            }
            for (int i = 0; i < DefaultRows; i++)
            {
                DataGridViewRow defRow = new();
                defRow.HeaderCell.Value = i.ToString();
                DataGridViewEx.Rows.Add(defRow);
                ++Rows;
            }

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Cell cell = new(i, j);
                    Database.Add(cell.Name, cell);
                }
            }
        }
        public static string CurrCell()
        {
            int currRow = DataGridViewEx.CurrentCell.RowIndex;
            int currColumn = DataGridViewEx.CurrentCell.ColumnIndex;
            return Cell.BuildNameCell(currColumn, currRow);
        }
        public static void UpdateDependencies(string currCell)
        {
            if (Database[currCell].CellDepends.Count != 0)
            {
                foreach (var cellDepends in Database[currCell].CellDepends)
                {
                    foreach (var dependentCells in Database[cellDepends].DependentCells)
                    {
                        if (dependentCells == currCell)
                        {
                            Database[cellDepends].DependentCells.Remove(currCell);
                        }
                        break;
                    }
                }
                Database[currCell].CellDepends.Clear();
            }

        }
        public static void RefreshCells(string currCell)
        {
            foreach (string item in Database[currCell].DependentCells)
            {
                int currColumn = Database[item].ColumnNumber;
                int currRow = Database[item].RowNumber;
                DataGridViewEx.CurrentCell = DataGridViewEx[currColumn, currRow];
                UpdateDependencies(item);
                Database[item].CellValue = Calculator.Evaluate(Database[item].Expression);
                DataGridViewEx[currColumn, currRow].Value = Database[item].CellValue.ToString();
                RefreshCells(item);
                break;
            }
        }

        public static void AddRow()
        {
            DataGridViewRow defRow = new();
            defRow.HeaderCell.Value = Rows.ToString();
            DataGridViewEx.Rows.Add(defRow);
            ++Rows;

            for (int i = 0; i < Columns; i++)
            {
                Cell cell = new(i, Rows - 1);
                Database.Add(cell.Name, cell);
            }
        }
        public static void AddColumn()
        {
            DataGridViewColumn defColumn = new();
            defColumn.HeaderText = Converter.To26System(Columns);
            defColumn.Name = defColumn.HeaderText;
            defColumn.CellTemplate = new Cell();
            DataGridViewEx.Columns.Add(defColumn);
            ++Columns;

            for (int j = 0; j < Rows; j++)
            {
                Cell cell = new(Columns - 1, j);
                Database.Add(cell.Name, cell);
            }
        }
        public static void DelRow()
        {
            int currRow = Rows - 1;
            for (int i = 0; i < Columns; i++)
            {
                if (Database[Cell.BuildNameCell(i, currRow)].DependentCells.Count != 0 ||
                    DataGridViewEx[i, currRow].Value != null)
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
            for (int i = 0; i < Columns; i++)
            {
                string currCell = Cell.BuildNameCell(i, currRow);
                if (Database[currCell].DependentCells.Count != 0)
                {
                    DataGridViewEx.CurrentCell = DataGridViewEx[i, currRow];
                    Database[currCell].CellValue = 0;
                    RefreshCells(currCell);

                    foreach (var dependentCell in Database[currCell].DependentCells)
                    {
                        foreach (var cellDepends in Database[dependentCell].CellDepends)
                        {
                            if (currCell == cellDepends)
                            {
                                Database[dependentCell].CellDepends.Remove(cellDepends);
                            }
                            break;
                        }
                    }
                }
                else if (Database[currCell].CellDepends.Count != 0)
                {
                    UpdateDependencies(currCell);
                }
            }
            for (int i = 0; i < Columns; i++)
            {
                Database.Remove(Cell.BuildNameCell(i, currRow));
            }
            DataGridViewEx.CurrentCell = DataGridViewEx[0, 0];
            DataGridViewEx.Rows.RemoveAt(currRow);
            --Rows;
        }
        public static void DelColumn()
        {
            int currColumn = Columns - 1;
            for (int j = 0; j < Rows; j++)
            {
                if (Database[Cell.BuildNameCell(currColumn, j)].DependentCells.Count != 0 ||
                    DataGridViewEx[currColumn, j].Value != null)
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
            for (int j = 0; j < Rows; j++)
            {
                string currCell = Cell.BuildNameCell(currColumn, j);
                if (Database[currCell].DependentCells.Count != 0)
                {
                    DataGridViewEx.CurrentCell = DataGridViewEx[currColumn, j];
                    Database[currCell].CellValue = 0;
                    RefreshCells(currCell);

                    foreach (var dependentCell in Database[currCell].DependentCells)
                    {
                        foreach (var cellDepends in Database[dependentCell].CellDepends)
                        {
                            if (currCell == cellDepends)
                            {
                                Database[dependentCell].CellDepends.Remove(cellDepends);
                            }
                            break;
                        }
                    }
                }
                else if (Database[currCell].CellDepends.Count != 0)
                {
                    UpdateDependencies(currCell);
                }
            }
            for (int j = 0; j < Rows; j++)
            {
                Database.Remove(Cell.BuildNameCell(currColumn, j));
            }
            DataGridViewEx.CurrentCell = DataGridViewEx[0, 0];
            DataGridViewEx.Columns.RemoveAt(currColumn);
            --Columns;
        }
    }
}
