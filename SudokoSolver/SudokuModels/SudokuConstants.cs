using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokoSolver.SudokuModels
{
    internal class SudokuConstants
    {
        public static List<int> PossibleValues
        {
            get
            {
                return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
        }

        public static List<string> GetSampleBoard(int boardNumber)
        {
            switch(boardNumber)
            {
                case 2:
                    return new List<string>
                    {
                        "7,0,0,0,5,8,0,0,2",
                        "6,0,5,0,0,2,0,0,9",
                        "2,0,9,0,7,0,0,3,8",
                        "0,9,7,8,0,3,2,5,0",
                        "0,0,0,2,0,0,0,1,3",
                        "3,4,0,1,9,0,6,8,0",
                        "0,0,3,0,0,0,0,9,0",
                        "8,0,0,0,3,0,7,0,0",
                        "0,2,6,7,8,0,3,0,0",
                    };
                    break;
                default:
                    return new List<string>
                    {
                        "0,0,7,5,3,0,0,0,0",
                        "0,1,0,0,4,0,7,2,5",
                        "0,0,0,8,2,7,0,6,0",
                        "4,5,1,9,6,0,8,0,0",
                        "0,3,6,0,0,0,0,4,0",
                        "0,0,0,0,7,4,0,0,0",
                        "0,7,5,0,0,6,0,0,3",
                        "3,0,9,2,0,8,5,0,0",
                        "2,4,8,0,5,3,0,1,0",
                    };
            }
        }
    }
}
