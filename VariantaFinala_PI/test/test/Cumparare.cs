using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace test
{
    public partial class Cumparare : Form
    {
        string id, denumire, editie1, pret;
        private SqlConnection con;
        private Utilizator utilizator;

        string titlu, continut, editie, data;

        public Cumparare(SqlConnection con, Utilizator utilizator, string id, string denumire, string editie1, string pret)
        {
            InitializeComponent();
            this.id = id;
            this.denumire = denumire;
            this.editie1 = editie1;
            this.pret = pret;
            this.con = con;
            this.utilizator = utilizator;
            load();
        }

        void update()
        {
            label5.Text = "Pret final: " + numericUpDown1.Value.ToString() + " X " + pret + " = " + (Convert.ToDouble(pret) * (double)numericUpDown1.Value).ToString() + " Lei";
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            update();
        }

        private void load()
        {
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 100;
            label1.Text += " " + denumire;
            label2.Text += " " + editie1;
            label4.Text += " " + pret;
            update();
        }

        void selectare()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Ziar WHERE IdZiar = @id",con);
            cmd.Parameters.AddWithValue("id", Convert.ToInt32(id));
            var red = cmd.ExecuteReader();
            if (red.Read())
            {
                titlu = red[2].ToString();
                data = red[3].ToString();
                continut = red[1].ToString();
                editie = red[4].ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            selectare();
            RectangleF rectangle2 = new RectangleF(new PointF(10.0F, 490F), new SizeF(700.0F, 500.0F));
            PrintPreviewDialog p = new PrintPreviewDialog();
            p.Document = new PrintDocument();
            p.Document.PrintPage += (a, b) =>
            {
                b.Graphics.DrawString("======================================", DefaultFont, Brushes.Black, 10, 10);
                b.Graphics.DrawString("Nume: " + utilizator.Nume, DefaultFont, Brushes.Black, 10, 60);
                b.Graphics.DrawString("Email: " + utilizator.Email, DefaultFont, Brushes.Black, 10, 120);
                b.Graphics.DrawString("Adresa: " + utilizator.Adresa, DefaultFont, Brushes.Black, 10, 180);
                b.Graphics.DrawString("======================================", DefaultFont, Brushes.Black, 10, 210);
                b.Graphics.DrawString("Denumire carte: " + denumire, DefaultFont, Brushes.Black, 10, 260);
                b.Graphics.DrawString("Cantitate:  " + numericUpDown1.Value.ToString(), DefaultFont, Brushes.Black, 10, 320);
                b.Graphics.DrawString("Pret final: " + ((int)numericUpDown1.Value * Convert.ToDouble(pret)).ToString() + " LEI", DefaultFont, Brushes.Black, 10, 380);
                b.Graphics.DrawString("======================================", DefaultFont, Brushes.Black, 10, 410);
                b.Graphics.DrawString("Titlu: " + titlu, DefaultFont, Brushes.Black, 10, 450);
                b.Graphics.DrawString("Continut: \n" + continut, DefaultFont, Brushes.Black, rectangle2);
                b.Graphics.DrawString("Editie: " + editie, DefaultFont, Brushes.Black, 10, 780);
                b.Graphics.DrawString("Data Aparitie: " + data, DefaultFont, Brushes.Black, 10, 810);
                b.Graphics.DrawString("======================================", DefaultFont, Brushes.Black, 10, 840);
            };
            p.ShowDialog();
        }
    }
}
