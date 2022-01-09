using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace test
{
    public partial class Redactare : Form
    {
        SqlConnection con;
        Utilizator utilizator;
        public Redactare(SqlConnection con, Utilizator utilizator)
        {
            InitializeComponent();
            this.con = con;
            this.utilizator = utilizator;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd;
            cmd = new SqlCommand("INSERT INTO Papers(Denumire, Editie, Pret) VALUES(@denumire, @editie, @pret)", con);
            cmd.Parameters.AddWithValue("denumire", textBox5.Text);
            cmd.Parameters.AddWithValue("editie", Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("pret", Convert.ToInt32(textBox4.Text));
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("INSERT INTO Ziar(Continut, Titlu, Data, Editie) VALUES(@continut, @titlu, @data, @editie)", con);
            cmd.Parameters.AddWithValue("continut", textBox3.Text);
            cmd.Parameters.AddWithValue("titlu", textBox1.Text);
            cmd.Parameters.AddWithValue("data", DateTime.Now);
            cmd.Parameters.AddWithValue("editie", Convert.ToInt32(textBox2.Text));
            cmd.ExecuteNonQuery();
            MessageBox.Show("Cartea a fost adaugata!");
        }
    }
}
