using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _1103
{
    public partial class FormInfo : Form
    {
        private int row;
        private bool isHistoryChanged;

        public FormInfo(int row)
        {
            InitializeComponent();
            this.row = row;
            isHistoryChanged = false;
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Server=(local);Integrated security=SSPI;database=master");
            con.Open();            
            SqlCommand com = new SqlCommand($"SELECT * FROM [{Form1.tableName}] where id={row}", con);
            try
            {
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    if (File.Exists(reader["ImgPath"].ToString()))
                    {
                        pbPhoto.Image = new Bitmap(reader["ImgPath"].ToString());
                    }
                    else
                    {
                        pbPhoto.Image = null;
                    }
                    lName.Text = reader["Name"].ToString();
                    lFam.Text = reader["Surname"].ToString();
                    lGen.Text = reader["Gender"].ToString();
                    rtbDiag.Text = reader["Diagnosis"].ToString();
                    rtbHis.Text = reader["History"].ToString();
                    if (rtbDiag.Text == "") rtbDiag.Text = "Диагноз отсутствует.";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }

        private void rtbHis_TextChanged(object sender, EventArgs e)
        {
            isHistoryChanged = true;
        }

        private void FormInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isHistoryChanged)
            {
                SqlConnection con = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");
                con.Open();
                SqlCommand com = new SqlCommand($"UPDATE {Form1.tableName} SET History = '{rtbHis.Text}' where id = {row}", con);
                try
                {
                    com.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.Close();
            }
        }
    }
}
