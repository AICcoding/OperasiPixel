using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.Structure;
using ZedGraph;

namespace Aplikasi_Operasi_Pixel
{
    public partial class Form1 : Form
    {
        Bitmap gambar_awal, gambar_akhir, gambar_tmp, gambar_now;
        Image<Bgr, Byte> gambar_awal_e, gambar_akhir_e, gambar_tmp_e;
        GraphPane histogram_awal, histogram_akhir;
        int mode;
        public Form1()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
            textBox1.Text = Convert.ToString(trackBar1.Value);
            textBox2.Text = Convert.ToString((float)trackBar2.Value/10F);

            radioButton4.Checked = true;
            mode = 2;
            radioButton2.Checked = true;
            radioButton6.Checked = true;
            radioButton8.Checked = true;

            comboBox1.Text = "Pilih canel";
            comboBox1.Items.Add("Gabungan");
            comboBox1.Items.Add("Red");
            comboBox1.Items.Add("Green");
            comboBox1.Items.Add("Blue");
            comboBox2.Text = "Pilih canel";
            comboBox2.Items.Add("Gabungan");
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Blue");

            histogram_awal = zedGraphControl1.GraphPane;
            histogram_awal.Title = "Histogram Gambar Awal";
            histogram_awal.YAxis.Title = "Jumlah Pixel";
            histogram_awal.XAxis.Title = "Nilai Pixel";

