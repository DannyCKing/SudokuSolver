using SudokoSolver.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace SudokoSolver.SudokuModels
{
    delegate void OnSquareSetHandler(SudokuSquare updatedSquare);
    delegate void OnSquareSetInRegionHandler(SudokuRegion updatedRegion, SudokuSquare updatedSquare);

    internal class SudokuSquare : BaseViewModel
    {
        internal event OnSquareSetHandler OnSquareSet;

        private SudokuState _currentState = SudokuState.Unknown;

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


        private int? _currentValue = null;
        public int? CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = value;

                if (_currentValue.HasValue)
                {
                    if (NonPossibleValues.Contains(_currentValue.Value))
                    {
                        CurrentState = SudokuState.Error;
                    }

                    var newVal = _currentValue.Value;
                    // only one value is possible if it is set
                    PossibleValues = new List<int>() { newVal };

                    // all values are not possible except for the current value
                    NonPossibleValues = SudokuConstants.PossibleValues;
                    NonPossibleValues.Remove(newVal);
                    CurrentState = SudokuState.Set;
                }
                else
                {
                    CurrentState = SudokuState.Unknown;
                }

                if (OnSquareSet != null)
                {
                    OnSquareSet(this);
                }
                OnPropertyChanged(nameof(CurrentValue));
            }
        }

        public List<int> PossibleValues { get; set; }

        public List<int> NonPossibleValues { get; set; }

        public int BoardRow { get; private set; }

        public int BoardColumn { get; private set; }

        public int RegionNumber { get; private set; }

        public SudokuSquare(int row, int column, int regionNumber)
        {
            BoardRow = row;
            BoardColumn = column;
            RegionNumber = regionNumber;
            CurrentValue = null;
            PossibleValues = SudokuConstants.PossibleValues;
            NonPossibleValues = new List<int>();
        }

        public void SetPossibleValuesBasedOnCurrentBoard(SudokuRegion currentRegion, SudokuRegion currentRow, SudokuRegion currentColumn)
        {
            if(CurrentValue.HasValue)
            {
                // don't set already set fields
                return;
            }
            var usedValuesInRegion = currentRegion.Squares.Where(x => x.CurrentValue.HasValue).Select(x => x.CurrentValue.Value).ToList();
            var usedValuesInRow = currentRow.Squares.Where(x => x.CurrentValue.HasValue).Select(x => x.CurrentValue.Value).ToList();
            var usedValuesInColumn = currentColumn.Squares.Where(x => x.CurrentValue.HasValue).Select(x => x.CurrentValue.Value).ToList();

            var usedListCombined = usedValuesInRegion.Union(usedValuesInRow).Union(usedValuesInColumn).ToList();
            var possibleValues = SudokuConstants.PossibleValues;

            PossibleValues = new List<int>();
            NonPossibleValues = new List<int>();
            foreach(var item in  possibleValues)
            {
                if(usedListCombined.Contains(item))
                {
                    NonPossibleValues.Add(item);
                }
                else
                {
                    PossibleValues.Add(item);
                }
            }

            if(PossibleValues.Count == 0 && BoardRow == 1 && BoardColumn == 2)
            {
                Console.WriteLine("How in the sam hill did i get here?");
            }

            if(PossibleValues.Count == 1 && GlobalSettings.ShowHints)
            {
                CurrentState = SudokuState.OnePossibility;
            }
            else if (PossibleValues.Count == 0)
            {
                CurrentState = SudokuState.Error;
            }
            else
            {
                CurrentState = SudokuState.Unknown;
            }
        }

        public bool CanBeSet()
        {
            if(CurrentValue.HasValue)
            {
                // already set
                return false;
            }
            else if(PossibleValues.Count == 0) 
            {
                // no possible values
                return false;
            }
            else if(PossibleValues.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetToPossibleValue()
        {
            if (PossibleValues.Count == 0)
            {
                throw new InvalidOperationException($"No possible values found for {BoardRow} and {BoardColumn}");
            }
            else if (PossibleValues.Count == 1)
            {
                CurrentValue = PossibleValues.First();
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}