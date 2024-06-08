using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StringConversion;

namespace src
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private String opsi_pencarian;
        public MainForm()
        {
            StringConversion.Program.test();
            this.opsi_pencarian = "BM";
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.opsi_pencarian = "BM";
            // MessageBox.Show(this.opsi_pencarian);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.opsi_pencarian = "KMP";
            // MessageBox.Show(this.opsi_pencarian);
        }

        private void pilihButton_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.CheckFileExists = true;
                openFileDialog.AddExtension = true;
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Image files (*.JPG;*.JPEG;*.PNG)|*.JPG;*.JPEG;*.PNG";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = openFileDialog.FileName;

                    pictureBox2.ImageLocation = imageLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
            
        }
    }
}
