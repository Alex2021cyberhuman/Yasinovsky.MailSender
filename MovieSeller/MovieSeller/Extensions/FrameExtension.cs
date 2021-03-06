using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MovieSeller.Extensions
{
    public static class FrameExtension
    {
        public static void RemoveHistory(this Frame frame)
        {
            while (frame.CanGoBack)
            {
                frame.RemoveBackEntry();
            }

        }
    }
}
