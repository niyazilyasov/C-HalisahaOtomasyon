using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace NIYAZ_ILYASOV_HALISAHAOTAMASYONU_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=rezerve.accdb");
        OleDbCommand komut;
        OleDbDataAdapter adptr;
        DataSet ds;
        DataTable dt;
        Button b;
        string secilen = "";
        void yukle()
        {
            baglanti.Open();
            DataSet ds = new DataSet();
            adptr = new OleDbDataAdapter("select *from rezerve", baglanti);
            adptr.Fill(ds, "rezerve");
            dataGridView1.DataSource = ds.Tables["rezerve"];
            adptr.Dispose();
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //comboxa sahalari atadim
            OleDbCommand komut = new OleDbCommand();
            komut.CommandText = "SELECT *from saha";
            komut.Connection = baglanti;
            OleDbDataReader rd;
            baglanti.Open();
            rd = komut.ExecuteReader();
            while (rd.Read())
            {
                comboBox1.Items.Add(rd["saha_id"]);
            }
            baglanti.Close();


        }
        void temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            temizle();
            if (comboBox1.SelectedIndex != 0)
            {


                baglanti.Open();
                komut = new OleDbCommand("select ID from rezerve", baglanti);
                adptr = new OleDbDataAdapter(komut);
                dt = new DataTable();
                OleDbDataReader rd = komut.ExecuteReader();
                while (rd.Read())
                {
                    foreach (Control item in panel1.Controls)
                    {
                        if (item is Button)
                        {
                            if (rd["ID"].ToString() == comboBox1.SelectedValue + item.Name.ToString())
                            {
                                item.BackColor = Color.Red;
                            }
                        }
                    }
                }
                baglanti.Close();
            }

        }

        private void button72_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            temizle();
            b = sender as Button;
            secilen = b.Name.ToString();
            button71.Text = secilen + " REZERVASYONU YAP";
            if (comboBox1.SelectedIndex != 0)
            {
               textBox4.Text = b.Name.ToString().ToUpper();
                if (b.BackColor == Color.Red) 
                {
                   
                    button71.BackColor = Color.YellowGreen;
                    button71.Enabled = false;
                    baglanti.Open();
                    komut = new OleDbCommand("select * from rezerve where ID", baglanti);
                    adptr = new OleDbDataAdapter(komut);
                    dt = new DataTable();
                    OleDbDataReader rd = komut.ExecuteReader();
                    while (rd.Read())
                    {
                        textBox1.Text = rd["adsoyad"].ToString();
                        textBox2.Text = rd["telefon"].ToString();
                        textBox3.Text = rd["ucret"].ToString();
                    }
                    baglanti.Close();
                }
                else // satılmadıysa yeşilse
                {
                    button71.Enabled = true; // rezervasyon butonunu aktif et
                    button71.BackColor = Color.YellowGreen;
                   
                }
            }

        }

        private void button71_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "") // textboxlar boş değilse
            {
                baglanti.Open(); // veri tabanına bilgileri kaydet
                komut = new OleDbCommand("insert into rezerve(adsoyad,telefon,ucret,saha,gun)values('"+ textBox1.Text +"','"+ textBox2.Text +"','"+ textBox3.Text +"','"+ comboBox1.Text +"','"+ textBox4.Text +"')", baglanti);
                komut.ExecuteReader();
                b.BackColor = Color.Red;
                MessageBox.Show(secilen + " Rezervasyon Yapıldı");
                button71.Enabled = false;
                button71.BackColor = Color.Green;           
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Boş alan bırakmayınız. ");
            }
            temizle();
            yukle();
        }

        private void button73_Click(object sender, EventArgs e)//gunlle butonu
        {
            baglanti.Open();
                int secili = (int)dataGridView1.CurrentRow.Cells[0].Value;
                string sorgu = "update rezerve set adsoyad='" + textBox1.Text + "',telefon='"+ textBox2.Text +"',ucret='"+ textBox3.Text +"',saha='"+ comboBox1.Text +"',gun='"+ textBox4.Text +"'where ID=" + secili;
                OleDbCommand komut = new OleDbCommand(sorgu, baglanti);               
                komut.ExecuteNonQuery();

                baglanti.Close();
                yukle();
                temizle();
      


        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        

      

        

       

        

    }
}
