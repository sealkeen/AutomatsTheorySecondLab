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
            ////Debug
            //int var = 0;
            //for (int k = 0; k < 8; k++)
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        Console.WriteLine(var);
            //    }
            //    var += 6;
            //}
            ////Debug Function DiagColumnToInsert
            //for (int i = 0; i > -30; i--) { if ((i) % 6 == 0) Console.WriteLine(); Console.Write(pr.DiagColumnToInsert(i)); }
            //Console.ReadKey(); return;
            ////Debug


            char[,] JKXtable = new char[3, 30];
            char[,,] DiagTables = new char[6, 4, 8]; pr.InitializeDiags(ref DiagTables);

            int[,] QXTable = {  { 0,0,0,  0,0,1,  0,1,0,  0,1,1,  1,0,0}, 
                                { 1,0,0,  0,1,0,  0,0,0,  0,0,1,  0,0,0},
                                { 0,0,0,  0,0,0,  0,1,1,  0,1,0,  0,0,1},
                                { 0,1,0,  0,1,0,  1,0,0,  0,0,1,  1,0,0} };

            JKXtable = pr.FillOutJKXTable(QXTable);
            pr.ShowTable(JKXtable);

            pr.FillOutDiagTables(ref DiagTables, JKXtable);
            pr.ShowTable(DiagTables);
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

        /// <summary>
        /// Выводим таблицу на экран
        /// </summary>
        /// <param name="table"></param>
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
        private void ShowTable(char[,,] tables)
        {
            const int TABLE_COUNT = 6;
            const int LINE_COUNT = 4;
            const int CHAR_PER_LINE = 8;
            for (int tableIndex = 0; tableIndex < TABLE_COUNT; ++tableIndex)
            {
                for (int line = 0; line < LINE_COUNT; ++line)
                {
                    string xx = Convert.ToString(line, 2);
                    if (xx.Length == 1)
                        Console.Write("0");
                    Console.Write(xx + '\t');
                    for (int charNumber = 0; charNumber < CHAR_PER_LINE; ++charNumber)
                    {
                        Console.Write(tables[tableIndex, line, charNumber] + " ");
                    }
                    Console.WriteLine("\n");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Заполняем таблицу входных символов JK триггера
        /// </summary>
        /// <param name="QXTable"></param>
        /// <returns></returns>
        private char[,] FillOutJKXTable(int[,] QXTable)
        {
            char[,] JKXtable = new char[3, 30];                 //Таблица входов JK триггеров
            const int TOP_LINE = 0; const int CHARS_PER_Q = 2;  //С верхней строкой сравниваются все остальные
            for (int line = 1; line < 4; ++line)                
            {
                string outputCharacters;                        //Набор символов {0 || 1 || -}, которые должны подаваться на вход триггера в таблице JKTable
                int charCounter = 0;                            //Построчный счётчик символов
                for (int crossChar = 0; crossChar < 15; ++crossChar)
                {
                    outputCharacters = pr.CompareInts(QXTable[TOP_LINE, crossChar], QXTable[line, crossChar]);  
                    for (int i = 0; i < CHARS_PER_Q; i++) JKXtable[line - 1, charCounter + i] = outputCharacters[i]; //Мы вставили 2 символа в таблицу 
                    charCounter += 2;                           //Прибавили 2 к счётчику символов
                }
            }
            return JKXtable;
        }

        /// <summary>
        /// Заполняем 3 диаграмм вейча
        /// </summary>
        /// <param name="firstTable"></param>
        /// <param name="secondTable"></param>
        /// <param name="thirdTable"></param>
        /// <param name="JKTable"></param>
        /// <returns></returns>
        private void FillOutDiagTables(ref char[,,] diagTable, char[,] JKTable)
        {
            const int TABLE_COUNT = 6;
            const int LINE_COUNT = 3;
            const int COLUMN_COUNT = 8;
            int insertDiagColumn = -1;
            int jkLine = -1;
            int jkStartColumn = 0; //Индекс столбца из JKtable, с него начинаем заполнять в цикле столбец DiagTable (Таблицы заполняются по очереди с 0 до 5)
            for (int diagColumn = 0; diagColumn < COLUMN_COUNT; ++diagColumn)
            {
                insertDiagColumn = DiagColumnToInsert(diagColumn);
                for (int diagLine = 0; diagLine < LINE_COUNT; ++diagLine)
                {
                    for (int table = 0; table < TABLE_COUNT; ++table)
                    {
                        jkLine = diagLine == 3 ? 2 : diagLine;
                        diagLine = diagLine == 2 ? 3 : diagLine;
                        if ((jkStartColumn + table) > 29) break;
                        diagTable[table, diagLine, insertDiagColumn] = JKTable[jkLine, jkStartColumn + table]; 

                    } 
                }
                jkStartColumn += 6;
            }
        }

        /// <summary>
        /// В какую часть таблицы вставить значения
        /// </summary>
        /// <param name="number"></param>
        private int DiagColumnToInsert(int number)
        {
            int returnedValue = -1;
            List<int> lineMembership = new List<int> { 0, 1, 3, 2, 6, 7, 5, 4 }; // 0 соответствует 000 значениям (Q3 Q2 Q1), так же как 4 => 100 значениям (Q3 Q2 Q1)
            returnedValue = lineMembership.IndexOf(number);
            return returnedValue;
        }
        private void InitializeDiags(ref char[,,] diagTable)
        {
            const int TABLE_COUNT = 6; const int LINES_COUNT = 4; const int CHARS_PER_LINE = 8;
            for (int table = 0; table < TABLE_COUNT; table++)
            {
                for (int line = 0; line < LINES_COUNT; line++)
                {
                    for (int Char = 0; Char < CHARS_PER_LINE; ++Char)
                    {
                        diagTable[table, line, Char] = '-';
                    }
                }
            }
        }
    }
}
