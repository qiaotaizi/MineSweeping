using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MineSweeping
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static int _blockWidth=50;

        public static int PoolSize = 10;

        public GridLength GridLength = new GridLength(PoolSize * _blockWidth);

        public MainPage()
        {
            this.InitializeComponent();
            GridLength blockWid = new GridLength(_blockWidth);
            for (var i = 0; i < PoolSize; i++)
            {
                MinePoolGrid.RowDefinitions.Add(new RowDefinition() {Height=blockWid });
                MinePoolGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width=blockWid});
            }

            for(var row = 0; row < PoolSize; row++)
            {
                for(var col = 0; col < PoolSize; col++)
                {
                    Button b = new Button() {Width=_blockWidth,Height=_blockWidth };
                    Grid.SetRow(b, row);
                    Grid.SetColumn(b, col);
                    b.Margin = new Thickness(1);
                    b.Click += MineButton_Click;
                    MinePoolGrid.Children.Add(b);

                }
            }
        }

        private void MineButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("出发了点击事件");
        }
    }
}
