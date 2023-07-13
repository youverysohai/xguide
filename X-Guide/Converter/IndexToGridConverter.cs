using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace X_Guide.Converter
{
    public class IndexToGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                int columns = 3; // Number of columns in the grid
                int row = index / columns;
                int column = index % columns;
                return new GridPosition(row, column);
            }

            return new GridPosition(0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GridPosition
    {
        public int Row { get; }
        public int Column { get; }

        public GridPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

}
