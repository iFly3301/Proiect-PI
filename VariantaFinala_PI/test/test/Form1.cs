using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace test
{
    public partial class Form1 : Form
    {

        //TipCont 0 - utilizator Basic
        //TipCont 1 - utilizator Redactor
        //TipCont 2 - Administrator

        private SqlConnection con;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;MultipleActiveResultSets=True;");
            con.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login(con);
            login.Show();
            this.Hide();
            login.FormClosed += (a, b) =>
            {
                this.Show();        
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register(con);
            register.Show();
            this.Hide();
            register.FormClosed += (a, b) =>
            {
                this.Show();
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("Icon.png");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
