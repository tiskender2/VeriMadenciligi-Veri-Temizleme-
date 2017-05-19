using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Veri_Temizleme
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
        VeriOku verioku;
        string deger;
        int t, k, j, i;
        int nulldeger;
        double tut, tut2;
        double[] altdizi = new double[8];
        double[] altdizi2 = new double[8];
        double[] altdizi3 = new double[8];
        double[] altdizi4 = new double[8];

        double toplam = 0;
        int sayac = 0;

        private void btnDosyaAc_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialaog = new OpenFileDialog();
            openFileDialaog.Title = "Dosya Aç";
            openFileDialaog.Filter = "Exel |*.xlsx";
            cbIller.Items.Clear();
            if (openFileDialaog.ShowDialog() == DialogResult.OK)
            {
                verioku = new VeriOku(openFileDialaog.FileName);
                List<string> iller = verioku.iller();
                for (int i = 0; i < iller.Count; i++)
                {
                    cbIller.Items.Add(iller[i]);
                }
            }

        }

        private void cbIller_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (verioku != null)
            {
                int indis = cbIller.SelectedIndex;
                List<string[]> satirlar = verioku.ilVerileri(indis, 12, 44);
                gridDoldur(satirlar);
            }
        }

        public void gridDoldur(List<string[]> satirlar)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            int columnsCount = satirlar[0].Length;
            for (int i = 0; i < columnsCount; i++)
            {
                dataGridView1.Columns.Add("", (i + 1) + ".Ay");
            }
            for (int i = 0; i < satirlar.Count; i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < columnsCount; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = satirlar[i][j];
                }
            }

        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                gürültüOrtalama();
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                altSınır();
            }
            else if (comboBox2.SelectedIndex == 2)
            {

                üstSınır();
            }
            else if (comboBox2.SelectedIndex == 3)
            {

                GürültüBul();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                sınıfOrtalama();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                nitelikOrtalama();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                EnYakınKomsu();
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                TümDegerOrt();
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                knn();
            }


        }
     
        public void sınıfOrtalama()
        {
            int satırsayısı = dataGridView1.RowCount - 2;
            int sutunsayısı = dataGridView1.ColumnCount;
            //  MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value.ToString());
            double toplam = 0;
            int sayac = 0;
            int nullsayac = 0;
            double nulldeger;

            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value == "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        nullsayac++;
                        t = j;
                        k = i;
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Aquamarine;

                    }
                    else
                    {
                        toplam += Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                        sayac++;
                    }
                }
                double ortalama = toplam / sayac;
                nulldeger = Math.Round(ortalama, 1);
                if (dataGridView1.Rows[t].Cells[k].Value == "" || dataGridView1.Rows[t].Cells[k].Value == null && nullsayac >= 1)
                {

                    dataGridView1.Rows[t].Cells[k].Value = nulldeger;
                    i--;

                }
                nullsayac = 0;
                toplam = 0;
                sayac = 0;
                dataGridView1.Refresh();
            }




        }
        public void TümDegerOrt()
        {
            int satırsayısı = dataGridView1.RowCount - 2;
            int sutunsayısı = dataGridView1.ColumnCount;
            int nullsayac = 0;
            int sayac = 0;
            double toplam = 0;
            double ort = 0;
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value == "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        nullsayac++;
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Aquamarine;
                    }
                    else
                    {
                        toplam += Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                        sayac++;
                    }
                }
            }
            MessageBox.Show(nullsayac + " Adet boş değer bulundu");
            ort = Math.Round(toplam / sayac,1);
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value == "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = ort;
                    }
                }
            }


        }
        public void knn()
        {

            int k = 5;
            double ortalama = 0;
            double[] uzaklıkDizi;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount - 1; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value == "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {

                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Aquamarine;
                        uzaklıkDizi = uzaklıkHesapla(j, k);
                        uzaklıkDizi = diziSıralama(uzaklıkDizi);
                        for (int m = 1; m <= k; m++)
                        {
                            ortalama += uzaklıkDizi[m];
                        }
                        ortalama = ortalama / k;
                        dataGridView1.Rows[j].Cells[i].Value = Math.Round(ortalama, 1);
                    }
                }
            }

        }
        public double[] uzaklıkHesapla(int satir, int k)
        {
            double uzaklık = 0;
            double[] uzaklıkDizi = new double[dataGridView1.RowCount];
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (!(dataGridView1.Rows[i].Cells[j].Value == "" || dataGridView1.Rows[satir].Cells[j].Value == ""))
                        uzaklık += Math.Pow((Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) - Convert.ToDouble(dataGridView1.Rows[satir].Cells[j].Value)), 2);
                }
                uzaklıkDizi[i] = Math.Sqrt(uzaklık);
                uzaklık = 0;
            }

            return uzaklıkDizi;
        }
        public double[] diziSıralama(double[] dizi)
        {
            double tut = 0;
            for (int i = 0; i < dizi.Length; i++)
            {
                for (int j = i + 1; j < dizi.Length; j++)
                {
                    if (dizi[j] < dizi[i])
                    {
                        tut = dizi[i];
                        dizi[i] = dizi[j];
                        dizi[j] = tut;
                    }
                }
            }
            return dizi;
        }


        public void GürültüSil()
        {
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double[] dizi = new double[satırsayısı];
            double ort = 0;
            double toplam = 0;
            double gürültüsınır;
            for (i = 0; i < sutunsayısı; i++)
            {
                if (dataGridView1.Rows[satırsayısı - 1].Cells[i].Value.ToString() != "" || dataGridView1.Rows[satırsayısı - 1].Cells[i].Value.ToString() != null)
                {
                    ort = Convert.ToDouble(dataGridView1.Rows[satırsayısı - 1].Cells[i].Value.ToString());
                }


                for (j = 0; j < satırsayısı; j++)
                {
                    dizi[j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());


                }
                dizi = diziSıralama(dizi);
                for (int t = 0; t < 7; t++)
                {
                    toplam += dizi[t];

                }
                gürültüsınır = ort + toplam;
                toplam = 0;
                for (int m = 0; m < satırsayısı; m++)
                {
                    if (dataGridView1.Rows[m].Cells[i].Value.ToString() != "" || dataGridView1.Rows[m].Cells[i].Value.ToString() != null)
                    {
                        if (gürültüsınır < Convert.ToDouble(dataGridView1.Rows[m].Cells[i].Value))
                        {
                            dataGridView1.Rows[m].Cells[i].Value ="";
                            dataGridView1.Rows[m].Cells[i].Style.BackColor = Color.Aquamarine;
                            
                        }

                    }
                }

            }
        

        }
        public void GürültüBul()
        {
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double[] dizi = new double[satırsayısı];
            double ort = 0;
            double toplam = 0;
            double gürültüsınır;
            int sayac = 0;
            for (i = 0; i < sutunsayısı; i++)
            {
                if (dataGridView1.Rows[satırsayısı-1].Cells[i].Value.ToString() != "" || dataGridView1.Rows[satırsayısı - 1].Cells[i].Value.ToString() != null)
                {
                    ort = Convert.ToDouble(dataGridView1.Rows[satırsayısı - 1].Cells[i].Value.ToString());
                }
                   
                   
                for (j = 0; j < satırsayısı; j++)
                {
                    dizi[j]=Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                    
                   
                }
                dizi = diziSıralama(dizi);
                for (int t = 0; t < 7; t++)
                {
                    toplam += dizi[t];
                  
                }
                gürültüsınır = ort + toplam;
                toplam = 0;
                for (int m = 0; m <satırsayısı; m++)
                {
                    if (dataGridView1.Rows[m].Cells[i].Value.ToString()!="" || dataGridView1.Rows[m].Cells[i].Value.ToString() !=null)
                    {
                        if (gürültüsınır< Convert.ToDouble(dataGridView1.Rows[m].Cells[i].Value))
                        {
                           // dataGridView1.Rows[m].Cells[i].Value ="";
                            dataGridView1.Rows[m].Cells[i].Style.BackColor = Color.Aquamarine;
                            sayac++;
                        }
                        
                    }
                }
                    
                }
            if (sayac != 0)
            {
                
                DialogResult secenek = MessageBox.Show(sayac + "Adet Gürültülü Veri Bulundu Verileri Silmek İstediğinize Emin Misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (secenek == DialogResult.Yes)
                {
                    GürültüSil();
                }
                else if (secenek == DialogResult.No)
                {
                    
                }

            }
            else
            {
                MessageBox.Show("Gürültülü Veri Yok");
            }
        }
        public void EnYakınKomsu()
        {
            int satırsayısı = dataGridView1.RowCount - 2;
            int sutunsayısı = dataGridView1.ColumnCount;
            double toplam = 0;
            int sayac = 0;
            double ort = 0;
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value == "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Aquamarine;
                        if (i == 0 && j!=0 && j!=satırsayısı-1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;           
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (i==11&& j!=0 && j!= satırsayısı - 1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (j==0 && i==0)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (j == 0 && i!=sutunsayısı-1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i -1].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                         
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (j == satırsayısı - 1 && i!=0 && i!=sutunsayısı-1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (j == satırsayısı - 1 && i==sutunsayısı-1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;

                        }
                        else if (j== satırsayısı - 1 && i==0)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else if (j==0 && i==sutunsayısı-1)
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i-1].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;
                        }
                        else
                        {
                            toplam = Convert.ToDouble(dataGridView1.Rows[j].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j + 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i - 1].Value.ToString());
                            sayac++;
                            toplam += Convert.ToDouble(dataGridView1.Rows[j - 1].Cells[i + 1].Value.ToString());
                            sayac++;
                            ort = toplam / sayac;
                            toplam = 0;
                            sayac = 0;

                        }
                        ort = Math.Round(ort,1);
                        dataGridView1.Rows[j].Cells[i].Value = ort;
                    }
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            int satırsayısı = dataGridView1.RowCount - 2;
            int sutunsayısı = dataGridView1.ColumnCount;
            double toplam = 0;
            double ort = 0;
            int sayac = 0;
            for (int i = 0; i < satırsayısı; i++)
            {
                if (dataGridView1.Rows[i].Cells[e.ColumnIndex].Value!=null && dataGridView1.Rows[i].Cells[e.ColumnIndex].Value!="")
                {
                    toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString());
                    sayac++;
                }
              
            }
            ort = toplam / sayac;
            ort = Math.Round(ort, 1);
            label6.Text = toplam.ToString();
            label7.Text = ort.ToString();
            toplam = 0;
            sayac = 0;
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int satırsayısı = dataGridView1.RowCount - 2;
            int sutunsayısı = dataGridView1.ColumnCount;
            double toplam = 0;
            double ort = 0;
            int sayac = 0;
            for (int i = 0; i < sutunsayısı; i++)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value != null && dataGridView1.Rows[e.RowIndex].Cells[i].Value != "")
                {
                    toplam += Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString());
                    sayac++;
                }

            }
            ort = toplam / sayac;
            ort = Math.Round(ort,1);
            label6.Text = toplam.ToString();
            label7.Text = ort.ToString();
            toplam = 0;
            sayac = 0;
        }

        public void nitelikOrtalama()
        {
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double toplam = 0;
            int sayac = 0;
            int nullsayac = 0;
            double nulldeger;

            for (i = 0; i < satırsayısı; i++)
            {
                for (j = 0; j < sutunsayısı; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value == ""|| dataGridView1.Rows[i].Cells[j].Value == null)
                    {
                        nullsayac++;
                        t = j;
                        k = i;
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Aquamarine;
                    }
                    else
                    {
                        toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value.ToString());
                        sayac++;
                    }
                }
                double ortalama = Math.Round( toplam / sayac,1);
                nulldeger = ortalama;
                if (dataGridView1.Rows[k].Cells[t].Value == "" || dataGridView1.Rows[k].Cells[t].Value == null && nullsayac >= 1)
                {
                    dataGridView1.Rows[k].Cells[t].Value = nulldeger;
                    i--;

                }
                nullsayac = 0;
                toplam = 0;
                sayac = 0;
                dataGridView1.Refresh();
            }
        }
        public void gürültüOrtalama()
        {
            
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double[] dizi = new double[satırsayısı];
          
            double ort = 0;
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value != "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        dizi[j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                    }
                    

                }

                for (int k = 0; k < satırsayısı; k++)
                {
                    tut = dizi[k];

                    for (int m = 0; m <satırsayısı; m++)
                    {
                        if (dizi[k]<dizi[m])
                        {
                            tut2 = dizi[k];
                            dizi[k] = dizi[m];
                            dizi[m] = tut2;


                        }

                    }
                  

                }
               
                    for (int l = 0; l <=7; l++)
                    {
                        altdizi[l] = dizi[l];
                        sayac++;
                        toplam += altdizi[l];
                    if (l==7)
                    {
                        ort = toplam / sayac;
                        for (int y = 0; y <=7; y++)
                        {
                            altdizi[y] = ort;
                            if (y==7)
                            {
                                for (int p= 0; p <= 7; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = ort;
                                    
                                }
                            }
                        }
                      
                        toplam = 0;
                        sayac = 0;
                    }
                       

                    }


                    for (int l = 8; l <= 15; l++)
                    {
                        altdizi2[l-8] = dizi[l];
                        sayac++;
                        toplam += altdizi2[l-8];
                    if (l == 15)
                    {
                        ort = toplam / sayac;
                        for (int y = 0; y <= 7; y++)
                        {
                            altdizi2[y] = ort;
                            if (y == 7)
                            {
                                for (int p = 8; p <= 15; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = ort;
                                    
                                }
                            }

                        }
                        toplam = 0;
                        sayac = 0;
                    }
                    }

                    for (int l = 16; l <= 23; l++)
                    {
                        altdizi3[l-16] = dizi[l];

                        sayac++;
                        toplam += altdizi3[l - 16];
                    if (l == 23)
                    {
                        ort = toplam / sayac;
                        for (int y = 0; y <= 7; y++)
                        {
                            altdizi3[y] = ort;
                            if (y == 7)
                            {
                                for (int p = 16; p <= 23; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = ort;
                                    
                                }
                            }
                        }
                        toplam = 0;
                        sayac = 0;
                    }
                    }

                    for (int l = 24; l <= 31; l++)
                    {
                        altdizi4[l-24] = dizi[l];
                        sayac++;
                        toplam += altdizi4[l - 24];
                    if (l == 31)
                    {
                        ort = toplam / sayac;
                        for (int y = 0; y <= 7; y++)
                        {
                            altdizi4[y] = ort;
                            if (y == 7)
                            {
                                for (int p = 24; p <= 31; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = ort;
                                 
                                }
                            }
                        }
                        toplam = 0;
                        sayac = 0;
                    }
                }


            }
         
        }
        public void üstSınır()
        {
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double[] dizi = new double[satırsayısı];
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value != "" || dataGridView1.Rows[j].Cells[i].Value == null)
                    {
                        dizi[j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                    }
                   

                }

                for (int k = 0; k < satırsayısı; k++)
                {
                    tut = dizi[k];

                    for (int m = 0; m < satırsayısı; m++)
                    {
                        if (dizi[k] < dizi[m])
                        {
                            tut2 = dizi[k];
                            dizi[k] = dizi[m];
                            dizi[m] = tut2;


                        }

                    }


                }

                for (int l = 0; l <= 7; l++)
                {
                    altdizi[l] = dizi[l];
                   
                    if (l == 7)
                    {
                        for (int y = 0; y <altdizi.Length; y++)
                        {
                            if (y != 0)
                            {
                                altdizi[y] = altdizi[altdizi.Length-1];
                                for (int p = 1; p <altdizi.Length; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = altdizi[y];
                                }
                             
                            }
                            else
                            {
                                dataGridView1.Rows[0].Cells[i].Value = altdizi[0];
                               
                            }
                        
                        }
                    }


                }


                for (int l = 8; l <= 15; l++)
                {
                    altdizi2[l - 8] = dizi[l];
                    
                    if (l == 15)
                    {

                        for (int y = 0; y <altdizi2.Length; y++)
                        {
                            if (y!=0)
                            {
                                altdizi2[y] = altdizi2[altdizi2.Length-1];
                                if (y==7)
                                {

                                for (int p = 9; p <= 15; p++)
                                {
                                    dataGridView1.Rows[p].Cells[i].Value = altdizi2[y];
                                }
                                    
                  
                                }
                            }
                            else
                            {
                              
                                dataGridView1.Rows[8].Cells[i].Value = altdizi2[0];
                             
                            }
                            

                        }
                    }
                }

                for (int l = 16; l <= 23; l++)
                {
                    altdizi3[l - 16] = dizi[l];

                    
                    if (l == 23)
                    {
                        for (int y = 0; y < altdizi3.Length; y++)
                        {
                            if (y != 0)
                            {
                              altdizi3[y] = altdizi3[altdizi3.Length - 1];
                                if (y == 7)
                                {

                                    for (int p = 17; p <= 23; p++)
                                    {
                                        dataGridView1.Rows[p].Cells[i].Value = altdizi3[y];
                                    }


                                }
                            }
                            else
                            {
                                dataGridView1.Rows[16].Cells[i].Value = altdizi3[0];
                                
                            }
                           
                        }
                    }
                }

                for (int l = 24; l <= 31; l++)
                {
                    altdizi4[l - 24] = dizi[l];
                    
                    if (l == 31)
                    {
                        
                        for (int y = 0; y < altdizi4.Length; y++)
                        {
                            if (y != 0)
                            {
                                altdizi4[y] = altdizi4[altdizi3.Length - 1];
                                if (y == 7)
                                {

                                    for (int p = 25; p <= 31; p++)
                                    {
                                        dataGridView1.Rows[p].Cells[i].Value = altdizi4[y];
                                    }


                                }
                            }
                            else
                            {
                                dataGridView1.Rows[24].Cells[i].Value = altdizi4[0];
                             
                            }
                        }
                    }
                }





            }


        }
        public void altSınır()
        {
            int sutunsayısı = dataGridView1.ColumnCount;
            int satırsayısı = dataGridView1.RowCount - 2;
            double[] dizi = new double[satırsayısı];
            for (i = 0; i < sutunsayısı; i++)
            {
                for (j = 0; j < satırsayısı; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value != "" || dataGridView1.Rows[j].Cells[i].Value ==null)
                    {
                        dizi[j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                    }
                    

                }

                for (int k = 0; k < satırsayısı; k++)
                {
                    tut = dizi[k];

                    for (int m = 0; m < satırsayısı; m++)
                    {
                        if (dizi[k] < dizi[m])
                        {
                            tut2 = dizi[k];
                            dizi[k] = dizi[m];
                            dizi[m] = tut2;


                        }

                    }


                }

                for (int l = 0; l <= 7; l++)
                {
                    altdizi[l] = dizi[l];

                    if (l == 7)
                    {
                        for (int y = 0; y < altdizi.Length; y++)
                        {
                            if (y!=7)
                            {
                                altdizi[y] = altdizi[(altdizi.Length - 1) - l];
                                if (y==6)
                                {
                                        for (int p = 0; p <= 6; p++)
                                        {
                                            dataGridView1.Rows[p].Cells[i].Value = altdizi[y];
                                        }
                                    
                                }
                            }
                            else
                            {
                                dataGridView1.Rows[7].Cells[i].Value = altdizi[altdizi.Length-1];
                               
                            }
                        
                        }
                    }


                }


                for (int l = 8; l <= 15; l++)
                {
                    altdizi2[l - 8] = dizi[l];

                    if (l == 15)
                    {

                        for (int y = 0; y < altdizi2.Length; y++)
                        {
                            if (y!=7)
                            {
                                altdizi2[y] = altdizi2[0];
                                if (y == 6)
                                {
                                    for (int p = 8; p <= 14; p++)
                                    {
                                        dataGridView1.Rows[p].Cells[i].Value = altdizi2[y];
                                    }

                                }
                            }
                            else
                            {
                                dataGridView1.Rows[15].Cells[i].Value = altdizi2[y];
                               
                            }


                        }
                    }
                }

                for (int l = 16; l <= 23; l++)
                {
                    altdizi3[l - 16] = dizi[l];


                    if (l == 23)
                    {
                        for (int y = 0; y < altdizi3.Length; y++)
                        {
                            if (y!=7)
                            {
                            altdizi3[y] = altdizi3[0];
                                if (y == 6)
                                {
                                    for (int p = 16; p <= 22; p++)
                                    {
                                        dataGridView1.Rows[p].Cells[i].Value = altdizi3[y];
                                    }

                                }
                            }
                            else
                            {
                                dataGridView1.Rows[23].Cells[i].Value = altdizi3[y];
                              
                            }

                        }
                    }
                }

                for (int l = 24; l <= 31; l++)
                {
                    altdizi4[l - 24] = dizi[l];

                    if (l == 31)
                    {
                        for (int y = 0; y < altdizi4.Length; y++)
                        {
                            if (y != 7)
                            {
                                altdizi4[y] = altdizi4[0];
                                if (y == 6)
                                {
                                    for (int p = 24; p <= 30; p++)
                                    {
                                        dataGridView1.Rows[p].Cells[i].Value = altdizi4[y];
                                    }

                                }
                            }
                            else
                            {
                                dataGridView1.Rows[31].Cells[i].Value = altdizi4[y];
                               

                            }
                        }
                    }
                }





            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel|*.xlsx";
            save.OverwritePrompt = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
            }

        }
    }
}