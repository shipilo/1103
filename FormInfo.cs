using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1103
{
    public partial class FormInfo : Form
    {
        private int row;

        public FormInfo(int row)
        {
            InitializeComponent();
            this.row = row;
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection("Server=localhost;Integrated security=SSPI;database=PatientsBD");
            sqlCon.Open();
            
            SqlCommand sqlCom1 = new SqlCommand($"SELECT * FROM [Main1] where id={row}", sqlCon);
            SqlDataReader sqlReader = sqlCom1.ExecuteReader();

            while (sqlReader.Read())
            {
                lName.Text = sqlReader["Name"].ToString();
                lFam.Text = sqlReader["Surname"].ToString();
                lGen.Text = sqlReader["Gender"].ToString();
            }

            sqlReader.Close();


            SqlCommand sqlCom2 = new SqlCommand($"SELECT * FROM [Main2] where id={row}", sqlCon);
            sqlReader = sqlCom2.ExecuteReader();

            while (sqlReader.Read())
            {
                rtbDiag.Text = sqlReader["Diagnosis"].ToString();
                rtbHis.Text = sqlReader["History"].ToString();
            }

            sqlReader.Close();
            sqlCon.Close();
            sqlCon.Dispose();

            if (row == 1)
            {
                pbPhoto.Image = _1103.Properties.Resources.img_709x945x24_0214;
            }
            else
            {
                pbPhoto.Image = null;
            }
        }
    }
}
