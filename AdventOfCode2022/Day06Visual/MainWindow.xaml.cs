using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Day06Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Label[] L = new Label[38];
        private DispatcherTimer dispatcherTimer;
        string signal = "mjqjpqmgbljsphdztnvjfqwrcgsmlb";
        int counter = 0;
        int ct1, ct2;
        int size = 10;
        bool grow = true;
        bool fast = true;

        public MainWindow()
        {
            InitializeComponent();
            L[0] = l01;
            L[1] = l02;
            L[2] = l03;
            L[3] = l04;
            L[4] = l05;
            L[5] = l06;
            L[6] = l07;
            L[7] = l08;
            L[8] = l09;
            L[9] = l10;
            L[10] = l11;
            L[11] = l12;
            L[12] = l13;
            L[13] = l14;
            L[14] = l15;
            L[15] = l16;                    
            L[16] = l17;
            L[17] = l18;
            L[18] = l19;
            L[19] = l20;
            L[20] = l21;
            L[21] = l22;
            L[22] = l23;
            L[23] = l24;
            L[24] = l25;
            L[25] = l26;
            L[26] = l27;
            L[27] = l28;
            L[28] = l29;
            L[29] = l30;
            L[30] = l31;
            L[31] = l32;
            L[32] = l33;
            L[33] = l34;
            L[34] = l35;
            L[35] = l36;
            L[36] = l37;
            L[37] = l38;

            foreach (var l in L)
            {
                l.FontFamily = new FontFamily("Comic Sans");
            }
            //  DispatcherTimer setup
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0,0,0,1,0);
            dispatcherTimer.Start();
            var inputfile = @"E:\develop\advent-of-code-input\2022\day06.txt";
            if (File.Exists(inputfile))
                signal = File.ReadAllLines(inputfile).First();
            (ct1, ct2) = PuzzleDay06(signal);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            for (int i = 0; i < L.Length -1;i++)
            {
                L[i].Content = L[i + 1].Content;
                L[i].FontStyle = L[i + 1].FontStyle;
                L[i].Foreground = L[i + 1].Foreground;
                L[i].FontSize = L[i + 1].FontSize;
            }
            L[^1].Content = signal[counter];
            if (counter >= ct1 - 3 && counter <= ct1)
            {
                L[^1].FontStyle = FontStyles.Oblique;
                L[^1].Foreground = Brushes.White;
                L[^1].FontSize = 30;
            }
            else if (counter >= ct2 - 13 && counter <= ct2)
            {
                L[^1].FontStyle = FontStyles.Oblique;
                L[^1].Foreground = Brushes.LightGreen;
                L[^1].FontSize = 30;
            }
            else
            {
                L[^1].FontStyle = FontStyles.Italic;
                L[^1].Foreground = Brushes.Black;
                if (grow)
                {
                    size += 2;
                    grow = size < 26;
                }
                else
                {
                    size -= 2;
                    grow = size < 10;
                }
                L[^1].FontSize = size;
            }

            if (counter == ct1 + L.Length)
            {
                lblSOP.Content = signal.Substring(ct1 - 3, 4) + " at #" + ct1;
            }
            if (counter == ct2 + L.Length)
            {
                lblMSG.Content = signal.Substring(ct2 - 13, 14) + " at #" + ct2;
            }

            counter++;
            if (counter >= signal.Length) counter = 0;
            if (counter > ct1 && counter < 40 + ct1 || counter > ct2 && counter < ct2 + 40)
            {
                if (fast)
                {
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100, 0);
                    fast = false;
                }
            }
            else if (!fast)
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1, 0);
                fast = true;
            }
            lblCount.Content = counter;
            dispatcherTimer.Start();
            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private static (int, int) PuzzleDay06(string T)
        {
            int answer1 = 0;
            int answer2 = 0;

            int k = -1;
            for (int i = 3; i < T.Length; i++)
            {
                string s = "";
                for (int j = i - 3; j <= i; j++)
                {
                    if (s.Contains(T[j])) break;
                    s = s + T[j];
                }
                if (s.Length == 4)
                {
                    k = i + 1;
                    break;
                }
            }
            answer1 = k;
         
            k = -1;
            for (int i = 13; i < T.Length; i++)
            {
                string s = "";
                for (int j = i - 13; j <= i; j++)
                {
                    if (s.Contains(T[j])) break;
                    s = s + T[j];
                }
                if (s.Length == 14)
                {
                    k = i + 1;
                    break;
                }
            }
            answer2 = k;
            return (answer1,answer2);
        }
    }
}
