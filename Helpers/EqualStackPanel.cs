using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BaHotHManager.Helpers
{
    public class EqualStackPanel : StackPanel
    {
        protected override Size ArrangeOverride(Size availableSize)
        {
            Point bottomRight;
            var x = 0.0;
            var y = 0.0;
            var proportionalWidth = availableSize.Width / Children.Count;
            var proportionalHeight = availableSize.Height / Children.Count;
            foreach (var child in Children)
            {
                var topLeft = new Point(x, y);
                if (Orientation == Orientation.Vertical)
                {
                    y += proportionalHeight;
                    bottomRight = new Point(availableSize.Width, y);
                }
                else
                {
                    x += proportionalWidth;
                    bottomRight = new Point(x, availableSize.Height);
                }
                child.Arrange(new Rect(topLeft, bottomRight));
            }
            return availableSize;
        }
    }
}
