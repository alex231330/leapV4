using System;
using System.Drawing;
using System.Windows.Forms;
using Leap;
using MonoBrick.NXT;
using System.Threading;

namespace leap_form
{
    public partial class Form1 : Form
    {
        int i1 = 0, k1, r = 4;
        int n = 0, i8 = 0;
        int[] score = new int[2];
        LeapMotion leap;
        Graphics g;
        byte[] array = new byte[9999999];
        bool mode = false;
        Thread drwH;
        Thread Drawing;
        int f1, f2, f3, f4, f5;
        Controller controller;
        bool reMode = false;
        static Brick<Sensor, Sensor, Sensor, Sensor> nxt;
        static Brick<Sensor, Sensor, Sensor, Sensor> nxt2;
        static Brick<Sensor, Sensor, Sensor, Sensor> nxt3;
        static byte[] mes;
        static byte[] mes2;
        byte[] mes3 = { 0,107 };
        byte[] mes4 = { 0, 35 };
        LocalNetwork ln = new LocalNetwork();
        float[] cordsX = new float[5];
        float[] cordsY = new float[5];
        int[] wtf = new int[6];
        int count = 0;
        String[] ReaderArray = new String[6];
        int pref5 = 0;
        int xx = 0, zz = 0, yy = 0;
        int handRotationState = 0;
        byte[] preMes1 = new byte[5];
        byte[] preMes2 = new byte[5];
        byte[,] preArray = new byte[5,8];
        int prevT = 0;
        int nn = 0;
        LeapMotion leaper = new LeapMotion();
        byte[] reaper = new byte[307200];

        public Form1()
        {
            InitializeComponent();
            leap = new LeapMotion();
            controller = new Controller();
            drwH = new Thread(drwHand);
            Drawing = new Thread(DrawingAll);
        }

        /// <summary>
        /// Buttons function
        /// </summary>
        
        private void Button_Click_1(object sender, EventArgs e)
        {
            if (nn == 0)
            {
                nxt = new Brick<Sensor, Sensor, Sensor, Sensor>("COM37");
                nxt2 = new Brick<Sensor, Sensor, Sensor, Sensor>("COM42");
                nxt3 = new Brick<Sensor, Sensor, Sensor, Sensor>("COM41");
                mes = new byte[] { 0, 0, 0, 0, 0 };
                mes2 = new byte[] { 0, 0, 0, 0, 0 };
     /*         try
                {
                    //nxt.Connection.Open();
                }
                catch (Exception)
                {

                    //nxt.Connection.Open();
                }
                try
                {

                    //nxt2.Connection.Open();
                }
                catch (Exception)
                {

                    //nxt2.Connection.Open();
                }
                try
                {
                   
                    //nxt3.Connection.Open();
                }
                catch (Exception)
                {
                    
                    //nxt3.Connection.Open();
                }*/
            }
            try
            {
                drwH.Start();
                Drawing.Start();
            }
            catch (Exception)
            {
                drwH.Start();
                Drawing.Start();
            }
        }

        void send_massage_func()
        {
            if (nxt.Connection.IsConnected)
            {
                //nxt.Mailbox.Send(mes, Box.Box1);
                //nxt.Mailbox.Send(mes2, Box.Box2);
            }
            if (nxt2.Connection.IsConnected)
            {
                //nxt2.Mailbox.Send(mes3, Box.Box1);
            }
            if (nxt3.Connection.IsConnected)
            {
                //nxt3.Mailbox.Send(mes4, Box.Box1);
            }
        }

        void drawRect()
        {
            g = pictureBox1.CreateGraphics();
            Brush brush = new SolidBrush(System.Drawing.Color.Cyan);
            Rectangle rect = new Rectangle(0, 0, 640, 480);
            g.FillRectangle(brush, rect);
        }

