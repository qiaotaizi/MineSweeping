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

        private static int _blockWidth = 50;

        //雷池长宽 同时也是地雷数量
        public static int PoolSize = 10;

        private static Random _mineGenerator = new Random();

        public ObservableCollection<MineBlock> MineBlocks;

        public GridLength GridLength = new GridLength(PoolSize * _blockWidth);

        private int[,] _mineArr;

        public MainPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            RefreshMineArr();
            MineBlocks = new ObservableCollection<MineBlock>();
            for (int i = 0; i < 100; i++)
            {
                MineBlock mine = new MineBlock();
                InitMineBlock(mine, i);
                MineBlocks.Add(mine);
            }
        }

        private void ReloadData()
        {
            RefreshMineArr();
            foreach (MineBlock block in MineBlocks)
            {
                ReinitMineBlock(block);
            }
        }

        private void ReinitMineBlock(MineBlock mine)
        {
            var isMine = false;
            for (int j = 0; j < PoolSize; j++)
            {
                if (mine.CordX == _mineArr[j, 0] && mine.CordY == _mineArr[j, 1])
                {
                    isMine = true;
                    break;
                }
            }
            mine.IsMine = isMine;
            if (isMine)
            {
                mine.MinesAround = 0;
                mine.ShowText = "x";
                mine.NumColor = MineBlock.NumColorArr[0];
            }
            else
            {
                var ma = 0;
                //计算周围数量
                for (var x = -1; x < 2; x++)
                {
                    for (var y = -1; y < 2; y++)
                    {
                        if (CordInMineArr(mine.CordX+x, mine.CordY+y))
                        {
                            ma++;
                        }
                    }
                }
                mine.MinesAround = ma;
                if (ma == 0)
                {
                    mine.ShowText = "";
                }
                else
                {
                    mine.ShowText = ma.ToString();
                }
                mine.NumColor = MineBlock.NumColorArr[ma];
            }
            mine.Color = MineBlock.COVERED;
            mine.ShowNum = Visibility.Collapsed;
        }

        //初始化块信息
        private void InitMineBlock(MineBlock mine, int i)
        {
            mine.CordX = i % PoolSize;
            mine.CordY = i / PoolSize;
            var isMine = false;
            for(int j = 0; j < PoolSize; j++)
            {
                if(mine.CordX==_mineArr[j,0] && mine.CordY == _mineArr[j, 1])
                {
                    isMine = true;
                    break;
                }
            }
            mine.IsMine = isMine;
            if (isMine)
            {
                mine.MinesAround = 0;
                mine.ShowText = "x";
                mine.NumColor = MineBlock.NumColorArr[0];
            }
            else
            {
                var ma = 0;
                //计算周围数量
                for (var x=-1;x<2;x++)
                {
                    for (var y=-1;y<2;y++)
                    {
                        if (CordInMineArr(mine.CordX+x, mine.CordY+y))
                        {
                            ma++;
                        }
                    }
                }
                mine.MinesAround = ma;
                if (ma == 0)
                {
                    mine.ShowText = "";
                }
                else
                {
                    mine.ShowText = ma.ToString();
                }
                mine.NumColor = MineBlock.NumColorArr[ma];
            }
            mine.Color = MineBlock.COVERED;
            Debug.WriteLine("Mine="+mine.CordX+","+mine.CordY+","+mine.IsMine+","+mine.MinesAround);
        }

        private bool CordInMineArr(int x, int y)
        {
            for(var i = 0; i < PoolSize; i++)
            {
                if (_mineArr[i,0]==x && _mineArr[i,1]==y)
                {
                    return true;
                }
            }
            return false;
        }

        //刷新/重置数组
        private void RefreshMineArr()
        {
            _mineArr = new int[PoolSize, 2];
            for (int i = 0; i < PoolSize;)
            {
                var randX = _mineGenerator.Next(0,PoolSize-1);
                var randY = _mineGenerator.Next(0,PoolSize-1);
                bool foundSameCord = false;
                //向前遍历
                for (int j = 0; j < i; j++)
                {
                    if(randX==_mineArr[j,0] && randY == _mineArr[j, 1])
                    {
                        //生成的两次位置相同,重新随机位置
                        foundSameCord = true;
                        break;
                    }
                }
                if (foundSameCord)
                {
                    continue;
                }
                _mineArr[i, 0] = randX;
                _mineArr[i, 1] = randY;
                Debug.WriteLine(i+"索引("+randX+","+randY+")");
                i++;
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadData();
        }

        //单击打开
        private void MineBlock_Click(object sender, ItemClickEventArgs e)
        {
            var clicked=e.ClickedItem as MineBlock;
            clicked.ShowNum = Visibility.Visible;
            clicked.Color = MineBlock.DISCOVERED;
            //如果非雷且数字为0
            //扩展周围显示
        }
    }
}
