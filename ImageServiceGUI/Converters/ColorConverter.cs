using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.Converters
{
    class ColorConverter : IValueConverter
    {
		// Coupling logging message with appropriate background.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            int type = (int) value;
            if (type == (int) MessageTypeEnum.INFO)
                return Brushes.Green;
            else if (type == (int)MessageTypeEnum.WARNING)
                return Brushes.Yellow;
            else
                return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
