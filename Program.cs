using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Web;

namespace Blahblah
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("MD5 Hash Generator");
            Console.WriteLine("This program creates MD5 hashes for all files in the folder.");
            Console.WriteLine("Work in progress...");

            //root = C:\Users\rasmu\source\repos\Blahblah\Blahblah\bin\Debug
            string root = Path.GetDirectoryName(args[0]);
            Console.WriteLine(args[0]);

            //hastlist = C:\Users\rasmu\source\repos\Blahblah\Blahblah\bin\Debug\haslist.txt
            string hashList = root + "/hashList.txt";
            
            //If file dosen't exists
            if (!File.Exists(hashList))
            {
                //Creates a file and close it
                var initHastListFile = File.Create(hashList);
                initHastListFile.Close();
            }
            
            //If the file does exists.
            else
            {
                //Deletes the file and create a new one and close it
                File.Delete(hashList);
                var hastListFile = File.Create(hashList);
                hastListFile.Close();
            }
            int i = 0;
            //Returns all files name and subdirectories
            //allLines = ["C:\\path\\filename.txt", "C:\\path\\filename2.txt"]
            string[] allFiles = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
            
            //Creates an empty list array
            //lines = [null, null, null]
            string[] lines = new string[allFiles.Count()];
            
            lines = DirSearch(root, lines, i, root);
            File.AppendAllLines(hashList, lines);
        }
        static string[] DirSearch(string dir, string[] lines, int counter, string root)
        {
            string hashListFileName = "hashList.txt";
            string[] lort = Directory.GetFiles(dir);

            foreach (string f in Directory.GetFiles(dir))
            {
                //Create an MD5 hash per file
                using (var md5 = MD5.Create())
                {
                    FileInfo info = new FileInfo(f);
                    string filename = info.FullName;
                    Console.WriteLine(filename);

                    //Do not make MD5 hash of "hastList.txt"
                    if (filename != hashListFileName)
                    {
                        using (var stream = File.OpenRead(filename))
                        {
                            byte[] fileMD5 = md5.ComputeHash(stream);
                            //string hash = HttpServerUtility.UrlTokenEncode(fileMD5);
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < fileMD5.Length; i++)
                            {
                                sb.Append(fileMD5[i].ToString("X2"));
                            }
                            string hash = sb.ToString().ToLower();
                            lines[counter] = info.FullName.Substring(root.Length + 1) + " " + hash;
                        }
                        counter++;
                    }
                }
            }

            foreach (string d in Directory.GetDirectories(dir))
            {
                DirSearch(d, lines, counter, root);
            }

            return lines;
        }
    }
}

