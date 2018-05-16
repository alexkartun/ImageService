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
            MessageTypeEnum type = (MessageTypeEnum) value;
            if (type == MessageTypeEnum.INFO)
                return "Green";
            else if (type == MessageTypeEnum.WARNING)
                return "Yellow";
            else
                return "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
