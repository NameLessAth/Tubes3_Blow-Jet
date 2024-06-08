using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;

namespace src
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private String opsi_pencarian;
        public MainForm()
        {
            Directory.SetCurrentDirectory("../../");
            List<(String, String)> temp = new List<(string, string)>();
            var connection = new SQLiteConnection("Data Source=db/test.db;Version=3;Journal Mode=Off",true);
            try
            {
                connection.Open();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            // Define the SQL query
            string query = "SELECT * FROM sidik_jari";

            // Create a command
            var command = new SQLiteCommand(query, connection);

            // Execute the query and get a data reader
            var reader = command.ExecuteReader();
                
            // Read the data
            while (reader.Read())
            {
                // Example of reading columns by name
                string berkas = reader.GetString(reader.GetOrdinal("berkas_citra"));
                string name = reader.GetString(reader.GetOrdinal("nama"));
                // Console.WriteLine(berkas + " " + name);
                temp.Add((berkas, name));
            }

            //foreach ((String,String) s in temp)
            //{
            //    Console.WriteLine(s.Item1, s.Item2);
            //}
            
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
                openFileDialog.Filter = "Image files (*.JPG;*.JPEG;*.PNG;*.BMP)|*.JPG;*.JPEG;*.PNG;*.BMP";

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.ImageLocation is null)
            {
                MessageBox.Show("Citra Sidik Jari belum dipilih");
                return;
            }

            var timer = new Stopwatch();
            timer.Start();
            (String, double) nama = ("",0.0);
            Console.WriteLine(pictureBox2.ImageLocation);
            nama = Converter.selectBerkasFromFingerprint(pictureBox2.ImageLocation, this.opsi_pencarian);
            timer.Stop();

            pictureBox1.ImageLocation = nama.Item1;
            String persentase = (nama.Item2 * 100).ToString() + "%";

            TimeSpan elapsed = timer.Elapsed;
            String waktu = elapsed.ToString(@"m\:ss\.fff");

            label2.Text = "Persentase kecocokan : " + persentase;
            label1.Text = "Waktu pencarian : " + waktu;

        }
    }
}
