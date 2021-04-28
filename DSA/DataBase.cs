using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace DSA
{
    class DataBase
    {
        public static string FILE_HISTORY = "history";
        public static string FILE_FAVORITES = "favorites";



        public static string getData(string path)
        {
            string s = "";
            StreamReader f = new StreamReader(path);
            while (!f.EndOfStream)
            {
                s += f.ReadLine();
            }
            f.Close();
            return s;
        }

        public static void fillList(ObservableCollection<string> list, string path)
        {
            try
            {
                StreamReader f = new StreamReader(path);
                while (!f.EndOfStream)
                {
                    list.Add(f.ReadLine());
                }
                f.Close();
            }
            catch (Exception e2) { }
           
        }

        public static void saveList(ObservableCollection<string> list, string path)
        {
            StreamWriter f = new StreamWriter(path);
            for (int i = 0; i < list.Count; i++)
            {
                f.WriteLine(list[i]);
            }
            f.Close();
        }

        public static void writeInFile(string path, string str, FileMode mode)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(
            File.Open(path, mode)))
            {
                binaryWriter.Write(str);
            }
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }
    }
}
