using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmsImplementation1.Model
{
    public class Pixel
    {
        public Rectangle m_PixelValue { get; set; }

        // Construtor vazio
        public Pixel() : this((Color)ColorConverter.ConvertFromString("#000000")) { }

        // Construtor
        public Pixel(Color p_Color)
        {
            m_PixelValue = new Rectangle();
            m_PixelValue.Height = 1;
            m_PixelValue.Width = 1;
            m_PixelValue.Fill = new SolidColorBrush(p_Color);
        }

     
    }
}
