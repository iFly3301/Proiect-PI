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
    public partial class Register : Form
    {
        private SqlConnection con;
        public bool Ok = false;
        private string code;

        public Register(SqlConnection con)
        {
            InitializeComponent();
            this.con = con;
            initCaptcha();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void initCaptcha()
        {
            textBox6.Text = "";
            pictureBox2.Image = null;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            List<string> imgs = new List<string>();
            foreach (var file in Directory.GetFiles("Captcha"))
                imgs.Add(file);
            Random rnd = new Random();
            var img = imgs[rnd.Next(0, imgs.Count)];
            pictureBox2.Image = Image.FromFile(img);
            code = img.Substring(img.LastIndexOf('\\') + 1);
            code = code.Substring(0, code.IndexOf('.'));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool ok = true;
            foreach (var control in this.Controls)
                if (control is TextBox && (control as TextBox).TextLength == 0)
                {
                    MessageBox.Show("Campurile nu pot fi vide !");
                    ok = false;
                    break;
                }
            if (ok)
            {
                if (user_exista(textBox3.Text))
                    MessageBox.Show("Utilizator deja existent !");
                else
                {
                    if (validareEmail(textBox3.Text))
                    {
                        if (textBox5.Text != textBox4.Text || textBox4.TextLength < 3)
                            MessageBox.Show("Parolele nu corespund sau nu au minim 3 caractere!");
                        else
                        {
                            if (textBox6.Text != code)
                            {
                                MessageBox.Show("Code Captcha Invalid");
                                initCaptcha();
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand("INSERT INTO Utilizatori(Nume, Prenume, Parola, Email, Adresa) VALUES(@nume, @prenume, @parola, @email, @adresa)", con);
                                cmd.Parameters.AddWithValue("@nume", textBox1.Text);
                                cmd.Parameters.AddWithValue("@prenume", textBox2.Text);
                                cmd.Parameters.AddWithValue("@parola", textBox4.Text);
                                cmd.Parameters.AddWithValue("@email", textBox3.Text);
                                cmd.Parameters.AddWithValue("@adresa", textBox7.Text);
                                int i = cmd.ExecuteNonQuery();
                                if (i == 1)
                                {
                                    MessageBox.Show("Sucessfully added !");
                                    Ok = true;
                                    this.Close();
                                }
                                else
                                    MessageBox.Show("Something went wrong !");
                                cmd.Dispose();
                            }
                        }
                    }
                    else MessageBox.Show("Email invalid !");
                }
            }
        }

        bool user_exista(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Utilizatori WHERE Email = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader red = cmd.ExecuteReader();
            bool ok = red.Read();
            red.Dispose();
            cmd.Dispose();
            if (ok)
                return true;
            return false;
        }

        bool validareEmail(string email)
        {
            if (email.Count(x => x == '@') != 1)
                return false;
            var ind = email.IndexOf('@');
            email = email.Substring(ind + 1);
            if (email.Count(x => x == '.') != 1)
                return false;
            ind = email.IndexOf('.');
            if (ind == 0 || ind == email.Length - 1)
                return false;
            return true;
        }
    }
}
