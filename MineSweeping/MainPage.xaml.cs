using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MineSweeping.Models;

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

        public ObservableCollection<MineBlock> MineBlocks;

        public GridLength GridLength = new GridLength(PoolSize * _blockWidth);

        public MainPage()
        {
            this.InitializeComponent();

            MineBlocks = new ObservableCollection<MineBlock>();
            for(int i = 0; i < 100; i++)
            {
                MineBlocks.Add(new MineBlock() { CordX = 1, CordY = 1, isMine = true, MinesAround = 1 });
            }
            

        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame=Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
