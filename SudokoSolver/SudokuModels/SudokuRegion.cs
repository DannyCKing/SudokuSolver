using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace SudokoSolver.SudokuModels
{
    /// <summary>
    /// A sudoku region represts one of the nine squares that make up an entire
    /// sudoku board. Each region has 9 square within it.
    /// </summary>
    internal class SudokuRegion : BaseViewModel
    {
        internal event OnSquareSetInRegionHandler OnSquareSetInRegion;

        private ObservableCollection<SudokuSquare> _sudokuSquares = null;

        private SudokuState _currentState = SudokuState.Default;

        public SudokuState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                OnPropertyChanged(nameof(CurrentState));
            }
        }

        public ObservableCollection<SudokuSquare> Squares
        {
            get
            {
                return _sudokuSquares;
            }
            set
            {
                _sudokuSquares = value;
                OnPropertyChanged("Squares");
            }
        }

        public int RegionNumber { get; set; }

        public SudokuRegion(int regionRow, int regionColumn, int regionNumber)
        {
            RegionNumber = regionNumber;

            var squaresToAdd = new List<SudokuSquare>();

            for(int row = 0; row < 3; row++)
            {
                for(int column = 0; column < 3; column++)
                {
                    int regionFirstRow = regionRow * 3;
                    int regionFirstColumn = regionColumn * 3;

                    var newSquare = new SudokuSquare(regionFirstRow + row, regionFirstColumn + column, RegionNumber);
                    newSquare.OnSquareSet += NewSquare_OnSquareSet;
                    squaresToAdd.Add(newSquare);
                }
            }

            Squares = new ObservableCollection<SudokuSquare>(squaresToAdd);
        }

        public SudokuRegion(List<SudokuSquare> squares)
        {
            Squares = new ObservableCollection<SudokuSquare>(squares);
        }

        private void NewSquare_OnSquareSet(SudokuSquare updatedSquare)
        {
            OnSquareSetInRegion(this, updatedSquare);
        }
    }
}