using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace xmldiv
{
    class Program
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="args">
     /// args[0] - путь к файл для разбивки
     /// 1 - путь к шаблону
     /// 2 - сообщений в файл
     /// 3 директория для сохранения
     /// 4 префикс сохранения
     /// 5 путь к "главному элементу" в исходнике
     /// 6 путь к "главному элементу" в шаблоне
     /// 7 true/false - добавлять ли @"<?xml version=""1.0"" encoding=""windows-1251""?>";
     /// </param>
        static void Main(string[] args)
        {
            
            var head= @"<?xml version=""1.0"" encoding=""windows-1251""?>";
            XDocument source;
            Console.WriteLine("Read file "+args[0]);
            using (MemoryStream stream=new MemoryStream())
            {
                using (StreamWriter sw=new StreamWriter(stream, Encoding.GetEncoding(1251)))
                {
                    if(bool.Parse(args[7])) sw.WriteLine(head);

                    using (StreamReader sr = new StreamReader(args[0], Encoding.GetEncoding(1251)))
                    {
                        /*
                        while (line )
                        {
                            //var s = sr.ReadLine();
                            //if(s.Trim()!="")  
                                sw.WriteLine(sr.ReadLine());    
                        
                        }
                        */
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if(line.Trim()!="")
                                sw.WriteLine(line);
                        }

                        
                        
                        sw.Flush();
                        stream.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        source = XDocument.Load(stream, LoadOptions.PreserveWhitespace);
                        //sw.Close();

                        /*
                        FileStream file = new FileStream("D:\\XML\\TEMP.XML", FileMode.Create, FileAccess.Write);
                        stream.WriteTo(file);
                        file.Close();
                        */

                    }
                }
            }
            
            //var xDocSource = XDocument.Load("D:\\XML\\TEMP.xml");

            var xDocTemplate = XDocument.Load(args[1]);
            var doc = new XDocument(xDocTemplate);
            int i = 0;
            int n = 0;
            int j = 0;
            var els = source.XPathSelectElement(args[5]).Elements();
            var ecount = els.Count();
            foreach (var el in els)
            {
                j++;
                Console.WriteLine(string.Format("Element {0} of {1}",j, ecount));
                doc.XPathSelectElement(args[6]).Add(el);
                i++;
                if(i==int.Parse(args[2]))
                {
                    n++;i = 0;
                    doc.Save(Path.Combine(args[3], args[4] + "_" + string.Format("{0:D5}",n) + ".xml"));
                    doc = new XDocument(xDocTemplate);
                }    
            };






        }
    }
}
