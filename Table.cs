using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LabEx
{
    internal static class Table
    {
        private static int _rows = 0;
        private static int _columns = 0;
        private static string _currCellForCheckRecursion;
        private static Dictionary<string, Cell> _database = new();

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


        //Default table initialization.
        public static void InitTable(int columns, int rows, DataGridView dataGridViewEx)
        {
            for (int i = 0; i < columns; i++)
            {
                DataGridViewColumn defColumn = new();
                defColumn.HeaderText = Converter.To26System(i);
                defColumn.Name = defColumn.HeaderText;
                defColumn.CellTemplate = new Cell();
                dataGridViewEx.Columns.Add(defColumn);
                ++Columns;
            }
            for (int i = 0; i < rows; i++)
            {
                DataGridViewRow defRow = new();
                defRow.HeaderCell.Value = i.ToString();
                dataGridViewEx.Rows.Add(defRow);
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

        //The main program method for working with expressions.
        public static void CalcExpression(DataGridView dataGridViewEx, string expressionInBox)
        {
            int currColumn = dataGridViewEx.CurrentCell.ColumnIndex;
            int currRow = dataGridViewEx.CurrentCell.RowIndex;
            string currCell = Cell.BuildCellName(currColumn, currRow);

            UpdateDependencies(currCell);

            try
            {
                Database[currCell].CellValue = Calculator.Evaluate(expressionInBox);
                Database[currCell].Expression = expressionInBox;
                if (Database[currCell].CellValue.ToString() == "∞")
                {
                    dataGridViewEx[currColumn, currRow].Value = "#DIV/0!";
                }
                else if (expressionInBox != "")
                {
                    dataGridViewEx[currColumn, currRow].Value = Database[currCell].CellValue.ToString();
                }
                else
                {
                    dataGridViewEx[currColumn, currRow].Value = null;
                }
                if (Database[currCell].DependentCells.Count != 0)
                {
                    RefreshCells(currCell, dataGridViewEx);

                    dataGridViewEx.CurrentCell = dataGridViewEx[currColumn, currRow];
                }
            }
            catch (StackOverflowException)
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

        //When using cells in the expression, we update the dependencies.
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
                            break;
                        }
                    }
                }
                Database[currCell].CellDepends.Clear();
            }

            /* For the subsequent check of the cells for the presence of recursion. */
            _currCellForCheckRecursion = currCell;
        }

        //Adds dependencies when using cells in an expression. Closely related to LabExVisitor.VisitIdentifierExpr
        public static void AddDependencies(string result, Cell cellInExpression)
        {
            /* First make sure the current cell does not create a stack overflow. */
            if (result == _currCellForCheckRecursion || ReccurenceCheck(cellInExpression, Database[_currCellForCheckRecursion]))
            {
                throw new StackOverflowException();
            }
            Database[_currCellForCheckRecursion].CellDepends.Add(cellInExpression.Name);
            cellInExpression.DependentCells.Add(_currCellForCheckRecursion);
        }

        //We recurrently update the cells that depend on the entered one.
        public static void RefreshCells(string currCell, DataGridView dataGridViewEx)
        {
            foreach (string item in Database[currCell].DependentCells)
            {
                int currColumn = Database[item].ColumnNumber;
                int currRow = Database[item].RowNumber;
                dataGridViewEx.CurrentCell = dataGridViewEx[currColumn, currRow];
                UpdateDependencies(item);

                Database[item].CellValue = Calculator.Evaluate(Database[item].Expression);
                if (Database[item].CellValue.ToString() == "∞")
                {
                    dataGridViewEx[currColumn, currRow].Value = "#DIV/0!";
                }
                else
                {
                    dataGridViewEx[currColumn, currRow].Value = Database[item].CellValue.ToString();
                }
                RefreshCells(item, dataGridViewEx);
                break;
            }
        }

        //We just clear the table and then the entire dictionary. Only A0 remains.
        public static void ClearTable(DataGridView dataGridViewEx)
        {
            foreach (var dependencies in Database.Values)
            {
                dependencies.DependentCells.Clear();
                dependencies.CellDepends.Clear();
                dataGridViewEx[dependencies.ColumnNumber, dependencies.RowNumber].Value = null;
            }
            int currColumnsCount = Columns - 1;
            for (int i = 0; i < currColumnsCount; i++)
            {
                DelColumn(dataGridViewEx.Columns.Count - 1, dataGridViewEx);
            }
            int currRowsCount = Rows - 1;
            for (int j = 0; j < currRowsCount; j++)
            {
                DelRow(dataGridViewEx.Rows.Count - 1, dataGridViewEx);
            }
            Database.Clear();
            Cell cell = new(0, 0);
            Database.Add(cell.Name, cell);
        }
        public static void AddRow(DataGridView dataGridViewEx)
        {
            DataGridViewRow defRow = new();
            defRow.HeaderCell.Value = Rows.ToString();
            dataGridViewEx.Rows.Add(defRow);
            ++Rows;

            for (int i = 0; i < Columns; i++)
            {
                Cell cell = new(i, Rows - 1);
                Database.Add(cell.Name, cell);
            }
        }
        public static void AddColumn(DataGridView dataGridViewEx)
        {
            DataGridViewColumn defColumn = new();
            defColumn.HeaderText = Converter.To26System(Columns);
            defColumn.Name = defColumn.HeaderText;
            defColumn.CellTemplate = new Cell();
            dataGridViewEx.Columns.Add(defColumn);
            ++Columns;

            for (int j = 0; j < Rows; j++)
            {
                Cell cell = new(Columns - 1, j);
                Database.Add(cell.Name, cell);
            }
        }

        //After checking for dependence, we remove cells from the table and the database one by one.
        public static void DelColumn(int lastColumn, DataGridView dataGridViewEx)
        {
            for (int j = 0; j < Rows; j++)
            {
                string cellName = Cell.BuildCellName(lastColumn, j);
                if (Database[cellName].DependentCells.Count != 0)
                {
                    dataGridViewEx.CurrentCell = dataGridViewEx[lastColumn, j];
                    Database[cellName].CellValue = 0;
                    RefreshCells(cellName, dataGridViewEx);

                    foreach (var dependentCell in Database[cellName].DependentCells)
                    {
                        foreach (var cellDepends in Database[dependentCell].CellDepends)
                        {
                            if (cellName == cellDepends)
                            {
                                Database[dependentCell].CellDepends.Remove(cellDepends);
                                break;
                            }
                        }
                    }
                }
                else if (Database[cellName].CellDepends.Count != 0)
                {
                    UpdateDependencies(cellName);
                }
            }
            for (int j = 0; j < Rows; j++)
            {
                Database.Remove(Cell.BuildCellName(lastColumn, j));
            }
            dataGridViewEx.CurrentCell = dataGridViewEx[0, 0];
            dataGridViewEx.Columns.RemoveAt(lastColumn);
            --Columns;
        }

        //Just like columns.
        public static void DelRow(int lastRow, DataGridView dataGridViewEx)
        {
            for (int i = 0; i < Columns; i++)
            {
                string cellName = Cell.BuildCellName(i, lastRow);
                if (Database[cellName].DependentCells.Count != 0)
                {
                    dataGridViewEx.CurrentCell = dataGridViewEx[i, lastRow];
                    Database[cellName].CellValue = 0;
                    RefreshCells(cellName, dataGridViewEx);

                    foreach (var dependentCell in Database[cellName].DependentCells)
                    {
                        foreach (var cellDepends in Database[dependentCell].CellDepends)
                        {
                            if (cellName == cellDepends)
                            {
                                Database[dependentCell].CellDepends.Remove(cellDepends);
                                break;
                            }
                        }
                    }
                }
                else if (Database[cellName].CellDepends.Count != 0)
                {
                    UpdateDependencies(cellName);
                }
            }
            for (int i = 0; i < Columns; i++)
            {
                Database.Remove(Cell.BuildCellName(i, lastRow));
            }
            dataGridViewEx.CurrentCell = dataGridViewEx[0, 0];
            dataGridViewEx.Rows.RemoveAt(lastRow);
            --Rows;
        }

        //Recursive check for recursion)
        public static bool ReccurenceCheck(Cell cellInExpression, Cell currCellFromGrid)
        {
            if (cellInExpression.CellDepends.Contains(currCellFromGrid.Name))
            {
                return true;
            }
            foreach (var prevCell in cellInExpression.CellDepends)
            {
                if (ReccurenceCheck(Database[prevCell], currCellFromGrid))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Save(System.IO.StreamWriter sw)
        {
            sw.WriteLine(Columns);
            sw.WriteLine(Rows);
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Cell cell = Database[Cell.BuildCellName(i, j)];
                    sw.WriteLine(cell.Name);
                    sw.WriteLine(cell.Expression);
                    sw.WriteLine(cell.CellValue);

                    if (cell.CellDepends.Count == 0)
                    {
                        sw.WriteLine("0");
                    }
                    else
                    {
                        sw.WriteLine(cell.CellDepends.Count);
                        foreach (string cellDepends in cell.CellDepends)
                        {
                            sw.WriteLine(cellDepends);
                        }
                    }
                    if (cell.DependentCells.Count == 0)
                    {
                        sw.WriteLine("0");
                    }
                    else
                    {
                        sw.WriteLine(cell.DependentCells.Count);
                        foreach (string dependentCells in cell.DependentCells)
                        {
                            sw.WriteLine(dependentCells);
                        }
                    }
                }
            }
        }
        public static void Open(int columns, int rows, System.IO.StreamReader sr, DataGridView dataGridView)
        {
            for (int i = 0; i < columns - 1; i++)
            {
                AddColumn(dataGridView);
            }
            for (int j = 0; j < rows - 1; j++)
            {
                AddRow(dataGridView);
            }
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    string name = sr.ReadLine();
                    string expression = sr.ReadLine();
                    string value = sr.ReadLine();

                    if (expression != "")
                    {
                        Database[name].Expression = expression;
                        Database[name].CellValue = double.Parse(value);
                        dataGridView[i, j].Value = value;
                    }

                    int cellDependsCount = int.Parse(sr.ReadLine());

                    for (int k = 0; k < cellDependsCount; k++)
                    {
                        Database[name].CellDepends.Add(sr.ReadLine());
                    }

                    int dependentCellsCount = int.Parse(sr.ReadLine());

                    for (int h = 0; h < dependentCellsCount; h++)
                    {
                        Database[name].DependentCells.Add(sr.ReadLine());
                    }
                }
            }
        }
    }
}
