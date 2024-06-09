using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using StringConversion;

namespace src
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private String opsi_pencarian;
        public MainForm()
        {

            Directory.SetCurrentDirectory("../../");

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

                    Console.WriteLine("Input " + imageLocation);
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

            reset_Display();
            var timer = new Stopwatch();
            timer.Start();
            (String, String, double) result = ("","",0.0);
            result = Converter.selectBerkasFromFingerprint(pictureBox2.ImageLocation, this.opsi_pencarian);
            timer.Stop();

            TimeSpan elapsed = timer.Elapsed;
            String waktu = elapsed.ToString(@"m\:ss\.fff");

            label1.Text = "Waktu pencarian : " + waktu;

            // if tidak ada yang cukup cocok
            if (result.Item3 <= 0.70)
            {
                MessageBox.Show("Sidik jari tidak ditemukan");
                return;
            }

            String persentase = (Math.Truncate(result.Item3 * 10000) / 100).ToString() + "%";
            label2.Text = "Persentase kecocokan : " + persentase;
            pictureBox1.ImageLocation = result.Item1;

            //NIK,  Nama,Tempat/Tgl,Kelamin,Darah,  Alamat, Agama,  Status, Kerja, Negara
            (String, String, String, String, String, String, String, String, String, String)
                biodata = search_Biodata(result.Item2);
            textBox1.Text = " List Biodata\r\n " +
                "NIK: " + biodata.Item1 + "\r\n " +
                "Nama: " + biodata.Item2 + "\r\n " +
                "Tempat/Tgl Lahir: " + biodata.Item3 + "\r\n " +
                "Jenis Kelamin: " + biodata.Item4 + "\r\n " +
                "Gol Darah: " + biodata.Item5 + "\r\n " +
                "Alamat: " + biodata.Item6 + "\r\n " +
                "Agama: " + biodata.Item7 + "\r\n " +
                "Status: " + biodata.Item8 + "\r\n " +
                "Pekerjaan: " + biodata.Item9 + "\r\n " +
                "Kewarganegaraan: " + biodata.Item10;

            Console.WriteLine("Finished");
        }

        private (String, String, String, String, String, String, String, String, String, String) search_Biodata(String nama)
        {
             //NIK,  Nama,Tempat/Tgl,Kelamin,Darah,  Alamat, Agama,  Status, Kerja, Negara
            (String, String, String, String, String, String, String, String, String, String)
                biodata = ("", "", "", "", "", "", "", "", "", "");
            using (var connection = new SQLiteConnection("Data Source=db/test.db;Version=3;Journal Mode=Off", true))
            {
                connection.Open();

                // Define the SQL query
                string query = "SELECT * FROM biodata";

                // Create a command
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and get a data reader
                    using (var reader = command.ExecuteReader())
                    {
                        // Read the data
                        while (reader.Read())
                        {
                            string name = reader.GetString(reader.GetOrdinal("nama"));

                            // fix alay angka dari data database
                            String patternAlay = "[0123456]|[a-z]";
                            string input = Regex.Replace(name, patternAlay, new MatchEvaluator(StringConversion.Program.replacement));

                            // modify pattern supaya match dengan singkatan
                            String patternSingkat = "[aiueo]";
                            string pattern = Regex.Replace(nama, patternSingkat, match => match+"?");

                            bool matchNama = Regex.IsMatch(input, pattern);
                            
                            if (matchNama)
                            {
                                string capitalize = Regex.Replace(nama.ToLower(), @"\b[a-z]", match => match.Value.ToUpper());

                                /*Console.WriteLine("Match!");
                                Console.WriteLine(capitalize);
                                Console.WriteLine(matchNama);*/

                                String nik = reader.GetString(reader.GetOrdinal("NIK"));
                                String tempat = reader.GetString(reader.GetOrdinal("tempat_lahir"));
                                String tanggal = reader.GetString(reader.GetOrdinal("tanggal_lahir"));
                                String kelamin = reader.GetString(reader.GetOrdinal("jenis_kelamin"));
                                String darah = reader.GetString(reader.GetOrdinal("golongan_darah"));
                                String alamat = reader.GetString(reader.GetOrdinal("alamat"));
                                String agama = reader.GetString(reader.GetOrdinal("agama"));
                                String status = reader.GetString(reader.GetOrdinal("status_perkawinan"));
                                String pekerjaan = reader.GetString(reader.GetOrdinal("pekerjaan"));
                                String kewarganegaraan = reader.GetString(reader.GetOrdinal("kewarganegaraan"));

                                biodata = (nik, capitalize, tempat + ", " + tanggal, kelamin, darah, alamat, agama, status, pekerjaan, kewarganegaraan);

                                return biodata;
                            } 
                        }
                    }
                }
            }

            return biodata;
        }

        private void reset_Display()
        {
            pictureBox1.ImageLocation = "../assets/cocok.PNG";
            label2.Text = "Persentase kecocokan : ";
            textBox1.Text = " List Biodata\r\n NIK: \r\n Nama:\r\n Tempat/Tgl Lahir:\r\n Jenis Kelamin:\r\n Gol Darah:\r\n" +
    " Alamat:\r\n Agama:\r\n Status:\r\n Pekerjaan:\r\n Kewarganegaraan:";
        }
    }
}
