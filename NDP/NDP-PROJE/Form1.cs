using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
/****************************************************************************
** SAKARYA ÜNİVERSİTESİ
** BİLGİSAYAR VE BİLİŞİM BİLİMLERİ FAKÜLTESİ
** BİLİŞİM SİSTEMLERİ MÜHENDİSLİĞİ BÖLÜMÜ
** NESNEYE DAYALI PROGRAMLAMA DERSİ
** 2019-2020 BAHAR DÖNEMİ
**
** PROJE NUMARASI.........: 01
** ÖĞRENCİ ADI............:Batuhan Öksüz
** ÖĞRENCİ NUMARASI.......:B191200003
** DERSİN ALINDIĞI GRUP...: A
****************************************************************************/
//Provider=Microsoft.Jet.OLEDB.4.0;Data Source="C:\Users\Batuhan\Desktop\NDP PROJE\Musteri.mdb"
namespace NDP_PROJE
{
   
    public partial class Form1 : Form
    {
        //Burası yapım aşamasında kullanıldı sıkıntı olursa diye saklanıyor.
        //OleDbConnection bilgi = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = C:\\Users\\Batuhan\\Desktop\\NDP PROJE\\Musteri.mdb");

        OleDbConnection bilgi = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =musteri.mdb");

        public Form1()
        {
            InitializeComponent();
        }

        public void veriler()
        {
            listView1.Items.Clear();

            bilgi.Open();

            OleDbCommand komut = new OleDbCommand();

            komut.Connection = bilgi;

            komut.CommandText = ("Select * From Musteri");

            OleDbDataReader oku = komut.ExecuteReader();

            

            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();

                ekle.Text = oku["Ad"].ToString();

                ekle.SubItems.Add(oku["Soyad"].ToString());

                ekle.SubItems.Add(oku["id"].ToString());

                ekle.SubItems.Add(oku["ipara"].ToString());

                ekle.SubItems.Add(oku["spara"].ToString());

                ekle.SubItems.Add(oku["ytarih"].ToString());

                listView1.Items.Add(ekle);

            }

            bilgi.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            veriler();

            faizislem();
            

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            veriler();

            MusteriBilgi musteri = new MusteriBilgi();

            musteri.ad = ad1.Text;

            musteri.soyad = soyad1.Text;

            musteri.tc = tc1.Text;

            musteri.para = para1.Text;

            musteri.faizpara =para1.Text;

            DateTime z = DateTime.Today;
            int yil, ay, gun;
            yil = z.Year;
            ay = z.Month;
            gun = z.Day;

            string[] liste = { musteri.ad, musteri.soyad, musteri.tc, musteri.para, musteri.faizpara, gun + "." + ay + "." + yil };

            ListViewItem lst = new ListViewItem(liste);

            if (musteri.ad!="" && musteri.soyad!="" && musteri.tc!=""&&musteri.para!="")
            {
                listView1.Items.Add(lst);

                bilgi.Open();

                string tarih = (gun + "." + ay + "." + yil).ToString();

                OleDbCommand komut = new OleDbCommand("insert into Musteri (Ad,Soyad,id,ipara,spara,ytarih) values ( '" + musteri.ad + "','" + musteri.soyad + "','" + musteri.tc + "','" + musteri.para + "','" + musteri.faizpara + "','" + tarih + "')", bilgi);
                
                komut.ExecuteNonQuery();
                
                bilgi.Close();

                ad1.Clear();
                soyad1.Clear();
                tc1.Clear();
                para1.Clear();
              

            }
            else
            {
                MessageBox.Show("Boş alanları Doldurunuz");
            }


           

        }
        public void faizislem()
        {
            DateTime z = DateTime.Today;

            int yil, ay, gun;
            yil = z.Year;
            ay = z.Month;
            gun = z.Day;

            int a,b,c,d,e;
            for(int i = 0;i < listView1.Items.Count;i++ )
            {

                DateTime bugun = Convert.ToDateTime(listView1.Items[i].SubItems[5].Text);

                DateTime ilktarih = Convert.ToDateTime(gun + "." + ay + "." + yil);

                TimeSpan fark = ilktarih - bugun;

               b = Convert.ToInt32(fark.TotalDays);

                c = Convert.ToInt32(listView1.Items[i].SubItems[3].Text);

                //8 faiz oranıdır
                d = (c * 8 * b) / 36500;
                
                c = d + c;
                listView1.Items[i].SubItems[4].Text = c.ToString();
            }
        }

     

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                bilgi.Open();
                OleDbCommand komut = new OleDbCommand("DELETE FROM Musteri WHERE Ad  ='" + listView1.SelectedItems[0].Text + "'", bilgi);
                komut.ExecuteNonQuery();
                bilgi.Close();
                veriler();
            }
            else
            {
                MessageBox.Show("Lütfen Birini Seçin");
            }

        }
    }
}
