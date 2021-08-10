using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
     /// 5 имя элемента к копированию
     /// 6 путь к "главному элементу" в шаблоне
     /// 7 true/false - добавлять ли @"<?xml version=""1.0"" encoding=""windows-1251""?>";
     /// </param>
        static void Main(string[] args)
        {
            
            if(args.Length<8)
            {
                Console.WriteLine("Pls. read read.me!!!");
                Environment.Exit(-1);
            }

            var xDocTemplate = XDocument.Load(args[1]);
            var doc = new XDocument(xDocTemplate);

            int i = 0;
            int n = 0;
            int counter = 0;
            
            using (XmlReader reader = XmlReader.Create(new StreamReader(args[0], Encoding.GetEncoding(1251))))
            {
                XElement name = null;
                XElement item = null;

                reader.MoveToContent();

                // Parse the file, save header information when encountered, and yield the
                // Item XElement objects as they are created.

                // loop through Customer elements
                while (!reader.EOF)
                {

                    if (reader.NodeType == XmlNodeType.Element
                        && reader.Name == args[5])
                    {
                        counter++;
                        item = XElement.ReadFrom(reader) as XElement;

                        if (counter % 1000 == 0) Console.WriteLine(counter.ToString() + " of ХЗ");
                        doc.XPathSelectElement(args[6]).Add(item);
                        i++;
                        if (i == int.Parse(args[2]))
                        {
                                n++; i = 0;
                                doc.Save(Path.Combine(args[3], args[4] + "_" + string.Format("{0:D5}", n) + ".xml"));
                                doc = new XDocument(xDocTemplate);
                        
                        }
                        

                    }
                    else reader.Read();
                }


            }

            Console.WriteLine("AllCount " + counter.ToString());


        }
    }
}
