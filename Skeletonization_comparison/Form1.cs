﻿using System;
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
            string s = "";
            double [,] f= get_base_map(img);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    s += "" + map[i, j];
                }
                s += "\n";
            }
            int p = 0;
            bool t = true;
            skeletization4(ref map);
            /*while ()//sk_zang_s(ref map, f, t)) 
            {
                t = !t;
                //if (p++ % 1 != 0) continue; 
                for(int i = 0; i < map.GetLength(0); i++)
                {
                    for(int j = 0; j < map.GetLength(1); j++)
                    {
                        s += "" + map[i, j];
                        f[i, j] = map[i, j];
                    }
                    s += "\n";
                }
            }*/
            int a = 0;
        }
        public static double[,] get_base_map(Image img)//запись массива насыщенности в переменную 
        {
            Bitmap b = new Bitmap(img);
            double[,] map = new double[b.Height+2, b.Width+2];
            for (int i = 0; i < map.GetLength(0)-2; i++)
            {
                for (int j = 0; j < map.GetLength(1)-2; j++)
                {
                    Color color = b.GetPixel(j, i);
                    map[i+1, j+1] = color.GetBrightness()<0.5 ? 1 : 0;
                }
            }
            return map;
        }
        public static bool sk_zang_s(ref double[,] a, double[,] b, bool f)//запись массива насыщенности в переменную 
        {
            bool res = false;
            for (int i = 1; i < a.GetLength(0)-1; i++)
            {
                for(int j = 1; j < a.GetLength(1)-1; j++)
                {
                    if (b[i, j] == 0) continue;
                    if (f == true)
                    {
                        double[] environment = get_environment(b, i, j);
                        int sm = 0;
                        int p = 0;
                        bool del = false;
                        for (int t = 0; t < 7; t++)
                        {
                            if (environment[t] == 0 && environment[t + 1] == 1) p++;
                            sm += (int)environment[t];
                        }
                        if (environment[7] == 0 && environment[0] >= 1) p++;
                        sm += (int)environment[7];
                        if (2 <= sm && sm <= 6)
                        {
                            if (p == 1)
                            {
                                if ((environment[0] * environment[2] * environment[4] == 0 && environment[2] * environment[4] * environment[6] == 0) 
                                     )
                                {
                                    del = true;
                                }
                            }
                        }
                        if (del)
                        {
                            res = true;
                            a[i, j] = 0;
                        }
                    }
                    else
                    {
                        double[] environment = get_environment(b, i, j);
                        int sm = 0;
                        int p = 0;
                        bool del = false;
                        for (int t = 0; t < 7; t++)
                        {
                            if (environment[t] == 0 && environment[t + 1] == 1) p++;
                            sm += (int)environment[t];
                        }
                        if (environment[7] == 0 && environment[0] >= 1) p++;
                        sm += (int)environment[7];
                        if (2 <= sm && sm <= 6)
                        {
                            if (p == 1)
                            {
                                if ( 
                                    (environment[0] * environment[2] * environment[6] == 0 && environment[0] * environment[4] * environment[6] == 0))
                                {
                                    del = true;
                                }
                            }
                        }
                        if (del)
                        {
                            res = true;
                            a[i, j] = 0;
                        }
                    }
                }
            }
            return res;
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

        public static void skeletization4(ref double[,] sq)
        {
            double[,] sf = drying_out(sq);
            double[,] f = drying_out(sq);
            double max = mx(sf);
            string s="";
            while (del_min(ref sf, max, f)) 
                { 
                    //if (p++ % 1 != 0) continue; 
                    for (int i = 0; i < sf.GetLength(0); i++)
                    {
                        for (int j = 0; j < sf.GetLength(1); j++)
                        {
                            s += "" + (sf[i, j]>0?1:0); 
                            f[i,j]=sf[i,j];
                        }
                        s += "\n";
                    }
                }
            sq = sf;
        }
        static double[,] drying_out(double[,] a)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] >= 1)
                    {
                        int n = 0;
                        bool f = true;
                        while (f)
                        {
                            if (a[i, j + n] == 0 || a[i, j - n] == 0)
                            {
                                res[i, j] = n;
                                f = false;
                            }
                            n++;
                        }
                    }
                }
            }
            return res;

        }
        static double mx(double[,] a)//поиск максимума, модуль 4
        {
            double max = a[0, 0];
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] > max)
                    {
                        max = a[i, j];
                    }
                }
            }
            return max;
        }
        static bool del_min(ref double[,] a, double max, double[,] b)//удалить минимум, модуль 4
        {
            double min = max;
            bool res = false;
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] < min && a[i, j] != 0)
                    {
                        min = a[i, j];
                    }
                }
            }
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                { 
                    if (b[i, j] <= min && b[i, j] != 0)
                    {
                        double[] environment = get_environment(b, i, j);
                        int sm = 0;
                        int p = 0;

                        if (environment[7] == 0 && environment[0] >= 1) p++;
                        sm += environment[7] > 0 ? 1 : 0;
                        for (int t = 0; t < 7; t++)
                        {
                            environment[t] = environment[t] > 0 ? 1 : 0;
                            if (environment[t] == 0 && environment[t + 1] >= 1) p++;
                            sm += environment[t] > 0 ? 1 : 0;
                            if (p > 2)
                                break;
                        }

                        if (sm > 2)
                        {
                            bool del = false;
                            if (p == 1)
                            {
                                for (int t = 0; t < 6; t++)
                                {
                                    if (t % 2 == 0 && environment[t] == 1 && (environment[t + 1] == 1))
                                    {
                                        del = true;
                                        break;
                                    }
                                    else if (t % 2 != 0 && environment[t] == 1 && environment[t + 1] == 1)
                                    {
                                        del = true;
                                        break;
                                    }
                                }
                                if (!del)
                                {
                                    if ((environment[6] == 1 && environment[7] == 1) ||
                                       (environment[7] == 1 && environment[0] == 1))
                                    {
                                        del = true;
                                    }
                                }
                            }  
                            if (del)
                            {
                                res = true;
                                a[i, j] = 0;
                            }
                            else
                            {
                                a[i, j]++;
                            }

                        }
                        else
                        {
                            a[i, j]++;
                            res = true;
                        }
                    }

                }
            }
            return res;
        }
    }
}
