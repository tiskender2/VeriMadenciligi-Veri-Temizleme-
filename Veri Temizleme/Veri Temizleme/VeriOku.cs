using Bytescout.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Veri_Temizleme
{
    public class VeriOku
    {
        List<string> ilListesi;
        string dosyaYolu;
        public VeriOku(string dosyaYolu)
        {
            this.dosyaYolu = dosyaYolu;
            ilListesi = new List<string>();
        }

        public List<string[]> ilVerileri(int sheetIndis, int sheetStart, int sheetEnd)
        {
            List<string[]> satirlar = new List<string[]>();
            Spreadsheet document = new Spreadsheet();
            document.LoadFromFile(dosyaYolu);
            Worksheet worksheet = document.Workbook.Worksheets[sheetIndis];

            int colm = worksheet.UsedRangeColumnMax;
           // MessageBox.Show(colm+"");

            for (int j = sheetStart; j <= sheetEnd; j++)
            {
                string[] sutunlar = new string[colm ];
                for (int k = 1; k <= colm; k++)
                {
                    Cell currentCell = worksheet.Cell(j, k);
                    sutunlar[(k - 1)] += currentCell.ValueAsString;
                }
                satirlar.Add(sutunlar);

            }
            return satirlar;

        }

        public List<string> iller()
        {
            Spreadsheet document = new Spreadsheet();
            document.LoadFromFile(dosyaYolu);
            for (int i = 0; i < document.Worksheets.Count; i++)
            {
                Worksheet worksheet = document.Workbook.Worksheets[i];

                ilListesi.Add(worksheet.Name);

            }
            return ilListesi;
        }
    }
}
