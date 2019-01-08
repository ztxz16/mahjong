using Emgu.CV;
using Emgu.CV.Structure;
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

namespace mahjong
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MyCard myCard = new MyCard();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetCard(MyCard myCard)
        {
            myCard.Clear();

            foreach (var c in tiao.Text)
            {
                if (c >= '1' && c <= '9')
                {
                    myCard.hand.colors[0].Insert((int)c - 48);
                }
            }

            foreach (var c in tong.Text)
            {
                if (c >= '1' && c <= '9')
                {
                    myCard.hand.colors[1].Insert((int)c - 48);
                }
            }

            foreach (var c in wan.Text)
            {
                if (c >= '1' && c <= '9')
                {
                    myCard.hand.colors[2].Insert((int)c - 48);
                }
            }
        }

        private void CalcHu(object sender, RoutedEventArgs e)
        {
            GetCard(myCard);
            if (myCard.CheckHu())
            {
                result.Text = "胡了";
            }
            else
            {
                result.Text = "没胡";
            }
        }

        private void CalcForgive(object sender, RoutedEventArgs e)
        {
            GetCard(myCard);

            Dictionary<KeyValuePair<int, int>, Ting> forgive = new Dictionary<KeyValuePair<int, int>, Ting>();
            myCard.CalcForgive(forgive);
            string resultString = "";
            foreach (var key in forgive.Keys)
            {
                resultString += String.Format("舍 {0}{1} : 听 ", key.Value, Util.names[key.Key]);
                for (int c = 0; c < 3; c++)
                {
                    foreach (int p in forgive[key].points[c])
                    {
                        resultString += String.Format("{0}{1} ", p, Util.names[c]);
                    }
                }

                resultString += "\n";
            }

            result.Text = resultString;
        }

        private void CalcTing(object sender, RoutedEventArgs e)
        {
            GetCard(myCard);

            int ans = 999;
            int limit = 3;
            myCard.GetToTing(0, limit, ref ans);
            if (ans > 100)
            {
                result.Text = String.Format("大于 {0} 向听", limit);
            }
            else
            {
                result.Text = String.Format("{0} 向听", ans);
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            tiao.Text = tong.Text = wan.Text = "";
        }

        private void qiwan_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (qiwan.Text.Count() > 0)
            {
                char c = qiwan.Text[0];
                for (int i = 0; i < wan.Text.Count(); i++)
                {
                    if (wan.Text[i] == c)
                    {
                        wan.Text = wan.Text.Remove(i, 1);
                        qiwan.Text = "";
                        return;
                    }
                }
            }

            qiwan.Text = "";
        }

        private void qitiao_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (qitiao.Text.Count() > 0)
            {
                char c = qitiao.Text[0];
                for (int i = 0; i < tiao.Text.Count(); i++)
                {
                    if (tiao.Text[i] == c)
                    {
                        tiao.Text = tiao.Text.Remove(i, 1);
                        qitiao.Text = "";
                        return;
                    }
                }
            }

            qitiao.Text = "";
        }

        private void qitong_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (qitong.Text.Count() > 0)
            {
                char c = qitong.Text[0];
                for (int i = 0; i < tong.Text.Count(); i++)
                {
                    if (tong.Text[i] == c)
                    {
                        tong.Text = tong.Text.Remove(i, 1);
                        qitong.Text = "";
                        return;
                    }
                }
            }

            qitong.Text = "";
        }

        private void GetImage(object sender, RoutedEventArgs e)
        {
            Util.GetScreen();
        }

        private void Ana(object sender, RoutedEventArgs e)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Util.CurrentPath + @"\screen.png");

            int channels = image.NumberOfChannels;
            int height = image.Height;
            int width = image.Width;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {

                }
            }
            screen.Source = ToBitmapSource(image);
        }

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                return bs;
            }
        }
    }
}
