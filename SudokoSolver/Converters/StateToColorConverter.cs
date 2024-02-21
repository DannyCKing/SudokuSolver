using SudokoSolver.Settings;
using SudokoSolver.SudokuModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SudokoSolver.Converters
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // default result
            Color result = Colors.White;

            try
            {
                var currentState = (SudokuState)value;
                if(currentState == SudokuState.Error)
                {
                    result = Colors.Red;
                }
                else if (currentState == SudokuState.Default)
                {
                    result = Colors.Black;
                }
                else if (currentState == SudokuState.Set)
                {
                    result = Colors.LightGreen;
                }
                else if(currentState == SudokuState.OnePossibility && GlobalSettings.ShowHints)
                {
                    result = Colors.Yellow;
                }
                else if (currentState == SudokuState.LockedIn)
                {
                    result = Colors.Green;
                }
            }
            catch(Exception)
            {
            }

            return new SolidColorBrush(result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