        void drawRound(Graphics g, Pen pen, int size, int xr, int yr, int W, int H)
        {
            int x1R = 0, y1R = 0;
            for (y1R = -size; y1R < size; y1R++)
                for (x1R = -size; x1R < size; x1R++)
                    if ((x1R * x1R + y1R * y1R) <= size * size && x1R + xr < W && y1R + yr < H && x1R + xr > 0 && y1R + yr > 0)
                    {
                        g.DrawLine(pen, x1R + xr, y1R + yr, x1R + xr + 1, y1R + yr + 1);
                    }
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (button2.Text == "start write")
                {
                    button2.Text = "writing...\n(stop)";
                    mode = true;
                }
                else
                {
                    button2.Text = "start write";
                    mode = false;
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                if (button3.Text == "Remake")
                {
                    button3.Text = "Stop remake";
                    reMode = true;
                }
                else if (button3.Text == "Stop remake")
                {
                    reMode = false;
                    button3.Text = "Remake";
                }
            }
            if (e.KeyCode == Keys.F3)
            {
                Random rnd = new Random();
                r = rnd.Next(0, 3);
               
                textBox1.Text = "" + r;
               
            }
            if (e.KeyCode == Keys.F4)
            {
                r = 4;
            }
            if (e.KeyCode == Keys.F5)
            {
                if (r == 4)
                    r = 5;
                else
                    r = 4;   
            }
        }
      
        private void byter()
        {
            leaper.setRaw(reaper);
            for (int i = 0; i < reaper.Length; i++)
            {
                Console.WriteLine(reaper[i] + "\n");
            }
        }

