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
    public partial class Login : Form
    {
        private SqlConnection con;
        private string code;

        public Login(SqlConnection con)
        {
            InitializeComponent();
            this.con = con;
            textBox1.Text = Properties.Settings.Default.Email;
            checkBox1.Checked = textBox1.Text != "";
            textBox1.TabStop = false;
            initCaptcha();
        }

        void initCaptcha()
        {
            textBox3.Text = "";
            pictureBox1.Image = null;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            List<string> imgs = new List<string>();
            foreach (var file in Directory.GetFiles("Captcha"))
                imgs.Add(file);
            Random rnd = new Random();
            var img = imgs[rnd.Next(0, imgs.Count)];
            pictureBox1.Image = Image.FromFile(img);
            code = img.Substring(img.LastIndexOf('\\') + 1);
            code = code.Substring(0, code.IndexOf('.'));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Utilizatori WHERE Email = @email AND Parola = @parola", con);
            cmd.Parameters.AddWithValue("@email", textBox1.Text);
            cmd.Parameters.AddWithValue("@parola", textBox2.Text);
            SqlDataReader red = cmd.ExecuteReader();
            if (textBox3.Text == code)
            {
                if (red.Read())
                {
                    save_property();
                    textBox1.Text = textBox2.Text = "";
                    int id = Convert.ToInt32(red[0]);
                    red.Dispose();
                    cmd.Dispose();
                    Panel panel = new Panel(con, new Utilizator(con, id));
                    panel.Show();
                    this.Hide();
                    panel.FormClosed += (a, b) =>
                    {
                        textBox1.Text = Properties.Settings.Default.Email;
                        checkBox1.Checked = textBox1.Text != "";
                        this.Show();
                    };
                }
                else
                {
                    MessageBox.Show("Wrong user or password !", "Logare");
                }
            }
            else {
                MessageBox.Show("Code Captcha Invalid");
                initCaptcha();
            }
            
        }

        void save_property()
        {
            if (checkBox1.Checked == true)
                Properties.Settings.Default.Email = textBox1.Text;
            else
                Properties.Settings.Default.Email = "";
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            save_property();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
