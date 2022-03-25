using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _1103
{
    public partial class FormAddNew : Form
    {
        private Form1 form1;
        private string imgPath;

        public FormAddNew(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            imgPath = "";
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            bool done = false;
            string genValue = "Муж";
            if (rbFemale.Checked) genValue = "Жен";

            SqlConnection con = new SqlConnection("Server=(local);Integrated security=SSPI;database=master");
            con.Open();
            SqlCommand com1 = new SqlCommand("INSERT INTO " + Form1.tableName + " (Name, Surname, Gender, Birth, ImgPath, Diagnosis) " +
                $"VALUES (N'{tbName.Text}', N'{tbSur.Text}', N'{genValue}', N'{dateTimePicker1.Value}', N'{imgPath}', N'{rtbDiag.Text}')", con); 
            try
            {
                com1.ExecuteReader();
                done = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
            con.Open();
            if (done)
            {
                SqlCommand com2 = new SqlCommand("SELECT MAX(id) from " + Form1.tableName, con);
                try
                {
                    int сount = Convert.ToInt32(com2.ExecuteScalar());
                    form1.dgvTable.Rows.Add(сount, tbName.Text, tbSur.Text, genValue, Convert.ToDateTime(dateTimePicker1.Value).ToString("MMMM dd, yyyy"));
                    form1.imgPaths.Add(сount, imgPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            con.Close();
            this.Close();
        }

        private void bOpenImgDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg*.png|Все файлы (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                imgPath = fileDialog.FileName;
                lImgPath.Text = imgPath;
            }
        }
    }
}
