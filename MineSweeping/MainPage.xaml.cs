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
        //雷池长宽,默认10
        public static int PoolSize = 10;

        //地雷数量,默认30
        public static int MineCount = 30;

        //地雷随机位置生成器
        private static Random _mineGenerator = new Random();

        //mvvm集合
        public ObservableCollection<MineBlock> MineBlocks;

        //地雷坐标数组
        private int[,] _mineArr;

        private int _mineMarked = 0;

        //初始化方法
        public MainPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        //加载雷池数据
        private void LoadData()
        {
            RefreshMineArr();
            MineBlocks = new ObservableCollection<MineBlock>();
            FillMineBlocks();
        }

        private void FillMineBlocks()
        {

            for (int i = 0; i < PoolSize * PoolSize; i++)
            {
                MineBlock mine = new MineBlock();
                InitMineBlock(mine, i);
                MineBlocks.Add(mine);
            }
        }



        //重新加载雷池数据
        private void ReloadData()
        {
            RefreshMineArr();
            _mineMarked = 0;
            MineBlocks.Clear();
            FillMineBlocks();
        }

        //重新初始化地雷块
        private void ReinitMineBlock(MineBlock mine)
        {
            var isMine = false;
            for (int j = 0; j < MineCount; j++)
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
                mine.ShowText = "※";
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
                        if (CordInMineArr(mine.CordX + x, mine.CordY + y))
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
        }

        //初始化块信息
        private void InitMineBlock(MineBlock mine, int i)
        {
            mine.CordX = i % PoolSize;
            mine.CordY = i / PoolSize;

            //复用重新加载地雷块的逻辑
            ReinitMineBlock(mine);

        }

        //判断给定坐标是否是地雷
        private bool CordInMineArr(int x, int y)
        {
            for (var i = 0; i < MineCount; i++)
            {
                if (_mineArr[i, 0] == x && _mineArr[i, 1] == y)
                {
                    return true;
                }
            }
            return false;
        }

        //刷新/重置地雷坐标数组
        private void RefreshMineArr()
        {
            _mineArr = new int[MineCount, 2];
            for (int i = 0; i < MineCount;)
            {
                var randX = _mineGenerator.Next(0, PoolSize - 1);
                var randY = _mineGenerator.Next(0, PoolSize - 1);
                bool foundSameCord = false;
                //向前遍历
                for (int j = 0; j < i; j++)
                {
                    if (randX == _mineArr[j, 0] && randY == _mineArr[j, 1])
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
                i++;
            }
        }

        //重新开始点击事件
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem selected = sender as MenuFlyoutItem;
            if (selected != null)
            {
                var option = selected.Tag.ToString();
                switch (option)
                {
                    case "five":
                        PoolSize = 5;
                        MineCount = 10;
                        MineGridView.ItemsPanel = Resources["PanelTemplateFive"] as ItemsPanelTemplate;
                        break;
                    case "ten":
                        PoolSize = 10;
                        MineCount = 30;
                        MineGridView.ItemsPanel = Resources["PanelTemplateTen"] as ItemsPanelTemplate;
                        break;
                    case "fifteen":
                        PoolSize = 15;
                        MineCount = 50;
                        MineGridView.ItemsPanel = Resources["PanelTemplateFifteen"] as ItemsPanelTemplate;
                        break;
                }

                ReloadData();
            }
        }

        //单机地雷事件
        private void MineBlock_Click(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as MineBlock;
            ShowMineBlock(clicked);

        }

        //递归显示被打开的地雷块
        private void ShowMineBlock(MineBlock mine)
        {
            //如果已经被打开,则直接return
            if (mine.ShowNum == Visibility.Visible)
            {
                return;
            }
            //如果右击状态非0,直接return
            if (mine.RightClickStatus != 0)
            {
                return;
            }
            mine.ShowNum = Visibility.Visible;
            mine.Color = MineBlock.DISCOVERED;
            //如果是地雷,终止游戏,显示对话框
            if (mine.IsMine)
            {
                ShowAll();
                GameOver();
                return;
            }
            if (mine.MinesAround > 0)
            {
                return;
            }
            //空白块
            //递归扩展周围显示
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    //获取周围8个方块的坐标
                    var cordXCurr = mine.CordX + x;
                    var cordYCurr = mine.CordY + y;
                    MineBlock blockCurr = BlockByCord(cordXCurr, cordYCurr);
                    if (blockCurr != null)
                    {
                        //逻辑上,这里不可能遍历到是地雷的方块
                        //只可能是数字方块
                        //所以不会打开地雷
                        ShowMineBlock(blockCurr);
                    }
                }
            }
        }

        //根据坐标获得雷池中的方块
        private MineBlock BlockByCord(int cordXCurr, int cordYCurr)
        {
            //判定坐标是否出界
            if (cordXCurr < 0 || cordXCurr >= PoolSize)
            {
                return null;
            }
            if (cordYCurr < 0 || cordYCurr >= PoolSize)
            {
                return null;
            }
            //index/PoolSize=CordY...CordX
            //index=PoolSize*CordY+CordX
            int index = PoolSize * cordYCurr + cordXCurr;
            if (index < 0 || index >= MineBlocks.Count)
            {
                return null;
            }
            //返回方块
            return MineBlocks[index];
        }

        //游戏失败-结束逻辑
        private async void GameOver()
        {
            var gameOverDialog = new GameOverDialog();
            var result = await gameOverDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                //再来一局
                ReloadData();
            }
        }

        //右击事件
        private void MineBlock_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //这个地方原本写的是e.OriginalSource as Grid
            //但有时类型转换得到的是null
            //这是因为有些时候OriginalSource 是 ListViewItemPresenter
            //所以使用FrameworkElement替代之
            var selected = (e.OriginalSource as FrameworkElement)?.DataContext as MineBlock;
            //非空校验
            if (selected == null)
            {
                return;
            }
            //如果已经被打开,右击失效
            if (selected.ShowNum == Visibility.Visible)
            {
                return;
            }
            //第一次右击
            //标为星号
            //同时检测是否所有地雷被标为星号,若是,游戏胜利,显示对话框
            if (selected.RightClickStatus == 0)
            {
                selected.ShowRightClick = Visibility.Visible;
                selected.RightClickShowText = "*";
                selected.RightClickStatus = 1;
                _mineMarked++;
                if (AllMineSweeped())
                {
                    ShowAll();
                    GameSuccess();
                    return;

                }
                return;
            }
            //第二次右击
            //标为问号
            if (selected.RightClickStatus == 1)
            {
                selected.RightClickShowText = "?";
                selected.RightClickStatus = 2;
                _mineMarked--;
                return;
            }
            //第三次右击
            //消除问号
            if (selected.RightClickStatus == 2)
            {
                selected.ShowRightClick = Visibility.Collapsed;
                selected.RightClickShowText = "";
                selected.RightClickStatus = 0;
                return;
            }
        }


        //揭示答案
        private void ShowAll()
        {
            foreach (MineBlock block in MineBlocks)
            {
                block.ShowNum = Visibility.Visible;
                block.Color = MineBlock.DISCOVERED;
                //被标记过的特殊处理
                if (block.RightClickStatus == 1)
                {
                    block.ShowRightClick = Visibility.Visible;
                    if (block.IsMine)
                    {
                        block.RightClickShowText = "√";
                    }
                    else
                    {
                        block.RightClickShowText = "×";
                    }
                }
            }
        }

        //是否所有的地雷块都被标记
        private bool AllMineSweeped()
        {
            if (_mineMarked != MineCount)
            {
                return false;
            }
            for (int i = 0; i < MineCount; i++)
            {

                var mineBlock = BlockByCord(_mineArr[i, 0], _mineArr[i, 1]);
                if (mineBlock.RightClickStatus != 1)
                {
                    //未被标记为地雷
                    return false;
                }
            }

            return true;
        }

        //游戏胜利
        private async void GameSuccess()
        {
            var successDialog = new SuccessDialog();
            var result = await successDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ReloadData();
            }
        }
    }
}
