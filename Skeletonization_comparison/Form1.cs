using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skeletonization_comparison
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            Image img = Image.FromFile(filename);
            double[,] map = get_base_map(img);
        }
        public static double[,] get_base_map(Image img)//запись массива насыщенности в переменную 
        {
            Bitmap b = new Bitmap(img);
            double[,] map = new double[b.Height, b.Width];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Color color = b.GetPixel(j, i);
                    map[i, j] = color.GetBrightness()<0.5 ? 1 : 0;
                }
            }
            return map;
        }
        public static double[,] sk_zang_s(double[,] a)//запись массива насыщенности в переменную 
        {
             
        }
        static double[] get_environment(double[,] a, int i, int j)//получение окружения
        {
            return new double[8] { a[i - 1, j] ,
                a[i - 1, j + 1] ,
                a[i, j + 1] ,
                a[i + 1, j + 1] ,
                a[i + 1, j] ,
                a[i + 1, j - 1] ,
                a[i, j - 1] ,
                a[i - 1, j - 1] };

        }
    }
}
