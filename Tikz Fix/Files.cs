using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tikz_Fix
{
    class Files
    {
        public static string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"c:\";
            openFileDialog.Filter = "Pliki tekstowe(*.txt) | *.txt";
            string path;
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                path = openFileDialog.FileName.ToString();
                try
                {
                    string readText = File.ReadAllText(path);
                    return readText;
                }
                catch
                {
                    MessageBox.Show("Wybrano zły plik!", "Uwaga");
                }
            }
            return null;
        }

        public static void WriteToFile(BindingList<TikzCode> tikzCode)
        {
            try
            {
                StreamWriter sw = new StreamWriter("TikzCode.txt");
                sw.WriteLine(StringOperations.GenerateOutput(tikzCode));
                sw.Close();

                MessageBox.Show("Pomyślnie zapisano");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

    }
}
