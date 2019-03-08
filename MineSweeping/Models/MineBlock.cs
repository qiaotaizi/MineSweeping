using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MineSweeping.Models
{
    public class MineBlock
    {

        public static SolidColorBrush IS_NOT_MINE = new SolidColorBrush(Colors.LightGray);
        public static SolidColorBrush IS_MINE = new SolidColorBrush(Colors.Red);

        public int CordX { get; set; }
        public int CordY { get; set; }
        public bool IsMine { get; set; }
        public int MinesAround { get; set; }
        public Brush Color { get; set; }
    }
}
