# Fingerprint Identifier with Pattern Matching
Tugas Besar 3 IF2211 Strategi Algoritma - String Matching dan Regex

<br/>
<p align="center">
  <img height="360px" src="https://i.ibb.co.com/ChL3Hgq/blowjet.png" alt="preview app"/>
  <br>
  <a><i><sup>Preview Aplikasi kelompok "Blow Jet"</sup></i></a>
</p>
<br/>

## Anggota 
1. Ariel Herfrison (13522002)
2. Eduardus Alvito Kristiadi (13522004)
3. M Athaullah Daffa Kusuma M (13522044)

## Daftar Isi
* [Deskripsi Program](#deskripsi-singkat)
* [Dependencies](#informasi-tambahan)
* [Features](#fitur-utama)
* [Setup](#setup)
* [Cara Penggunaan](#cara-menjalankan-program)

## Deskripsi Singkat
Program ini merupakan tugas besar 3 dari mata kuliah IF2211 Strategi Algoritma. Program ini berfungsi untuk Mencari identitas dari pemilik sidik jari yang di-input oleh pengguna. 

## Informasi Tambahan
- Program dibuat dengan : dotnet 8.0.206
- Kakas GUI yang digunakan : Windows Form
- IDE yang digunakan : Visual Studio dan Visual Studio Code
- Laporan dibuat dengan : Google Docs 

## Fitur Utama
- Pencocokan menggunakan Boyer–Moore String-search Algorithm
- Pencocokan menggunakan Knuth–Morris–Pratt String-search Algorithm

## Setup
1. Program menggunakan relative pathing dan asumsi path pada berkas_citra di database berupa `../test/SOCOFing/`. Lokasi eksekusi program dari folder `src`, Silahkan pindahkan folder `src/db` dan `src.exe` di dalam foldern `src/bin/Release` untuk mengganti path jika terjadi perbedaan path pada berkas_citra.
2. Program menggunakan Database berupa `test.db`. Jika ingin mengganti database, silahkan replace file tersebut dengan nama yang sama.

## Petunjuk Cara Menjalankan Program dan lainnya

### Cara Menjalankan Program
berikut merupakan langkah untuk menjalankan program:
1. Pastikan lokasi folder `src/db` dan `src.exe` tepat sesuai dengan [Setup](#setup)
2. Jalankan Program dengan menjalankan `src.exe` di dalam folder `src/bin/Release` (atau di folder lain jika lokasi program diubah)


### Input
Pengguna nantinya akan diminta untuk memasukkan gambar berupa input, lalu pengguna memilih algoritma String Matching yang digunakan dengan menekan tombol B/KM di bawah (B berarti Boyer-Moore, KM berarti Knuth-Morris-Pratt). 

### Output
Nantinya program akan mengoutput berupa data diri pemilik sidik jari dengan waktu eksekusi dan persentase kecocokan, atau program akan mengoutput error message berupa "Sidik jari tidak ditemukan".
