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
        //方块覆盖时的颜色常量
        public static SolidColorBrush COVERED = new SolidColorBrush(Colors.Gray);
        //方块打开时的颜色常量
        public static SolidColorBrush DISCOVERED = new SolidColorBrush(Colors.LightGray);

        //数字-颜色对应关系数组
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

        //横轴坐标
        public int CordX { get; set; }

        //纵轴坐标
        public int CordY { get; set; }

        //显示文字
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

        //是否为地雷
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

        //周边地雷数
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

        //是否显示数字
        private Visibility _showNum = Visibility.Collapsed;
        public Visibility ShowNum
        {
            get => _showNum;
            set
            {
                _showNum = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowNum"));
            }
        }

        //方块颜色
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

        //数字颜色
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

        private int _rightClickStatus=0;
        public int RightClickStatus
        {
            get => _rightClickStatus;
            set
            {
                _rightClickStatus = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RightClickStatus"));
            }
        }

        //是否显示右键标记
        private Visibility _showRightClick=Visibility.Collapsed;
        public Visibility ShowRightClick
        {
            get => _showRightClick;
            set
            {
                _showRightClick = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowRightClick"));
            }
        }

        //右键标记显示字符
        private string _rightClickShowText;
        public string RightClickShowText
        {
            get => _rightClickShowText;
            set
            {
                _rightClickShowText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RightClickShowText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
