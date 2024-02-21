using SudokoSolver.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SudokoSolver.SudokuModels
{
    internal class SudokuBoard : BaseViewModel
    {
        private bool _isSolving = false;

        public bool IsSolving
        {
            get
            {
                return _isSolving;
            }
            set
            {
                _isSolving = value;
                OnPropertyChanged(nameof(IsSolving));
            }
        }

        private ObservableCollection<SudokuRegion> _regions = null;

        public ObservableCollection<SudokuRegion> Regions
        {
            get
            {
                if(_regions == null)
                {
                    _regions = new ObservableCollection<SudokuRegion>();
                }
                return _regions;
            }
            set
            {
                _regions = value;
                OnPropertyChanged("Regions");
            }
        }

        private Dictionary<int, SudokuRegion> _Rows = new Dictionary<int, SudokuRegion>();
        private Dictionary<int, SudokuRegion> _Columns = new Dictionary<int, SudokuRegion>();
        private Dictionary<int, SudokuRegion> _Regions = new Dictionary<int, SudokuRegion>();


        public SudokuBoard()
        {
            Regions = new ObservableCollection<SudokuRegion>();
            int regionNumber = 0;

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    var region = new SudokuRegion(row, column, regionNumber);
                    region.OnSquareSetInRegion += Region_OnSquareSetInRegion;
                    Regions.Add(region);
                    _Regions.Add(regionNumber, region);
                    regionNumber++;
                }
            }

            // add squares to row
            for(int row = 0;row < 9; row++)
            {
                var squaresToAdd = new List<SudokuSquare>();
                foreach(var region in Regions)
                {
                    var squares = region.Squares.Where(x => x.BoardRow == row);
                    squaresToAdd.AddRange(squares);
                }

                _Rows[row] = new SudokuRegion(squaresToAdd);
            }

            // add squares to column
            for (int column = 0; column < 9; column++)
            {
                var squaresToAdd = new List<SudokuSquare>();
                foreach (var region in Regions)
                {
                    var squares = region.Squares.Where(x => x.BoardColumn == column);
                    squaresToAdd.AddRange(squares);
                }

                _Columns[column] = new SudokuRegion(squaresToAdd);
            }

#if DEBUG
            LoadSampleBoard();
#endif
        }

        private void LoadSampleBoard()
        {
            var sampleBoard = SudokuConstants.GetSampleBoard(2);
            for (int row = 0 ; row < sampleBoard.Count; row++)
            {
                var rowValues = sampleBoard[row];
                rowValues = rowValues.Replace(",", "");
                for(int column = 0 ; column < rowValues.Length; column++)
                {
                    var currentValue = int.Parse(rowValues[column].ToString());
                    if(currentValue != 0)
                    {
                        SetSquare(row, column, currentValue);
                    }
                }
            }
        }

        private void Region_OnSquareSetInRegion(SudokuRegion region, SudokuSquare updatedSquare)
        {
            // update all the squares in the region, row and column based on current values
            var squaresInRegion = region.Squares.ToList();
            var squaresInRow = _Rows[updatedSquare.BoardRow].Squares.ToList();
            var squareInColumn = _Columns[updatedSquare.BoardColumn].Squares.ToList();

            var allUpdatedSquares = squaresInRegion.Union(squaresInRow).Union(squareInColumn).ToList();
            foreach (var square in allUpdatedSquares)
            {
                if (square != updatedSquare && !square.CurrentValue.HasValue)
                {
                    // update all squares except the current square and any ones that are already set
                    var currentRegion = _Regions[square.RegionNumber];
                    var currentRow = _Rows[square.BoardRow];
                    var currentColumn = _Columns[square.BoardColumn];
                    square.SetPossibleValuesBasedOnCurrentBoard(currentRegion, currentRow, currentColumn);
                }
            }
        }

        private ICommand _solveSquareCommand = null;
        public ICommand SolvePossibleSquaresCommand
        {
            get
            {
                if (_solveSquareCommand == null)
                {
                    _solveSquareCommand = new BaseCommand(SolvePossiblesSquares, CanSolvePossibleSquare);
                }
                return _solveSquareCommand;
            }
        }

        private ICommand _verifyBoardCommand = null;
        public ICommand VerifyBoardCommand
        {
            get
            {
                if (_verifyBoardCommand == null)
                {
                    _verifyBoardCommand = new BaseCommand(VerifyBoard, CanVerifyBoard);
                }
                return _verifyBoardCommand;
            }
        }

        private bool CanVerifyBoard()
        {
            return !IsSolving;
        }

        private void VerifyBoard()
        {
            IsSolving = true;

            foreach(var region in Regions)
            {
                try
                {
                    foreach (var number in SudokuConstants.PossibleValues)
                    {
                        region.Squares.Single(x => x.CurrentValue == number);
                    }
                }
                catch (Exception ex) 
                {
                    region.CurrentState = SudokuState.Error;
                    continue;
                }

                region.CurrentState = SudokuState.LockedIn;
            }

            IsSolving = false;
        }

        private bool CanSolvePossibleSquare()
        {
            return !IsSolving;
        }

        private void SolvePossiblesSquares()
        {
            IsSolving = true;

            var allSquares = Regions.SelectMany(region => region.Squares);

            foreach(var square in allSquares)
            {
                if(square.CanBeSet())
                {
                    square.SetToPossibleValue();
                }
            }

            IsSolving = false;
        }

        private bool SetSquare(int row, int column, int valueToSet)
        {
            try
            {
                var allSquares = Regions.SelectMany(_region => _region.Squares);
                var square = allSquares.Single(x => x.BoardRow == row && x.BoardColumn == column);
                square.CurrentValue = valueToSet;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
