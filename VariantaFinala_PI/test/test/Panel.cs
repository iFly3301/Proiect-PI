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
    public partial class Panel : Form
    {
        private readonly SqlConnection con;
        private readonly Utilizator utilizator;
        int primul_id, ultimul_id;
        public Panel(SqlConnection con, Utilizator utilizator)
        {
            InitializeComponent();
            this.utilizator = utilizator;
            this.con = con;
            dataGridView1.CellClick += (a, b) =>
            {
                dataGridView1.CurrentRow.Selected = true;
                textBox1.Text = dataGridView1.Rows[b.RowIndex].Cells[0].FormattedValue.ToString();
                textBox2.Text = dataGridView1.Rows[b.RowIndex].Cells[1].FormattedValue.ToString();
                textBox3.Text = dataGridView1.Rows[b.RowIndex].Cells[2].FormattedValue.ToString();
                textBox4.Text = dataGridView1.Rows[b.RowIndex].Cells[3].FormattedValue.ToString();
            };

            iesireToolStripMenuItem.Text = utilizator.Email;
            if (utilizator.TipCont == 1)
            {
                button5.Visible = true;
                button3.Visible = true;
                button2.Visible = false;
                label6.Visible = true;
                button4.Visible = true;
            }
            else if (utilizator.TipCont == 0)
            {
                button2.Visible = true;
                button4.Visible = true;
                button2.Text = "CUMPARA";
            }
        }

        public void load()
        {
            SqlCommand cmd;
            dataGridView1.Rows.Clear();
            cmd = new SqlCommand("SELECT * FROM Papers", con);
            var red = cmd.ExecuteReader();
            while (red.Read())
            {
                dataGridView1.Rows.Add(red[0].ToString(), red[1], red[2], red[3]);
                ultimul_id = Convert.ToInt32(red[0]);
            }

            cmd = new SqlCommand("SELECT * FROM Papers", con);
            red = cmd.ExecuteReader();
            if (red.Read())
            {
                primul_id = Convert.ToInt32(red[0]);
                textBox1.Text = primul_id.ToString();
                textBox2.Text = red[1].ToString();
                textBox3.Text = red[2].ToString();
                textBox4.Text = red[3].ToString();
            }
        }

        private void Panel_Load(object sender, EventArgs e)
        {
            load();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cumparare cumpara = new Cumparare(con, utilizator, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            cumpara.Show();
            this.Hide();
            cumpara.FormClosed += (a, b) =>
            {
                this.Show();
            };
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Redactare redactare = new Redactare(con, utilizator);
            redactare.Show();
            this.Hide();
            redactare.FormClosed += (a, b) =>
            {
                this.Show();
                load();
                ultimul_id++;
            };
        }

        private void iesireToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void inchideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Doresti sa stergi o carte?"))
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow newDataRow = dataGridView1.Rows[rowIndex];
                SqlCommand cmd;
                cmd = new SqlCommand("DELETE FROM Papers WHERE IdZiar = @id", con);
                cmd.Parameters.AddWithValue("id", newDataRow.Cells[0].Value);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("DELETE FROM Ziar WHERE IdZiar = @id", con);
                cmd.Parameters.AddWithValue("id", newDataRow.Cells[0].Value);
                cmd.ExecuteNonQuery();
                dataGridView1.Rows.RemoveAt(rowIndex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Papers WHERE Denumire = @denumire", con);
            cmd.Parameters.AddWithValue("denumire", textBox5.Text);
            var red = cmd.ExecuteReader();
            if (red.Read())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(red[0], red[1], red[2], red[3]);
            }
            else MessageBox.Show("Cartea nu exista !");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            load();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