            histogram_akhir = zedGraphControl2.GraphPane;
            histogram_akhir.Title = "Histogram Gambar Akhir";
            histogram_akhir.YAxis.Title = "Jumlah Pixel";
            histogram_akhir.XAxis.Title = "Nilai Pixel";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar1.Value);
            if (pictureBox1.Image != null)
            {
                if (mode == 1)
                {
                    operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value/10F);
                }
                else if (mode == 2)
                {
                    operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                }
            }
            
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString((float)trackBar2.Value / 10F);
            if (pictureBox1.Image != null)
            {
                if (mode == 1)
                {
                    operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value / 10F);
                }
                else if (mode == 2)
                {
                    operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if(pilih_gambar.ShowDialog()==DialogResult.OK)
            {
                gambar_awal_e = new Image<Bgr, byte>(pilih_gambar.FileName);
                gambar_akhir_e = new Image<Bgr, byte>(pilih_gambar.FileName);
                gambar_tmp_e = new Image<Bgr, byte>(pilih_gambar.FileName);

                gambar_awal = new Bitmap(new Bitmap(pilih_gambar.FileName));
                gambar_akhir = new Bitmap(new Bitmap(pilih_gambar.FileName));
                gambar_tmp = new Bitmap(new Bitmap(pilih_gambar.FileName));

                pictureBox1.Image = gambar_awal;
                if(mode==1)
                {
                    operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value / 10F);
                }
                else if(mode==2)
                {
                    operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                }
            }
        }

        private void operasi_pixel_primitif(int kecerahan, float kekontrasan)
        {
            int r, g, b;
            float tmp;
            for (int i = 0; i < gambar_awal.Width; i++)
            {
                for (int j = 0; j < gambar_awal.Height; j++)
                {
                    tmp = (kekontrasan * gambar_awal.GetPixel(i, j).R) + kecerahan;
                    tmp = (float)Math.Round(tmp);
                    r = (int)tmp;
                    if (r > 255) r = 255;
                    else if (r < 0) r = 0;

                    tmp = (kekontrasan * gambar_awal.GetPixel(i, j).G) + kecerahan;
                    tmp = (float)Math.Round(tmp);
                    g = (int)tmp;
                    if (g > 255) g = 255;
                    else if (g < 0) g = 0;

                    tmp = (kekontrasan * gambar_awal.GetPixel(i, j).B) + kecerahan;
                    tmp = (float)Math.Round(tmp);
                    b = (int)tmp;
                    if (b > 255) b = 255;
                    else if (b < 0) b = 0;

                    gambar_akhir.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            pictureBox2.Image = gambar_akhir;
        }

        private void operasi_pixel_emgu(int kecerahan, float kekontrasan)
        {
            gambar_akhir_e = (kekontrasan * gambar_awal_e) + kecerahan;
            pictureBox2.Image = gambar_akhir_e.ToBitmap();   
        }

        private void ubah_ke_negatif_primitif()
        {
            trackBar1.Value = 0;
            trackBar2.Value = 10;
            textBox1.Text = "0";
            textBox2.Text = "1";
            int r, g, b;
            for (int i = 0; i < gambar_awal.Width; i++)
            {
                for (int j = 0; j < gambar_awal.Height; j++)
                {
                    r = 255 - gambar_awal.GetPixel(i, j).R;

                    g = 255 - gambar_awal.GetPixel(i, j).G;

                    b = 255 - gambar_awal.GetPixel(i, j).B;

                    gambar_akhir.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            gambar_awal = (Bitmap)gambar_akhir.Clone();
            pictureBox2.Image = gambar_akhir;

        }

        private void ubah_ke_negatif_emgu()
        {
            trackBar1.Value = 0;
            trackBar2.Value = 10;
            textBox1.Text = "0";
            textBox2.Text = "1";
            //gambar_akhir_e = gambar_awal_e.Not();
            gambar_akhir_e = 255-gambar_awal_e;
            gambar_awal_e = gambar_akhir_e.Clone();
            pictureBox2.Image = gambar_akhir_e.ToBitmap();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton3.Checked==true)
            {
                mode = 1;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                mode = 2;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                if(gambar_awal!=null)
                {
                    if (mode == 1)
                    {
                        ubah_ke_negatif_primitif();
                    }
                    else if (mode == 2)
                    {
                        ubah_ke_negatif_emgu();
                    }
                }      
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked==true)
            {
                if(gambar_awal!=null)
                {
                    gambar_awal = gambar_tmp;
                    gambar_awal_e = gambar_tmp_e;
                    if (mode == 1)
                    {
                        operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }
                    else if (mode == 2)
                    {
                        operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }
                }                
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (pictureBox1.Image != null)
                {
                    trackBar1.Value = Convert.ToInt16(textBox1.Text);
                    if (mode == 1)
                    {
                        operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }
                    else if (mode == 2)
                    {
                        operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }

                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (pictureBox1.Image != null)
                {
                    decimal tmp;
                    tmp = Convert.ToDecimal(textBox2.Text) * 10;
                    trackBar2.Value = (int)tmp;
                    if (mode == 1)
                    {
                        operasi_pixel_primitif(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }
                    else if (mode == 2)
                    {
                        operasi_pixel_emgu(trackBar1.Value, (float)trackBar2.Value / 10F);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(gambar_awal!=null)
            {
                if(mode==1)
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        //Rumus membuat histogram gabungan BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "gabungan");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        //Rumus membuat histogram red BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "red");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        //Rumus membuat histogram green BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "green");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        //Rumus membuat histogram blue BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "blue");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                }
                else if(mode==2)
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        //Rumus membuat histogram gabungan BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "gabungan");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        //Rumus membuat histogram red BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "red");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        //Rumus membuat histogram green BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "green");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        //Rumus membuat histogram blue BELUM SELESAI...
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "blue");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(gambar_awal!=null)
            {
                if (mode == 1)
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        //Rumus membuat histogram gabungan BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "gabungan");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        //Rumus membuat histogram red BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "red");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 2)
                    {
                        //Rumus membuat histogram green BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "green");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 3)
                    {
                        //Rumus membuat histogram blue BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "blue");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                }
                else if (mode == 2)
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        //Rumus membuat histogram gabungan BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "gabungan");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        //Rumus membuat histogram red BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "red");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 2)
                    {
                        //Rumus membuat histogram green BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "green");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (comboBox2.SelectedIndex == 3)
                    {
                        //Rumus membuat histogram blue BELUM SELESAI...
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "blue");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                }
            }
        }

        private void membuat_histogram_primitif(Bitmap gambar, GraphPane histogram, string RGB)
        {
            int[] nilai_pixel = new int[256];
            int tmp, r, g, b;
            PointPairList data_pixel = new PointPairList();
            BarItem kurva;
            if (RGB == "red") //jika red
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        tmp = gambar.GetPixel(i, j).R;
                        nilai_pixel[tmp] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Red);
                kurva.Bar.Fill = new Fill(Color.Red);
                //histogram.AxisFill = new Fill(Color.Red);
            }
            else if (RGB == "green") //jika green
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        tmp = gambar.GetPixel(i, j).G;
                        nilai_pixel[tmp] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Green);
                kurva.Bar.Fill = new Fill(Color.Green);
            }
            else if (RGB == "blue") //jika blue
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        tmp = gambar.GetPixel(i, j).B;
                        nilai_pixel[tmp] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Blue);
                kurva.Bar.Fill = new Fill(Color.Blue);
            }
            else if (RGB == "gabungan") //jika gabungan
            {
                int[,] n_p = new int[256, 3];
                int nilai_mak = 0;
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {

                        r = gambar.GetPixel(i, j).R;
                        g = gambar.GetPixel(i, j).G;
                        b = gambar.GetPixel(i, j).B;

                        n_p[r, 0] += 1;
                        n_p[g, 1] += 1;
                        n_p[b, 2] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    /*if (nilai_mak < n_p[i, 0]) nilai_mak = n_p[i, 0]; //jika r lebih besar
                    else if (nilai_mak < n_p[i, 1]) nilai_mak = n_p[i, 1]; //jika g lebih besar
                    else if (nilai_mak < n_p[i, 2]) nilai_mak = n_p[i, 2]; //jika b lebih besar

                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;*/

                    nilai_mak = nilai_mak + n_p[i, 0] + n_p[i, 1] + n_p[i, 2];
                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;
                }

                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                kurva.Bar.Fill = new Fill(Color.Black);
            }
            else if(RGB == "luminositi")
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        tmp = (int)((gambar.GetPixel(i, j).R * .3) + (gambar.GetPixel(i, j).G * .59) + (gambar.GetPixel(i, j).B * .11));
                        nilai_pixel[tmp] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                //kurva.Bar.Fill = new Fill(Color.White);
            }
        }

        private void membuat_histogram_emgu(Image<Bgr,byte> gambar, GraphPane histogram, string RGB)
        {
            int[] nilai_pixel = new int[256];
            //int tmp, r, g, b;
            PointPairList data_pixel = new PointPairList();
            BarItem kurva;

            //load image
            //Image<Bgr, byte> image = new Image<Bgr, byte>("sample.png");

            //get the pixel from [row,col] = 24,24
            //Bgr pixel = image[24, 24];

            //get the b,g,r values
            //double b = pixel.Blue;
           // double g = pixel.Green;
            //double r = pixel.Red;
            int r, g, b, grey;
            Bgr pixel;

            if (RGB == "red") //jika red
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        r = (int)pixel.Red;
                        nilai_pixel[r] += 1;
                        /*MessageBox.Show(r.ToString());
                        MessageBox.Show(nilai_pixel[r].ToString());*/
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Red);
                kurva.Bar.Fill = new Fill(Color.Red);
                //histogram.AxisFill = new Fill(Color.Red);
            }
            else if (RGB == "green") //jika green
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        g = (int)pixel.Green;
                        nilai_pixel[g] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Green);
                kurva.Bar.Fill = new Fill(Color.Green);
            }
            else if (RGB == "blue") //jika blue
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        b = (int)pixel.Blue;
                        nilai_pixel[b] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Blue);
                kurva.Bar.Fill = new Fill(Color.Blue);
            }
            else if (RGB == "gabungan") //jika gabungan
            {
                int[,] n_p = new int[256, 3];
                int nilai_mak = 0;
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {

                        pixel = gambar[j, i];
                        r = (int)pixel.Red;
                        g = (int)pixel.Green;
                        b = (int)pixel.Blue;

                        n_p[r, 0] += 1;
                        n_p[g, 1] += 1;
                        n_p[b, 2] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    /*if (nilai_mak < n_p[i, 0]) nilai_mak = n_p[i, 0]; //jika r lebih besar
                    else if (nilai_mak < n_p[i, 1]) nilai_mak = n_p[i, 1]; //jika g lebih besar
                    else if (nilai_mak < n_p[i, 2]) nilai_mak = n_p[i, 2]; //jika b lebih besar

                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;*/

                    nilai_mak = nilai_mak + n_p[i, 0] + n_p[i, 1] + n_p[i, 2];
                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;
                }

                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                kurva.Bar.Fill = new Fill(Color.Black);
            }
            else if (RGB == "luminositi")
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        grey = (int)((pixel.Red * .3) + (pixel.Green * .59) + (pixel.Blue * .11));
                        nilai_pixel[grey] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                //kurva.Bar.Fill = new Fill(Color.White);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Hati-hati!!!\nGambar akan menjadi hitam.\nApakah anda yakin ingin melanjutkannya ? ", "Peringatan", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, 0) == DialogResult.OK)
            {
                MessageBox.Show("SELAMAT DATANG DI LINGKARAN SETAN!!");
                int r, g, b;
                Double tmp;
                for (int i = 0; i < gambar_awal.Width; i++)
                {
                    for (int j = 0; j < gambar_awal.Height; j++)
                    {
                        tmp = Math.Log(1 + Convert.ToDouble(gambar_awal.GetPixel(i, j).R));
                        r = (int)tmp;
                        if (r > 255) r = 255;
                        else if (r < 0) r = 0;

                        tmp = Math.Log(1 + Convert.ToDouble(gambar_awal.GetPixel(i, j).G));
                        g = (int)tmp;
                        if (g > 255) g = 255;
                        else if (g < 0) g = 0;

                        tmp = Math.Log(1 + Convert.ToDouble(gambar_awal.GetPixel(i, j).B));
                        b = (int)tmp;
                        if (b > 255) b = 255;
                        else if (b < 0) b = 0;

                        gambar_akhir.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }
                pictureBox2.Image = gambar_akhir;
            }
            else
            {
                MessageBox.Show("SELAMAT ANDA KELUAR DARI LINGKARAN SETAN!!");
            }
        }
            
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar.ToString() == "-")))
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar.ToString() == ".")))
                e.Handled = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                if (gambar_tmp != null)
                {
                    if (mode == 1)
                    {
                        comboBox1.Visible = false;
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "luminositi");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (mode == 2)
                    {
                        comboBox1.Visible = false;
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "luminositi");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                comboBox1.Visible = true;
                comboBox1.Text = "Gabungan";
                if (gambar_tmp != null)
                {
                    if (mode == 1)
                    {
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_primitif(gambar_tmp, histogram_awal, "gabungan");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                    else if (mode == 2)
                    {
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(gambar_tmp_e, histogram_awal, "gabungan");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    }
                }
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                if (gambar_akhir != null)
                {
                    if (mode == 1)
                    {
                        comboBox2.Visible = false;
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "luminositi");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (mode == 2)
                    {
                        comboBox2.Visible = false;
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "luminositi");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                }
            }

            
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                if (gambar_akhir != null)
                {
                    if (mode == 1)
                    {
                        comboBox2.Visible = true;
                        comboBox2.Text = "Gabungan";
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_primitif(gambar_akhir, histogram_akhir, "gabungan");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                    else if (mode == 2)
                    {
                        comboBox2.Visible = true;
                        comboBox2.Text = "Gabungan";
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(gambar_akhir_e, histogram_akhir, "gabungan");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    }
                }
            }
        }
    }
}
