using System.Collections.Generic;
using System.Windows.Forms;

namespace LabEx
{
    public class Cell : DataGridViewTextBoxCell
    {
        private double _cellValue;
        private string _name;
        private string _expression;
        private int _columnNumber;
        private int _rowNumber;
        private List<string> _cellDepends = new();
        private List<string> _dependentCells = new();

        public Cell() { }
        public Cell(int columnNumber, int rowNumber)
        {
            _name = BuildCellName(columnNumber, rowNumber);
            _columnNumber = columnNumber;
            _rowNumber = rowNumber;
            _cellValue = 0;
            _expression = "";
        }

        public int ColumnNumber
        {
            get { return _columnNumber; }
            set { _columnNumber = value; }
        }
        public int RowNumber
        {
            get { return _rowNumber; }
            set { _rowNumber = value; }
        }
        public double CellValue
        {
            get { return _cellValue; }
            set { _cellValue = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }
        public List<string> CellDepends
        {
            get { return _cellDepends; }
            set { _cellDepends = value; }
        }
        public List<string> DependentCells
        {
            get { return _dependentCells; }
            set { _dependentCells = value; }
        }
        public static string BuildCellName(int column, int row)
        {
            return Converter.To26System(column) + row.ToString();
        }
        
        
    }
}
