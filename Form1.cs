using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _1103
{
    public partial class Form1 : Form
    {
        private int row;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //DateBaseCommand("SELECT * FROM[Main]");

            String str;
            SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

            str = "CREATE DATABASE MyDatabase ON PRIMARY " +
             "(NAME = MyDatabase_Data, " +
             "FILENAME = 'MyDatabaseData.mdf', " +
             "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
             "LOG ON (NAME = MyDatabase_Log, " +
             "FILENAME = 'MyDatabaseLog.ldf', " +
             "SIZE = 1MB, " +
             "MAXSIZE = 5MB, " +
             "FILEGROWTH = 10%)";

            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void DateBaseCommand(string command)
        {
            SqlConnection sqlCon = new SqlConnection("Server=localhost;Integrated security=SSPI;database=PatientsBD");
            sqlCon.Open();

            if (command.Split()[0].ToLower().Equals("insert"))
            {
                SqlCommand sqlCom1 = new SqlCommand(command, sqlCon);
                MessageBox.Show(sqlCom1.ExecuteNonQuery().ToString());
            }

            SqlCommand sqlCom2 = new SqlCommand("SELECT * FROM [Main1]", sqlCon);
            SqlDataReader sqlReader = sqlCom2.ExecuteReader();

            dgvTable.Rows.Clear();

            while (sqlReader.Read())
            {
                dgvTable.Rows.Add(sqlReader["id"], sqlReader["Name"], sqlReader["Surname"], sqlReader["Gender"], Convert.ToDateTime(sqlReader["Birthday"]).ToString("MMMM dd, yyyy"));
            }
            sqlReader.Close();
            sqlCon.Close();
            sqlCon.Dispose();
        }

        private void bAddNew_Click(object sender, EventArgs e)
        {
            FormAddNew form = new FormAddNew(this);
            form.Show();
        }

        private void bInfo_Click(object sender, EventArgs e)
        {
            FormInfo form = new FormInfo(row);
            form.Show();
        }

        private void dgvTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            row = e.RowIndex;
            if (row != -1 && dgvTable.Rows[row].Cells[0].Value != null)
            {
                if (dgvTable.Rows[row].Cells[0].Value.ToString() == "1")
                {
                    pbPhoto.Image = _1103.Properties.Resources.img_709x945x24_0214;
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
