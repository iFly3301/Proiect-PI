using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace test
{
    public class Utilizator
    {
        public int ID { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email { get; set; }
        public string Adresa { get; set; }
        public int TipCont { get; set; }

        public Utilizator(SqlConnection con, int id)
        {
            this.ID = id;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Utilizatori WHERE IdUtilizator = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            var red = cmd.ExecuteReader();
            if (red.Read())
            {
                this.Nume = red[1].ToString();
                this.Prenume = red[2].ToString();
                this.Email = red[4].ToString();
                this.Adresa = red[5].ToString();
                this.TipCont = Convert.ToInt32(red[6]);
            }
            red.Dispose();
            cmd.Dispose();
        }
    }
}
