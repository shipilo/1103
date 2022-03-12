using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1103
{
    public partial class FormAddNew : Form
    {
        private Form1 form1;

        public FormAddNew(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            string genValue = "Муж";
            if (rbFemale.Checked) genValue = "Жен";
            form1.DateBaseCommand("INSERT INTO [Main1] (Name, Surname, Gender, Birthday) VALUES (N'" +
                tbName.Text + "','" +
                tbSur.Text + "','" +
                genValue + "','" +
                dateTimePicker1.Value + "')");
            this.Close();
        }
    }
}
