using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace PomodoroTimeWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isWorkingTime;
        private TimeSpan workingTime = new TimeSpan(0, 0, 0, 0, 1500000);
        private TimeSpan breakingTime = new TimeSpan(0, 0, 0, 0, 300000);
        private Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            /*
             * Windowの表示位置をマニュアル指定
             */
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            /*
             * 表示位置(Top)を調整。
             * 「ディスプレイの作業領域の高さ」-「表示するWindowの高さ」
             */
            this.Top = SystemParameters.WorkArea.Height - this.Height;

            /*
             * 表示位置(Left)を調整
             * 「ディスプレイの作業領域の幅」-「表示するWindowの幅」
             */
            this.Left = SystemParameters.WorkArea.Width - this.Width;

            //25分を初期設定
            timerTextBlock.Text = workingTime.ToString(@"mm\:ss");

            //True：作業時間 False：休憩時間
            isWorkingTime = true;


            //定期的に実行
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,1);
            timer.Tick +=  Timer_Tick;
            timer.Start();
            
        }

        private void Timer_Tick(object sender,EventArgs e)
        {
            int countMax = isWorkingTime == true ? (int)workingTime.TotalSeconds : (int)breakingTime.TotalSeconds;
            int totalSecounds = (int)stopwatch.Elapsed.TotalSeconds;

            TimeSpan countTime = new TimeSpan(0, 0, (countMax - totalSecounds));
            timerTextBlock.Text = countTime.ToString(@"mm\:ss");
            if (countTime.TotalSeconds == 0)
            {
                TimerSetUp();
                timerTextBlock.Text = isWorkingTime == true ? workingTime.ToString(@"mm\:ss") : breakingTime.ToString(@"mm\:ss");
                stopwatch.Restart();
            } 
        }


        private void TimerSetUp()
        {
            if (isWorkingTime)
            {
                stopwatch.Stop();
                MessageBox.Show("作業をやめてください。", "作業終了", MessageBoxButton.OK, MessageBoxImage.Information);
                isWorkingTime=false;
            }
            else
            {
                stopwatch.Stop();
                MessageBox.Show("作業を始めてください。", "休憩終了", MessageBoxButton.OK, MessageBoxImage.Information);
                isWorkingTime = true;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void EndImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Application.Current.Shutdown();
            }
        }

        private void StartOrStop(object sender, RoutedEventArgs e)
        {
            //タイマーの状態を確認
            if (!stopwatch.IsRunning)
            {
                //タイマーを進める
                StartOrStopButton.Content = "Stop";
                stopwatch.Start();
                
            }
            else
            {
                //タイマーを止める
                StartOrStopButton.Content = "Start";
                stopwatch.Stop();
            }
        }

        private void Timer_Reset(object sender, RoutedEventArgs e)
        {
            stopwatch.Reset();
            isWorkingTime = true ;
            timerTextBlock.Text = workingTime.ToString(@"mm\:ss");
        }
    }
}
