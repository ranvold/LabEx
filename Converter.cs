using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabEx
{
    // The 26-ary system is used to convert the table column number.
    internal static class Converter
    {
        public static string To26System(int columnNum)
        {
            int temp = 0;
            int[] tempArr = new int[100];
            while(columnNum > 25)
            {
                tempArr[temp] = columnNum / 26 - 1;
                temp++;
                columnNum %= 26;
            }
            tempArr[temp] = columnNum;

            string converted = "";
            for (int i = 0; i <= temp; i++)
            {
                converted += ((char)('A' + tempArr[i])).ToString();
            }
            return converted;
        }
        public static int From26System(string columnHead)
        {
            char[] tempArr = columnHead.ToCharArray();
            int lengthArr = tempArr.Length;
            int converted = 0;
            for (int i = lengthArr - 2; i >= 0; i--)
            {
                converted += (((int)tempArr[i] - (int)'A') + 1)
                    * Convert.ToInt32(Math.Pow(26, lengthArr - i - 1));
            }
            converted += ((int)tempArr[lengthArr - 1] - (int)'A');
            return converted;
        }
    }
}
