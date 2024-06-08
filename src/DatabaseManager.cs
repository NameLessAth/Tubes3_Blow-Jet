using System.Drawing;
using System.Data.SQLite;


namespace src{
    class DatabaseManager{
        public static void Main(string[] args){
            Console.Write(ConverterImage.ImageToBinaryString("../test/SOCOFing/Altered/Altered-Easy/5__M_Right_index_finger_CR.BMP"));
        }
        public static List<(String, String)> GetSidikJari(){
            // try{
            //     Tes.ConvertSqlToDb("db/coba.sql", "db/coba.db");
            // } catch (Exception e) {Console.WriteLine(e.Message);}
            List<(String, String)> temp = []; 
            using (var connection = new SQLiteConnection("Data Source=db/test.db;Version=3;"))
            {
                connection.Open();  
                
                // Define the SQL query
                string query = "SELECT * FROM sidik_jari";

                // Create a command
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and get a data reader
                    using (var reader = command.ExecuteReader())
                    {
                        // Read the data
                        while (reader.Read())
                        {
                        // Example of reading columns by name
                        string berkas = reader.GetString(reader.GetOrdinal("berkas_citra"));
                        string name = reader.GetString(reader.GetOrdinal("nama"));
                        Console.WriteLine(berkas + " " + name);
                        temp.Add((berkas, name));
                        } 
                    }
                }   
            } return temp;
        }
        static void ConvertSqlToDb(string sqlFilePath, string dbFilePath)
        {
            // Create and open the SQLite database
            using (var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
            {
                connection.Open();

                // Read the SQL script from the file
                string sqlScript = File.ReadAllText(sqlFilePath);

                // Split the script into individual commands
                string[] commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Execute each command
                foreach (var commandText in commands)
                {
                    if (!string.IsNullOrWhiteSpace(commandText))
                    {
                        using (var command = new SQLiteCommand(commandText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        static void RemoveSidikJari(SQLiteConnection connection, string berkas, string nama){
            string insertQuery = @"
                DELETE FROM sidik_jari WHERE berkas_citra=@berkas_citra AND nama=@nama;";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@berkas_citra", berkas);
                command.Parameters.AddWithValue("@nama", nama);
                command.ExecuteNonQuery();
            }
        }
        static void InsertSidikJari(SQLiteConnection connection, string berkas, string nama){
            string insertQuery = @"
                INSERT INTO sidik_jari VALUES (@berkas_citra, @nama);";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@berkas_citra", berkas);
                command.Parameters.AddWithValue("@nama", nama);
                command.ExecuteNonQuery();
            }
        }
        static void InsertBiodata(SQLiteConnection connection, string nik, string nama, string tempat_lahir, string tanggal_lahir, string jenis_kelamin, string golongan_darah, string alamat, string agama, string status_perkawinan, string pekerjaan, string kewarganegaraan)
        {
            string insertQuery = @"
                INSERT INTO biodata (NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan) 
                VALUES (@NIK, @nama, @tempat_lahir, @tanggal_lahir, @jenis_kelamin, @golongan_darah, @alamat, @agama, @status_perkawinan, @pekerjaan, @kewarganegaraan);";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@NIK", nik);
                command.Parameters.AddWithValue("@nama", nama);
                command.Parameters.AddWithValue("@tempat_lahir", tempat_lahir);
                command.Parameters.AddWithValue("@tanggal_lahir", tanggal_lahir);
                command.Parameters.AddWithValue("@jenis_kelamin", jenis_kelamin);
                command.Parameters.AddWithValue("@golongan_darah", golongan_darah);
                command.Parameters.AddWithValue("@alamat", alamat);
                command.Parameters.AddWithValue("@agama", agama);
                command.Parameters.AddWithValue("@status_perkawinan", status_perkawinan);
                command.Parameters.AddWithValue("@pekerjaan", pekerjaan);
                command.Parameters.AddWithValue("@kewarganegaraan", kewarganegaraan);

                command.ExecuteNonQuery();
            }
        }
    }
}