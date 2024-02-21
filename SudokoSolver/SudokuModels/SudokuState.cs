using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokoSolver.SudokuModels
{
    public enum SudokuState
    {
        Unknown,
        Default,
        OnePossibility,
        Error,
        Set,
        LockedIn
    }
}
