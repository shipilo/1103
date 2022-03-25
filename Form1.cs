using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace _1103
{
    public partial class Form1 : Form
    {
        private int row;
        public static string tableName;
        public SqlConnection con;
        public Dictionary<int, string> imgPaths;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection("Server=(local);Integrated security=SSPI;database=master");
            imgPaths = new Dictionary<int, string>();
            tableName = "Patients_shipilo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool dbIsExist = false;
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SqlCommand com1 = new SqlCommand("SELECT CONVERT(BIT, COUNT(*)) FROM sys.tables WHERE name = N'" + tableName + "'", con);
            SqlDataReader reader = null;
            try
            {
                reader = com1.ExecuteReader();
                reader.Read();
                dbIsExist = Convert.ToBoolean(reader[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (reader != null) reader.Close();
            if (!dbIsExist)
            {
                SqlCommand com2 = new SqlCommand(
                    "CREATE TABLE " + tableName +
                    "(id INT PRIMARY KEY IDENTITY (1,1) NOT NULL," +
                    "Name NVARCHAR(30)," +
                    "Surname NVARCHAR(30)," +
                    "Gender NVARCHAR(30)," +
                    "Birth DATE," +
                    "ImgPath NVARCHAR(max)," +
                    "Diagnosis NVARCHAR(30)," +
                    "History NVARCHAR(30))", con);
                try
                {
                    com2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (dbIsExist)
            {
                SqlCommand com3 = new SqlCommand("SELECT * FROM " + tableName, con);
                dgvTable.Rows.Clear();
                try
                {
                    reader = com3.ExecuteReader();
                    while (reader.Read())
                    {
                        dgvTable.Rows.Add(reader["id"], reader["Name"], reader["Surname"], reader["Gender"], Convert.ToDateTime(reader["Birth"]).ToString("MMMM dd, yyyy"));
                        imgPaths.Add(Convert.ToInt32(reader["id"]), reader["ImgPath"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if(reader != null) reader.Close();
            con.Close();
        }

        private void bAddNew_Click(object sender, EventArgs e)
        {
            FormAddNew form = new FormAddNew(this);
            form.Show();
        }

        private void bInfo_Click(object sender, EventArgs e)
        {
            FormInfo form = new FormInfo(row + 1);
            form.Show();
        }

        private void dgvTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            row = e.RowIndex;
            if (row != -1 && dgvTable.Rows[row].Cells[0].Value != null)
            {
                if (File.Exists(imgPaths[(int)dgvTable.Rows[row].Cells[0].Value]))
                {
                    pbPhoto.Image = new Bitmap(imgPaths[(int)dgvTable.Rows[row].Cells[0].Value]);
                }
                else
                {
                    pbPhoto.Image = null;
                }
                lName.Text = dgvTable.Rows[row].Cells[1].Value.ToString();
                lSur.Text = dgvTable.Rows[row].Cells[2].Value.ToString();
                lGen.Text = dgvTable.Rows[row].Cells[3].Value.ToString();
                DateTime now = DateTime.Today;
                DateTime bday = Convert.ToDateTime(dgvTable.Rows[row].Cells[4].Value.ToString());
                int age = now.Year - bday.Year;
                if (bday > now.AddYears(-age)) age--;
                lAge.Text = age.ToString();
            }
        }
    }
}
