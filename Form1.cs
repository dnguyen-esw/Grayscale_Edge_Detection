using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLA_Project07
{
    public partial class Form1 : Form
    {
        Bitmap hinhgoc;
        Bitmap hinhgray;
        public Form1()
        {
            InitializeComponent();
            hinhgoc = new Bitmap(@"C:\Users\hoang\OneDrive\Máy tính\Image Processing\C#\lena_color.gif");
            pictureBox_hinhgoc.Image = hinhgoc;
            textBox_nguong.Text = "130";
            hinhgray = Gray_Luminance(hinhgoc);
        }
        
        public Bitmap Gray_Lightness(Bitmap hinhgoc)
        {
            Bitmap Hinhmucxam = new Bitmap(hinhgoc.Width, hinhgoc.Height);
            for (int x = 0; x < hinhgoc.Width; x++)
            {
                for (int y = 0; y < hinhgoc.Height; y++)
                {
                    Color pixel = hinhgoc.GetPixel(x, y);
                    byte R = pixel.R;
                    byte G = pixel.G;
                    byte B = pixel.B;

                    byte max = Math.Max(R, Math.Max(G, B));
                    byte min = Math.Min(R, Math.Min(G, B));
                    byte gray = (byte)((max + min) / 2);
                    Hinhmucxam.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return Hinhmucxam;
        }
        public Bitmap Gray_Average(Bitmap hinhgoc)
        {
            Bitmap Hinhmucxam = new Bitmap(hinhgoc.Width, hinhgoc.Height);
            for (int x = 0; x < hinhgoc.Width; x++)
            {
                for (int y = 0; y < hinhgoc.Height; y++)
                {
                    Color pixel = hinhgoc.GetPixel(x, y);
                    byte R = pixel.R;
                    byte G = pixel.G;
                    byte B = pixel.B;

                    byte gray = (byte)((R + G + B) / 3);
                    Hinhmucxam.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return Hinhmucxam;
        }
        public Bitmap Gray_Luminance(Bitmap hinhgoc)
        {
            Bitmap Hinhmucxam = new Bitmap(hinhgoc.Width, hinhgoc.Height);
            for (int x = 0; x < hinhgoc.Width; x++)
            {
                for (int y = 0; y < hinhgoc.Height; y++)
                {
                    Color pixel = hinhgoc.GetPixel(x, y);
                    byte R = pixel.R;
                    byte G = pixel.G;
                    byte B = pixel.B;

                    byte gray = (byte)(0.2126 * R + 0.7152 * G + 0.0722 * B);
                    Hinhmucxam.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return Hinhmucxam;
        }

        public Bitmap Edge_Detect_Sobel(Bitmap hinhxam, int D0)
        {
            //sobel gx
            int[,] mask_x = new int[3, 3];
            mask_x[0, 0] = -1;      mask_x[1, 0] = -2;      mask_x[2, 0] = -1;
            mask_x[0, 1] =  0;      mask_x[1, 1] =  0;      mask_x[2, 1] =  0;
            mask_x[0, 2] =  1;      mask_x[1, 2] =  2;      mask_x[2, 2] =  1;
            //sobel gy
            int[,] mask_y = new int[3, 3];
            mask_y[0, 0] = -1;      mask_y[1, 0] = 0;       mask_y[2, 0] = 1;
            mask_y[0, 1] = -2;      mask_y[1, 1] = 0;       mask_y[2, 1] = 2;
            mask_y[0, 2] = -1;      mask_y[1, 2] = 0;       mask_y[2, 2] = 1;

            Bitmap edge = new Bitmap(hinhxam.Width, hinhxam.Height);

            for (int x = 1; x < hinhxam.Width - 1; x++)
                for (int y = 1; y < hinhxam.Height - 1; y++)
                {
                    double gx = 0;
                    double gy = 0;
                    double Mxy = 0;
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            Color color = hinhxam.GetPixel(i, j);
                            byte value = color.R;
                            gx += value * mask_x[i - (x - 1), j - (y - 1)];//tích chập ma trận mask_x và ma trận 3x3 giá trị các điểm ảnh lân cận f(x,y)
                            gy += value * mask_y[i - (x - 1), j - (y - 1)];//tích chập ma trận mask_y và ma trận 3x3 giá trị các điểm ảnh lân cận f(x,y)
                        }
                    Mxy = Math.Abs(gx) + Math.Abs(gy);

                    if (Mxy<=D0)
                        edge.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else
                        edge.SetPixel(x, y, Color.FromArgb(255,255,255));
                }

            return edge;
        }
        public Bitmap Edge_Detect_Prewitt(Bitmap hinhxam, int D0)
        {
            //prewitt gx
            int[,] mask_x = new int[3, 3];
            mask_x[0, 0] = -1; mask_x[1, 0] = -1; mask_x[2, 0] = -1;
            mask_x[0, 1] = 0; mask_x[1, 1] = 0; mask_x[2, 1] = 0;
            mask_x[0, 2] = 1; mask_x[1, 2] = 1; mask_x[2, 2] = 1;
            //prewitt gy
            int[,] mask_y = new int[3, 3];
            mask_y[0, 0] = -1; mask_y[1, 0] = 0; mask_y[2, 0] = 1;
            mask_y[0, 1] = -1; mask_y[1, 1] = 0; mask_y[2, 1] = 1;
            mask_y[0, 2] = -1; mask_y[1, 2] = 0; mask_y[2, 2] = 1;

            Bitmap edge = new Bitmap(hinhxam.Width, hinhxam.Height);

            for (int x = 1; x < hinhxam.Width - 1; x++)
                for (int y = 1; y < hinhxam.Height - 1; y++)
                {
                    double gx = 0;
                    double gy = 0;
                    double Mxy = 0;
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            Color color = hinhxam.GetPixel(i, j);
                            byte value = color.R;
                            gx += value * mask_x[i - (x - 1), j - (y - 1)];
                            gy += value * mask_y[i - (x - 1), j - (y - 1)];
                        }
                    Mxy = Math.Abs(gx) + Math.Abs(gy);

                    if (Mxy <= D0)
                        edge.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else
                        edge.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                }

            return edge;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox_edge.Image = Edge_Detect_Sobel(hinhgray, Convert.ToInt32(textBox_nguong.Text));
        }

        private void button_prewitt_Click(object sender, EventArgs e)
        {
            pictureBox_edge.Image = Edge_Detect_Prewitt(hinhgray, Convert.ToInt32(textBox_nguong.Text));
        }
    }
}
