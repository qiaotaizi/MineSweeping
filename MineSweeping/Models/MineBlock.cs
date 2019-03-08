using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MineSweeping.Models
{
    public class MineBlock : INotifyPropertyChanged
    {

        public static SolidColorBrush COVERED = new SolidColorBrush(Colors.Gray);
        public static SolidColorBrush DISCOVERED = new SolidColorBrush(Colors.LightGray);

        public static SolidColorBrush[] NumColorArr = {
            new SolidColorBrush(Colors.Black),
        new SolidColorBrush(Colors.Green),
        new SolidColorBrush(Colors.Orange),
        new SolidColorBrush(Colors.Blue),
        new SolidColorBrush(Colors.Red),
        new SolidColorBrush(Colors.Purple),
        new SolidColorBrush(Colors.AliceBlue),
       new SolidColorBrush(Colors.Pink),
        new SolidColorBrush(Colors.Black),
    };
        public int CordX { get; set; }
        public int CordY { get; set; }

        private string _showText;
        public string ShowText
        {
            get => _showText;
            set
            {
                _showText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowText"));
            }
        }
        private bool _isMine;
        public bool IsMine
        {
            get => _isMine;
            set
            {
                _isMine = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsMine"));
            }
        }

        private int _minesAround;
        public int MinesAround
        {
            get => _minesAround;
            set
            {
                _minesAround = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MinesAround"));
            }
        }

        private Visibility _showNum=Visibility.Collapsed;
        public Visibility ShowNum
        {
            get => _showNum;
            set
            {
                _showNum = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowNum"));
            }
        }

        private Brush _color;
        public Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        private Brush _numColor;
        public Brush NumColor
        {
            get => _numColor;
            set
            {
                _numColor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NumColor"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