        public void drawLine(Pen pen, Point[] points, int Width, int xLine1, int yLine1, int xLine2, int yLine2, int W, int H)
        {
            int x1, y1;
            Graphics g = pictureBox1.CreateGraphics();
            if (yLine1 > yLine2)
            {
                int tmp = yLine1;
                yLine1 = yLine2;
                yLine2 = tmp;
                tmp = xLine1;
                xLine1 = xLine2;
                xLine2 = tmp;
            }
            for (y1 = yLine1; y1 < yLine2; y1++)
            {
                x1 = (((y1 - yLine1) * (xLine2 - xLine1)) / (yLine2 - yLine1)) + xLine1;
                for (int i = 0; i < Width; i++)
                { 
                    for (int kk = 0; kk < Width; kk++)
                    {
                        if ((y1 + i) * W + x1 + kk < H * W - 4 && (y1 + i) * W + x1 + kk > 0)
                        {
                            g.DrawLine(pen, x1, y1, x1, y1);
                        }
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "start write")
            {
                button2.Text = "writing...\n(stop)";
                mode = true;
            }
            else
            {
                button2.Text = "start write"; mode = false;
            } 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (count == 1) count = 0; else count = 1;
            if (count == 1)
            {
                button3.Text = "Stop remake";
                reMode = true;
            }
            else if (count == 0)
            {
                reMode = false;
                button3.Text = "Remake";
            }
        }

        void DrawingAll()
        {
            leap.OnFrame(controller);
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(System.Drawing.Color.Black);
            Pen pen1 = new Pen(System.Drawing.Color.Yellow);
            Pen pen2 = new Pen(System.Drawing.Color.Red);
            string[] drawString = new string[10];
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            int sd = 0 - 3, s1 = 300;

            while (true)
            {
                leap.OnFrame(controller);
                if (count == 0)
                {
                    int ghj;
                    int s = 400;
                    int koe = 3;
                    
                    drawString[0] = "" + mes[1];
                    drawString[1] = "" + mes[2];
                    drawString[2] = "" + mes[3];
                    drawString[3] = "" + mes[4];
                    drawString[4] = "" + 5;
                    g.DrawString(drawString[0], drawFont, drawBrush, 60, 50, drawFormat);
                    g.DrawString(drawString[1], drawFont, drawBrush, 110, 50, drawFormat);
                    g.DrawString(drawString[2], drawFont, drawBrush, 160, 50, drawFormat);
                    g.DrawString(drawString[3], drawFont, drawBrush, 210, 50, drawFormat);
                    g.DrawString(drawString[4], drawFont, drawBrush, 260, 50, drawFormat);
                    g.DrawString(drawString[5], drawFont, drawBrush, 450, 50, drawFormat);
                    g.DrawString(drawString[6], drawFont, drawBrush, 500, 50, drawFormat);
                    g.DrawString(drawString[7], drawFont, drawBrush, 400, 50, drawFormat);
                    g.DrawString(drawString[8], drawFont, drawBrush, 470, 50, drawFormat);
                    dr(4 * (int)leap.cordY - s, koe * (int)leap.cordX - s - s1, 30);
                    dr(4 * (int)leap.cordY4 - s, koe * (int)leap.cordX4 - s - s1, 30);
                    dr(4 * (int)leap.cordY8 - s, koe * (int)leap.cordX8 - s - s1, 30);
                    dr(4 * (int)leap.cordY12 - s, koe * (int)leap.cordX12 - s - s1, 30);
                    dr(4 * (int)leap.cordY16 - s, koe * (int)leap.cordX16 - s - s1, 30);
                    dr(4 * (int)leap.cordY2 - s, koe * (int)leap.cordX2 - s - s1, 30);
                    dr(4 * (int)leap.cordY6 - s, koe * (int)leap.cordX6 - s - s1, 30);
                    dr(4 * (int)leap.cordY10 - s, koe * (int)leap.cordX10 - s - s1, 30);
                    dr(4 * (int)leap.cordY14 - s, koe * (int)leap.cordX14 - s - s1, 30);
                    dr(4 * (int)leap.cordY18 - s, koe * (int)leap.cordX18 - s - s1, 30);
                    dr(4 * (int)leap.cordY11 - s, koe * (int)leap.cordX11 - s - s1, 30);
                    dr(4 * (int)leap.t[1] - s, koe * (int)leap.t[0] - s - s1, 60);
                    dr(4 * (int)leap.h[2] - s, koe * (int)leap.h[0] - s - s1, 30);
                    for (int f = 0; f < 1; f++)
                        for (int h = 0; h < 1; h++)
                        {
                            g.DrawLine(pen, 4 * (int)(leap.cordY2 - sd + h) - s, koe * (int)(leap.cordX2 - sd + f) - s - s1, 4 * (int)(leap.cordY6 - sd + h) - s, koe * (int)(leap.cordX6 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY14 - sd + h) - s, koe * (int)(leap.cordX14 - sd + f) - s - s1, 4 * (int)(leap.cordY2 - sd + h) - s, koe * (int)(leap.cordX2 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY14 - sd + h) - s, koe * (int)(leap.cordX14 - sd + f) - s - s1, 4 * (int)(leap.cordY10 - sd + h) - s, koe * (int)(leap.cordX10 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY10 - sd + h) - s, koe * (int)(leap.cordX10 - sd + f) - s - s1, 4 * (int)(leap.cordY11 - sd + h) - s, koe * (int)(leap.cordX11 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY6 - sd + h) - s, koe * (int)(leap.cordX6 - sd + f) - s - s1, 4 * (int)(leap.cordY18 - sd + h) - s, koe * (int)(leap.cordX18 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY2 - sd + h) - s, koe * (int)(leap.cordX2 - sd + f) - s - s1, 4 * (int)(leap.cordY - sd + h) - s, koe * (int)(leap.cordX - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY6 - sd + h) - s, koe * (int)(leap.cordX6 - sd + f) - s - s1, 4 * (int)(leap.cordY4 - sd + h) - s, koe * (int)(leap.cordX4 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY10 - sd + h) - s, koe * (int)(leap.cordX10 - sd + f) - s - s1, 4 * (int)(leap.cordY8 - sd + h) - s, koe * (int)(leap.cordX8 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY14 - sd + h) - s, koe * (int)(leap.cordX14 - sd + f) - s - s1, 4 * (int)(leap.cordY12 - sd + h) - s, koe * (int)(leap.cordX12 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.cordY18 - sd + h) - s, koe * (int)(leap.cordX18 - sd + f) - s - s1, 4 * (int)(leap.cordY16 - sd + h) - s, koe * (int)(leap.cordX16 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.h[2] - sd + h) - s, koe * (int)(leap.h[0] - sd + f) - s - s1, 4 * (int)(leap.cordY11 - sd + h) - s, koe * (int)(leap.cordX11 - sd + f) - s - s1);
                            g.DrawLine(pen, 4 * (int)(leap.h[2] - sd + h) - s, koe * (int)(leap.h[0] - sd + f) - s - s1, 4 * (int)(leap.cordY18 - sd + h) - s, koe * (int)(leap.cordX18 - sd + f) - s - s1);
                        }
                   // byter();
                    Thread.Sleep(100);
                    g.Clear(System.Drawing.Color.Yellow);
                }
            }

        }

       void drawWLine(int x, int y, int x1, int y1)
        {
            g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(System.Drawing.Color.White);
            g.DrawLine(pen, x, y, x1, y1);
        }
        void dr(int x, int y, int size)
        {
            g = pictureBox1.CreateGraphics();
            SolidBrush redBrush = new SolidBrush(System.Drawing.Color.Black);
            g.FillEllipse(redBrush, x, y, size, size);
        }
        int vectorlength(float x1, float y1, float x2, float y2)
        {
            return (int)(Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))) ;
        }
        int vectorlength1(float x1, float y1, float x2, float y2, float z1, float z2)
        {
            return (int)(Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2))); 
        }
        int kv(int yyyy)
        {
            return yyyy * yyyy;
        }
        void drwHand()
        {
            pref5 = f5;
            while (true)
            {
                prevT = (int)leap.wrist[2];
                if (count == 0 && r == 4)
                {
                    //leap.serializingData[5];
                    f1 = (int)(4 * (((180 / 3.14) * Math.Acos(((kv((int)(leap.cordX6 - leap.cordX4)) + kv((int)(leap.cordY6 - leap.cordY4)) + kv((int)(leap.cordZ6 - leap.cordZ4))) + (kv((int)(leap.cordX18 - leap.cordX6)) + kv((int)(leap.cordY18 - leap.cordY6)) + kv((int)(leap.cordZ18 - leap.cordZ6))) - (kv((int)(leap.cordX18 - leap.cordX4)) + kv((int)(leap.cordY18 - leap.cordY4)) + kv((int)(leap.cordZ18 - leap.cordZ4)))) / (2 * Math.Sqrt(kv((int)(leap.cordX4 - leap.cordX6)) + kv((int)(leap.cordY4 - leap.cordY6)) + kv((int)(leap.cordZ4 - leap.cordZ6))) * Math.Sqrt(kv((int)(leap.cordX18 - leap.cordX6)) + kv((int)(leap.cordY18 - leap.cordY6)) + kv((int)(leap.cordZ18 - leap.cordZ6)))))) - 85));
                        f2 = (int)(4 * (((180 / 3.14) * Math.Acos(((kv((int)(leap.cordX2 - leap.cordX)) + kv((int)(leap.cordY2 - leap.cordY)) + kv((int)(leap.cordZ2 - leap.cordZ))) + (kv((int)(leap.cordX18 - leap.cordX2)) + kv((int)(leap.cordY18 - leap.cordY2)) + kv((int)(leap.cordZ18 - leap.cordZ2))) - (kv((int)(leap.cordX18 - leap.cordX)) + kv((int)(leap.cordY18 - leap.cordY)) + kv((int)(leap.cordZ18 - leap.cordZ)))) / (2 * Math.Sqrt(kv((int)(leap.cordX - leap.cordX2)) + kv((int)(leap.cordY - leap.cordY2)) + kv((int)(leap.cordZ - leap.cordZ2))) * Math.Sqrt(kv((int)(leap.cordX18 - leap.cordX2)) + kv((int)(leap.cordY18 - leap.cordY2)) + kv((int)(leap.cordZ18 - leap.cordZ2)))))) - 80));
                        f3 = (int)(4 * (((180 / 3.14) * Math.Acos(((kv((int)(leap.cordX14 - leap.cordX12)) + kv((int)(leap.cordY14 - leap.cordY12)) + kv((int)(leap.cordZ14 - leap.cordZ12))) + (kv((int)(leap.px - leap.cordX14)) + kv((int)(leap.py - leap.cordY14)) + kv((int)(leap.pz - leap.cordZ14))) - (kv((int)(leap.px - leap.cordX12)) + kv((int)(leap.py - leap.cordY12)) + kv((int)(leap.pz - leap.cordZ12)))) / (2 * Math.Sqrt(kv((int)(leap.cordX12 - leap.cordX14)) + kv((int)(leap.cordY12 - leap.cordY14)) + kv((int)(leap.cordZ12 - leap.cordZ14))) * Math.Sqrt(kv((int)(leap.px - leap.cordX14)) + kv((int)(leap.py - leap.cordY14)) + kv((int)(leap.pz - leap.cordZ14)))))) - 85));
                        f4 = (int)(4 * (((180 / 3.14) * Math.Acos(((kv((int)(leap.cordX10 - leap.cordX8)) + kv((int)(leap.cordY10 - leap.cordY8)) + kv((int)(leap.cordZ10 - leap.cordZ8))) + (kv((int)(leap.px - leap.cordX10)) + kv((int)(leap.py - leap.cordY10)) + kv((int)(leap.pz - leap.cordZ10))) - (kv((int)(leap.px - leap.cordX8)) + kv((int)(leap.py - leap.cordY8)) + kv((int)(leap.pz - leap.cordZ8)))) / (2 * Math.Sqrt(kv((int)(leap.cordX8 - leap.cordX10)) + kv((int)(leap.cordY8 - leap.cordY10)) + kv((int)(leap.cordZ8 - leap.cordZ10))) * Math.Sqrt(kv((int)(leap.px - leap.cordX10)) + kv((int)(leap.py - leap.cordY10)) + kv((int)(leap.pz - leap.cordZ10)))))) - 90));
                        f5 = 250 + (vectorlength1(leap.cordX14, leap.cordY14, leap.cordX17, leap.cordY17, leap.cordZ14, leap.cordZ17) - 93) * 9;
                        handRotationState = 255 - (int)((leap.cordZ6 - leap.cordZ10) * 3.5 + 140);
                        xx = (int)(Math.Abs((int)leap.cordX11 - 150) * 1);
                        zz = (int)(Math.Abs((int)leap.cordZ11 - 390) * 1);
                        yy = (int)leap.cordY11 / 3;
                    if (f1 > 250 || f1 < -1000)
                        mes[1] = 0;
                    else if (f1 > 0)
                        mes[1] = (byte)(250 - f1);
                    else
                        mes[1] = 250;
                    if (f2 > 250 || f2 < -1000)
                        mes[2] = 250;
                    else if (f2 > 0)
                        mes[2] = (byte)(f2);
                    else
                        mes[2] = 0;
                    if (f3 > 250 || f3 < -1000)
                        mes[3] = 250;
                    else if (f3 > 0)
                        mes[3] = (byte)(f3);
                    else
                        mes[3] = 0;
                    if (f4 > 250 || f4 < -1000)
                        mes[4] = 250;
                    else if (f4 > 0)
                        mes[4] = (byte)(f4);
                    else
                        mes[4] = 0;
                    if (f5 > 192)
                        mes2[1] = 0;
                    else if (f5 < 64) 
                        mes2[1] = 250;
                    else
                        mes2[1] = (byte)(250 - f5);
                    mes2[0] = 0;
                        mes2[2] = (byte)((Math.Sqrt((xx * xx + zz * zz))) * 2 / 5 + 25);
                        mes2[3] = (byte)((Math.Sqrt(((270 - zz) * (270 - zz) + xx * xx))) * 2 / 5 + 25);
                    if (handRotationState < 0)
                        mes2[4] = 0;
                    else if (handRotationState > 75 && handRotationState < 200)
                        mes2[4] = 35;
                    else
                      mes2[4] = (byte)(handRotationState / 4);
                    mes4[1] = mes2[4];
                        mes3[1] = (byte)yy;
                    send_massage_func();
                }
                else if(r == 0)
                {

                    mes[1] = 250;

                    mes[2] = 0;

                    mes[3] = 0;

                    mes[4] = 0;

                    mes2[1] = 250;
                    send_massage_func();

                }
                else if (r == 1)
                {

                    mes[1] = 0;

                    mes[2] = 250;

                    mes[3] = 0;

                    mes[4] = 0;

                    mes2[1] = 0;
                    send_massage_func();

                }
                else if (r == 2)
                {
                    mes[1] = 0;
                    mes[2] = 250;
                    mes[3] = 250;
                    mes[4] = 250;
                    mes2[1] = 0;
                    send_massage_func();
                }
                if (mode == true)
                {
                    k1 = i1 + 4;
                    for (; i1 < k1; i1++)
                    {
                        array[i1] = mes[i1 % 9 + 1];
                    }
                    for (; i1 < k1 + 4; i1++)
                    {
                        array[i1] = mes2[i1 % 9 - 4 + 1];
                    }
                    array[i1] = mes3[1];
                    i1++;
                }
                if (reMode == true)
                {
                    for (int i = 0; i < i1;)
                    {
                        mes[1] = array[i];
                        i++;
                        mes[2] = array[i];
                        i++;
                        mes[3] = array[i];
                        i++;
                        mes[4] = array[i];
                        i++;
                        mes2[1] = array[i];
                        i++;
                        mes2[2] = array[i];
                        i++;
                        mes2[3] = array[i];
                        i++;
                        mes2[4] = array[i];
                        i++;
                        mes3[1] = array[i];
                        i++;
                        mes4[1] = mes2[4];
                        send_massage_func();
                    }
                }
               }
            }
            }
    }   