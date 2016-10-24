using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Symbol { minus = '-', zero = '0', one = '1'}

namespace Automats
{
    class Program
    {
        static Program pr = new Program();
        static void Main(string[] args)
        {
            char[,] JKXtable = new char[3, 30];

            int[,] QXTable = {  { 0,0,0,  0,0,1,  0,1,0,  0,1,1,  1,0,0}, 
                                { 1,0,0,  0,1,0,  0,0,0,  0,0,1,  0,0,0},
                                { 0,0,0,  0,0,0,  0,1,1,  0,1,0,  0,0,1},
                                { 0,1,0,  0,1,0,  1,0,0,  0,0,1,  1,0,0} };

            JKXtable = pr.FillOutJKXTable(QXTable);
            pr.ShowTable(JKXtable);
            Console.ReadKey();
        }

        private string CompareInts(int topChar, int crossChar)
        {
            string output;
            if (topChar == 0)
            {
                output = crossChar == 0 ? "0-" : "1-";
            }
            else   
                output = crossChar == 0 ? "-1" : "-0";
            return output;
        }

        private void ShowTable(char[,] table)
        {
            for (int line = 0; line < 3; ++line)
            {
                string xx = Convert.ToString(line, 2);
                if (xx.Length == 1)
                    Console.Write("0");
                Console.Write(xx + '\t');
                for (int charNumber = 0; charNumber < 30; ++charNumber)
                {
                    //table[line, charNumber] = ChooseCharacter();
                    Console.Write(table[line, charNumber] + " ");
                }
                Console.WriteLine("\n");
            }
        }
        private char[,] FillOutJKXTable(int[,] QXTable)
        {
            char[,] JKXtable = new char[3, 30];
            const int TOP_LINE = 0; const int CHARS_PER_Q = 2;
            for (int line = 1; line < 4; ++line)
            {
                string outputCharacters;
                int charCounter = 0;
                for (int crossChar = 0; crossChar < 15; ++crossChar)
                {
                    outputCharacters = pr.CompareInts(QXTable[TOP_LINE, crossChar], QXTable[line, crossChar]);
                    for (int i = 0; i < CHARS_PER_Q; i++) JKXtable[line - 1, charCounter + i] = outputCharacters[i];
                    charCounter += 2;
                }
            }
            return JKXtable;
        }
    }
}
