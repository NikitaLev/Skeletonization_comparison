using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            int x=1;
            pictureBox1.Image = new Bitmap(img, new Size(img.Width * x, img.Height * x));
            double[,] r1, r2, r3, r4, r5;
            //sk3_0(map); //sk2_0(map);// sk_Zang(map);// 
            //int start, end;
            Image i2, i3, i4, i5;
            long p2, p3, p4, p5;

            /*start = Environment.TickCount;
           r2 = skeletization4(map);
           end = Environment.TickCount;
           p2 = end - start;
           i2 = gen_bitm(r2, x);

           start = Environment.TickCount;
           r3 = sk_Zang(map);
           end = Environment.TickCount;
           p3 = end - start;
           i3 = gen_bitm(r3, x);


           start = Environment.TickCount;
           r4 = sk2_0(map);
           end = Environment.TickCount;
           p4 = end - start;
           i4 = gen_bitm(r4, x);
           */
            Stopwatch watch =  Stopwatch.StartNew(); 
            watch.Stop();


            watch.Restart();// = //Stopwatch.StartNew();
            r2 = skeletization4(map); 
            p2 = watch.ElapsedMilliseconds;
            i2 = gen_bitm(r2, x);

            watch.Restart();//.Start();
            r3 = sk_Zang(map);
            watch.Stop();
            p3 = watch.ElapsedMilliseconds;
            i3 = gen_bitm(r3, x);


            watch.Restart();
            r4 = sk2_0(map); 
            p4 = watch.ElapsedMilliseconds;
            i4 = gen_bitm(r4, x);

            watch.Restart();
            r5 = sk5_0(map);
            p5 = watch.ElapsedMilliseconds;
            i5 = gen_bitm(r5, x);

            pictureBox2.Image = i2;
            pictureBox3.Image = i3;
            pictureBox4.Image = i4;
            pictureBox5.Image = i5;
            label10.Text = Convert.ToString(p2) + "ms";
            label11.Text = Convert.ToString(p3) + "ms";
            label12.Text = Convert.ToString(p4) + "ms";
            label5.Text = Convert.ToString(p5) + "ms";
            //int p = 0;
            //bool t = true; 
            //skeletization4(ref map);
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
        public static Bitmap gen_bitm(double[,] map, int x)
        {
            Bitmap res = new Bitmap(map.GetLength(1), map.GetLength(0));
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    res.SetPixel(j, i, (map[i, j] == 1 ? Color.Black : Color.White));//s += "" + map[i, j];
                }
            }
            return new Bitmap(res, new Size(res.Width * x, res.Height * x));
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
        public static double[,] sk_Zang(double[,] a)
        {  
            bool g = true; //; string s = "";
            double[,] d = Clone(a);
            while (sk_zang_s(ref d, g))
            {
                g = !g; 
                /*for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        s += "" + a[i, j];
                    }
                    s += "\n";
                }*/
            } 

            return d;
        }
        public static double[,] sk2_0(double[,] a)
        {
            //a= Clone2(a);
            double[,] d = Clone(a); 
            /*for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    s += "" + a[i, j];
                }
                s += "\n";
            } */
            bool g = true; ;
            while (Impr_Alg1(ref d, g))
            {
                g = !g;
                /*for (int i = 0; i < a.GetLength(0); i++)
               {
                   for (int j = 0; j < a.GetLength(1); j++)
                   {
                       s += "" + a[i, j];
                   }
                   s += "\n";
               }*/
            } 
            Impr_Alg2(ref d);
            

            return d;
        }
        public static double[,] Clone2(double[,] a)
        {
            double[,] b = new double[a.GetLength(0), a.GetLength(1)];
            for(int i = 0; i < a.GetLength(0); i++)
            {
                for(int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j]==1?0:1;
                }
            }
            return b;
        }
        public static double[,] Clone(double[,] a)
        {
            double[,] b = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j] ;
                }
            }
            return b;
        }
        public static bool Impr_Alg1(ref double[,] a, bool g)
        {
            bool res = false;
            double[,] b = Clone(a);
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                { 
                    if (b[i, j] == 0) continue;
                    double[] environment = get_environment2(b, i, j);
                    int sm = 0;
                    int p = 0;
                    bool del = false;
                    for (int t = 0; t < 7; t++)
                    {
                        if (environment[t] == 0 && environment[t + 1] == 1) p++;
                        sm += (int)environment[t];
                    }
                    if (environment[7] == 0 && environment[0] == 1) p++;
                    sm += (int)environment[7];// + (int)environment[8]+ (int)environment[9];
                    if (2 <= sm && sm <= 6)
                    {
                        if (p == 1)
                        {
                            /*if ((environment[4] == 1 ? 0 : 1) + environment[0] + environment[9] == 1 ||
                                (environment[2] == 1 ? 0 : 1) + environment[8] + environment[6] == 1)
                            {

                                del = true;
                            }*/
                            if ( g && (environment[4] == 1 ? 0 : 1) + environment[0] + environment[9] == 1 
                                )
                            { 
                                del = true;
                            }
                            if(!g && (environment[2] == 1 ? 0 : 1) + environment[8] + environment[6] == 1)
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
            return res;

        }
        public static void Impr_Alg2(ref double[,] a)
        {
            //double[,] b = Clone(a);
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] == 0) continue;
                    double[] environment = get_environment2(a, i, j); 
                    if ((environment[2] * environment[4] == 1 && environment[7] == 0)  ||
                       (environment[4] * environment[6] == 1 && environment[1] == 0)  ||
                       (environment[0] * environment[2] == 1 && environment[5] == 0)  ||
                       (environment[0] * environment[6] == 1 && environment[3] == 0)  ) 
                    {
                        a[i, j] = 0; 
                    }
                }
            } 

        }
        public static bool sk_zang_s(ref double[,] a, bool f)//запись массива насыщенности в переменную 
        {
            double[,] b = Clone(a);
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
        static double[] get_environment2(double[,] a, int i, int j)//получение окружения
        {
            return new double[10] { a[i - 1, j] ,
                a[i - 1, j + 1] ,
                a[i, j + 1] ,
                a[i + 1, j + 1] ,
                a[i + 1, j] ,
                a[i + 1, j - 1] ,
                a[i, j - 1] ,
                a[i - 1, j - 1],
                (j+2<a.GetLength(1)?a[i,j+2]:0),
                (i+2<a.GetLength(0)?a[i+2,j]:0)};

        }
        static double[] get_environment4(double[,] a, int i, int j)//получение окружения
        {
            return new double[10] { 
                a[i - 1, j - 1],
                a[i - 1, j] ,
                a[i - 1, j + 1] ,
                a[i, j + 1] ,
                a[i + 1, j + 1] ,
                a[i + 1, j] ,
                a[i + 1, j - 1] ,
                a[i, j - 1] ,
                a[i - 1, j - 1],
                a[i - 1, j]};

        }
        static double[] get_environment3(double[,] a, int i, int j)//получение окружения
        {
            return new double[11] {
                a[i, j - 1] ,
                a[i - 1, j - 1],
                a[i - 1, j] ,
                a[i - 1, j + 1] ,
                a[i, j + 1] ,
                a[i + 1, j + 1] ,
                a[i + 1, j] ,
                a[i + 1, j - 1] ,
                a[i, j - 1] ,
                a[i - 1, j - 1],
                a[i - 1, j]};

        }
        public static double[,] skeletization4(double[,] map)
        {
            double[,] d = drying_out(map); 
            /*for(int i = 0; i < sf.GetLength(0); i++)
            {
                for (int j = 0; j < sf.GetLength(1); j++)
                {
                    s1 += sf[i,j] + "";
                }
                s1 += "\n";
            }
            string s2 = "";*/
            double max = mx(d);
            while (del_min(ref d, max))
            {
               /*for (int i = 0; i < sf.GetLength(0); i++)
                {
                    for (int j = 0; j < sf.GetLength(1); j++)
                    {
                        s2 += sf[i, j]==0?"  ": sf[i, j] <10 ? " "+sf[i, j]: sf[i, j]+"";
                    }
                    s2 += "\n";
                }*/
                 
            }
            bin_matr(ref d);
            return d;
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
        static bool del_min(ref double[,] a, double max)//удалить минимум, модуль 4
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
                    if (a[i, j] <= min && a[i, j] != 0)
                    {
                        double[] environment = get_environment(a, i, j);
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
                                    if (t % 2 == 0 && environment[t] == 1 && (environment[t + 1] == 1) )
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
                            if (!del && p >= 2)
                            {
                                if (environment[2] == 1 && environment[3] == 0 && environment[4] == 1 && (environment[5] == 1 || environment[6] == 1))
                                {
                                    del = true;
                                }
                            }/**/

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


        /// <summary>
        /// ///////////////////////////
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// 


        public static double[,] sk3_0(double[,] a)
        { 
            double[,] f = drying_out(a);
            string s = "";
            double max = mx(f);
            while (sk3_0_mod(ref f, max))
            {
                /*int n = 0;
                 for (int i = 0; i < f.GetLength(0); i++)
                {
                    for (int j = 0; j < f.GetLength(1); j++)
                    {
                        s += (f[i, j] == 0 ? 0 + "" : "" + 1);//"" + sf[i, j];  
                        n += f[i, j] == 0 ? 0 : 1;
                    }
                    s += "\n";
                }
                s += " - " + n+ "\n";*/
            }
            bin_matr(ref f);
            Impr_Alg2(ref f);
            return f;
        }
        public static bool sk3_0_mod(ref double[,] a, double max)
        {
            bool res = false;
            double min = min_matr(a);
            double[,] b = Clone(a);

            for(int i = 1; i < a.GetLength(0)-1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (b[i, j] != min) continue;
                    string s = "";
                    bool del = true;
                    double[] environment = get_environment3(b, i, j);
                    int num_environment = 0;
                    int p = 0;
                    for (int r = 0; r < environment.Length; r++)
                    {
                        environment[r] = environment[r] == 0 ? 0 : 1;
                        if (2 <= r && r <= 9 && environment[r] == 1)
                        {
                            num_environment++;
                            if (environment[r] == 0 && environment[r + 1] >= 1) p++;
                        }
                    }
                    /*s += environment[1] + "" + environment[2] + "" + environment[3] + "\n" +
                        environment[8] + "" + "x" + environment[4] + "\n" +
                        environment[7] + "" + environment[6] + "" + environment[5] + "\n";*/
                    if (num_environment > 2 && num_environment<=6)
                    {
                        for (int r = 2; r < environment.Length - 1; r++)
                        {
                            if (environment[r] == 0) continue;
                            if ((r - 2) % 2 != 0 && environment[r - 1] + environment[r + 1] == 2)
                                continue;
                            if ((r - 2) % 2 == 0 && environment[r - 2] + environment[r - 1] + environment[r + 1] + environment[r + 2] >= 2)
                                continue;
                            del = false;
                            break;

                            //break;
                        }
                        if (del)
                        {
                            res = true;
                            a[i, j] = 0;
                        }
                        else a[i, j]++;
                    }
                    else
                    {
                        a[i, j]++;
                        res = true;
                    }
                    /*if (num_environment > 2 )
                    {
                        
                        if (p == 1)
                        {
                            for (int t = 4; t < 6; t++)
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
                        else
                        {
                            for (int r = 2; r < environment.Length - 1; r++)
                            {
                                if (environment[r] == 0) continue;
                                if ((r - 2) % 2 != 0 && environment[r - 1] + environment[r + 1] == 2)
                                    continue;
                                if ((r - 2) % 2 == 0 && environment[r - 2] + environment[r - 1] + environment[r + 1] + environment[r + 2] >= 2)
                                    continue;
                                del = true;
                                break;
                            }
                        }
                        if (del)
                        {
                            res = true;
                            a[i, j] = 0;
                        }
                        else a[i, j]++;
                }
                    else
                    {
                        a[i, j]++;
                        res = true;
                    }*/
                }
            }
            return max==min?false:res;
        }
        public static double min_matr(double[,] a)
        {
            double min=double.MaxValue;
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] != 0 && a[i, j] < min)
                    {
                        min = a[i, j];
                    }
                }
            }
            return min;
        }
        public static void bin_matr(ref double[,] a)
        { 
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    a[i, j] = a[i, j] == 0 ? 0 : 1;
                }
            }
        }
        public static double  num_p( double[,] a)
        {
            int res = 0;
            for (int i = 1; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] > 0) res++;
                }
            }
            return res;
        }

        //////////////
        public static double[,] sk5_0(double[,] map)
        {
            double[,] d = drying_out(map);
            /*string s = "";
            for(int i = 0; i < d.GetLength(0); i++)
            {
                for (int j = 0; j < d.GetLength(1); j++)
                {
                    s  += d[i, j] == 0 ? "   " : d[i, j] < 10 ? "  " + d[i, j] : d[i, j] + "";
                    }
                s  += "\n";
            }
            s += num_p(d) + "\n";*/
            double max = mx(d); 
            while (del_min2(ref d, max ))
            { 
                /*for (int i = 0; i < d.GetLength(0); i++)
                {
                    for (int j = 0; j < d.GetLength(1); j++)
                    {
                        //s += d[i, j] == 0 ? "  " : 1+"";
                        s += d[i, j]==0?"  ": d[i, j] <10 ? d[i, j]+ " " : d[i, j]+"";
                    }
                    s += "\n";
                }
                s += num_p(d)+"\n";*/


            }
            bin_matr(ref d); 
            return d;
        }
        static double[,] drying_out2(double[,] a)
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
                            if (a[i, j + n] == 0 || a[i, j - n] == 0 || a[i + n, j] == 0 || a[i - n,j] == 0)
                            {
                                res[i, j] = n;
                                f = false;
                            }
                            n++;
                        }
                    }
                }
            }  
            /*for (int i = 1; i < a.GetLength(1) - 1; i++)
            {
                for (int j = 1; j < a.GetLength(0) - 1; j++)
                {
                    if (a[j, i] >= 1)
                    {
                        int n = 0;
                        bool f = true;
                        while (f)
                        {
                            if (a[j+n, i] == 0 || a[j-n, i] == 0)
                            {
                                res[j, i] += n;
                                f = false;
                            }
                            n++;
                        }
                    }
                }
            }*/
            return res;

        } 
        static bool del_min2(ref double[,] a, double max )//удалить минимум, модуль 4
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
                    if (a[i, j] == min)
                    {
                        double[] environment = get_environment4(a, i, j);
                        int sm = 0;
                        int p = 0;
                        for (int t = 0; t <environment.Length; t++)
                        {
                            environment[t] = environment[t] > 0 ? 1 : 0; 
                            if(1<=t && t <= 8)
                            {
                                if ((t - 1) % 2 == 0)
                                {
                                    if (environment[t] == 0 && environment[t + 1] >= 1) 
                                        p++;
                                }
                                else
                                {
                                    if(environment[t] == 0 && environment[t + 1]>=1 && environment[t - 1]==0) 
                                        p++;
                                }
                                sm+=(int)environment[t];
                            }
                        }
                        if (sm >=1 && sm<=5)
                        {
                            bool del = false;
                            if (p == 1)
                            {
                                for (int t = 1; t < 9; t++)
                                {
                                    if (t-1 % 2 == 0 && environment[t] == 1 && (environment[t + 1] == 1 || environment[t + 2] == 1))
                                    {
                                        del = true;
                                        break;
                                    }
                                    else if (t-1 % 2 != 0 && environment[t] == 1 && environment[t + 1] == 1)
                                    {
                                        del = true;
                                        break;
                                    }
                                } 
                            }

                            /*if (!del && p >= 2)
                            {
                                if (environment[2] == 1 && environment[3] == 0 && environment[4] == 1 && (environment[5] == 1 || environment[6] == 1))
                                {
                                    del = true;
                                }
                            } */
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
                       /*double[] environment = get_environment(a, i, j);
                       int sm = 0;
                       int p = 0;

                       if (environment[7] == 0 && environment[0] >= 1) p++;
                       sm += environment[7] > 0 ? 1 : 0;
                       for (int t = 0; t < 7; t++)
                       {
                           environment[t] = environment[t] > 0 ? 1 : 0;
                           if(t%2==0)
                               if (environment[t] == 0 && environment[t + 1] >= 1) p++;
                           sm += environment[t] > 0 ? 1 : 0;
                           if (p > 2)
                               break;
                       }
                       if(sm==1 && environment[7] == 1)
                       { 
                           res = true;
                           a[i, j] = 0;
                           continue;
                       }
                       if (sm >= 1&& sm < 6 )
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

                           if (!del && p >= 2)
                            {
                                if (environment[2] == 1 && environment[3] == 0 && environment[4] == 1 && (environment[5] == 1 || environment[6] == 1))
                                {
                                    del = true;
                                }
                            }

                            /*if ( f&&!del && p >= 2)
                            {
                                if (environment[0] == 1 && environment[1] == 0 && environment[2] == 1 && (environment[3] == 1 || environment[4] == 1))
                                {
                                    del = true;
                                }
                            }
                            if (!f && !del && p >= 2)
                            {
                                if (environment[0] == 1 && environment[7] == 0 && environment[6] == 1 && (environment[5] == 1 || environment[4] == 1))
                                {
                                    del = true;
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
                       }*/
                        }

                    }
            }
            return res;
        }
    }
}
