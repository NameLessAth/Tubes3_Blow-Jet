using System.Data.SqlClient;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace src{
    public class BloggingContext : DbContext{
        public DbSet<Biodata> Biodatas {get; set;}
        public DbSet<SidikJari> SidikJaris {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder
            .UseSqlServer(@"Server=localhost:8080\mssqllocaldb;Database=stima;Trusted_Connection=True;")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
    public class SidikJari{
        public String berkas_citra {get; set;}
        public String nama {get; set;}  
    }
    public class Biodata{
        public String NIK {get; set;}
        public String nama {get; set;}
        public String tempat_lahir {get; set;}
        public String tanggal_lahir {get; set;}
        public String jenis_kelamin {get; set;}
        public String golongan_darah {get; set;}
        public String alamat {get; set;}
        public String agama {get; set;}
        public String status_perkawinan {get; set;}
        public String pekerjaan {get; set;}
        public String kewarganegaraan {get; set;}

    }
    
}