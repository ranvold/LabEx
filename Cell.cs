using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabEx
{
    internal class Cell : DataGridViewTextBoxCell
    {
        private double _cellValue;
        private string _name;
        private string _expression;
        private int _columnNumber;
        private int _rowNumber;

        public Cell() { }
        public Cell(int columnNumber, int rowNumber)
        {
            _name = BuildNameCell(columnNumber, rowNumber);
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

        public static string BuildNameCell(int column, int row)
        {
            return Converter.To26System(column) + row.ToString();
        }
        
    }
}
