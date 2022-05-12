using System;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateTestDocument();
            Task53();
        }


        static void CreateTestDocument()
        {
            XNamespace coversNS = "namespace_covers";
            XNamespace titlesNS = "namespace_titles";
            XDocument doc = new XDocument
            (
                new XDeclaration("1.0", "windows-1251", null), 
                new XElement("описания-книг",
                    new XElement(coversNS + "book-cover", 
                        new XAttribute("color", "123.22"), 
                        new XAttribute("material", "55.43")),
                    new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='hello.xsl'"),
                    new XElement(titlesNS + "title-info",
                        new XAttribute("title", "1"),
                        new XElement("genre", new XAttribute("match", "90"), "sf_fantasy"),
                        new XElement("author",
                            new XElement("first-name",
                            new XAttribute("date-of-birth", "1892"), "Джон"),
                            new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='hello.xsl'"),
                            new XElement("middle-name", "Рональд Руэл"),
                            new XElement("last-name", "Толкин")
                        ),
                        new XElement("book-title", "Возвращение Короля"),
                        new XElement("lang", "ru"),
                        new XElement("sequence",
                            new XAttribute("name", "-522.4"),
                            new XAttribute("number", "-58.3")
                        )
                    )
                )
            );
            doc.Save("TestDocument.xml");
            Console.WriteLine(doc);
        }


        /// <summary>
        /// LinqXml3.
        /// Даны имена существующего текстового файла и создаваемого XML-документа. 
        /// Создать XML-документ с корневым элементом root и элементами первого уровня line, 
        /// каждый из которых содержит одну строку из исходного файла. 
        /// Элемент, содержащий строку с порядковым номером N (1, 2, …), 
        /// должен иметь атрибут num со значением, равным N.
        /// </summary>
        /// <param nameTextFile="fileTask3">Имя существующего текстового файла</param>
        /// <param nameXMLDocument="XMLDocumentTask3">Имя создаваемого XML-документа</param>
        static void Task03(string nameTextFile= "fileTask3.txt", string nameXMLDocument = "XMLDocumentTask3")
        {
            List<string> lines = new List<string>();
            string line;
            int N = 1;

            try
            {
                StreamReader sr = new StreamReader(nameTextFile);
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                sr.Close();

                XDocument d = new XDocument
                (
                    new XDeclaration("1.0", "windows-1251", null),
                    new XElement("root", lines.Select(n =>
                        new XElement("line", new XAttribute("num", N++), n)))
                );

                d.Save(nameXMLDocument);
                Console.WriteLine(d);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml13.
        /// Дан XML-документ, содержащий хотя бы один атрибут. 
        /// Вывести все различные имена атрибутов, входящих в документ. 
        /// Порядок имен атрибутов должен соответствовать порядку их первого вхождения в документ.
        /// Указание.Использовать методы SelectMany и Distinct.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        static void Task13(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                var attributes = doc.Root.Descendants().SelectMany<XElement, XAttribute>(el => el.Attributes()).Select<XAttribute, XName>(attr => attr.Name).Distinct();
                foreach (var attrName in attributes)
                {
                    Console.WriteLine(attrName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml23.
        /// Дан XML-документ. Удалить из документа все инструкции обработки.
        /// Указание. Для получения последовательности всех инструкций обработки
        /// воспользоваться методом OfType<XProcessingInstruction>.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        static void Task23(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                doc.Descendants().SelectMany<XElement, XNode>(el => el.Nodes()).OfType<XProcessingInstruction>().ToList().ForEach(x => x.Remove());
                Console.WriteLine(doc);

                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml33.
        /// Дан XML-документ. Для каждого элемента первого уровня, имеющего атрибуты, 
        /// добавить в конец его дочерних узлов элемент с именем attr и атрибутами, 
        /// совпадающими с атрибутами обрабатываемого элемента первого уровня, 
        /// после чего удалить все атрибуты у обрабатываемого элемента. 
        /// Добавленный элемент attr должен быть представлен в виде комбинированного тега.
        /// Указание.Использовать метод ReplaceAttributes, указав в качестве параметра новый дочерний элемент.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        static void Task33(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                doc.Root.Nodes().Where(node => node is XElement)
                                .Where(el => ((XElement)el).Attributes().Count() > 0)
                                .ToList()
                                .ForEach(x => ((XElement)x)
                                .ReplaceAttributes(new XElement("attr", ((XElement)x).Attributes())));

                Console.WriteLine(doc);
                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml43.
        /// Дан XML-документ, в котором значения всех атрибутов являются текстовыми представлениями вещественных чисел. 
        /// Добавить к каждому элементу первого уровня, содержащему дочерние элементы, дочерний элемент max, 
        /// содержащий текстовое представление максимального из значений атрибутов всех элементов-потомков данного элемента. 
        /// Если ни один из элементов-потомков не содержит атрибутов, то элемент max не добавлять.
        /// Указание.
        /// Для единообразной обработки двух ситуаций (наличие или отсутствие атрибутов у потомков) 
        /// можно построить по последовательности атрибутов последовательность числовых значений Nullable - типа double?, 
        /// применить к ней метод Max и добавить новый элемент max с помощью метода SetElementValue, 
        /// указав в качестве второго параметра результат, возвращенный методом Max.
        /// При отсутствии атрибутов у потомков метод Max вернет значение null; 
        /// в этом случае метод SetElementValue не будет создавать новый элемент.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        static void Task43(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                doc.Root.Nodes().Where(node => node is XElement)
                                .ToList()
                                .ForEach(x => ((XElement)x)
                                .SetElementValue("max", ((XElement)x).Attributes().Select(attr => Convert.ToDouble(attr.Value)).Max()));

                Console.WriteLine(doc);
                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml53.
        /// Дан XML-документ. В каждом элементе первого уровня определено пространство имен, 
        /// распространяющееся на все его элементы-потомки. 
        /// Для каждого элемента первого уровня добавить в конец его набора дочерних узлов элемент 
        /// с именем namespace и значением, равным пространству имен обрабатываемого элемента первого уровня 
        /// (пространство имен добавленного элемента должно совпадать с пространством имен его родительского элемента).
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        static void Task53(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                //doc.Root.Nodes().Where(node => node is XElement).ToList().ForEach(x => ((XElement)x).Add(new XElement("namespace", x.));

                Console.WriteLine(doc);
                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /*
        static void Task23(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                void CounterNode(XElement node) => node.Nodes().OfType<XProcessingInstruction>().ToList().ForEach(x => x.Remove());

                PerformOperation Counter;
                Counter = CounterNode;
                ApplyToAllNodes(doc.Root, Counter);
                Console.WriteLine(doc);

                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        delegate void PerformOperation(XElement node);
        static void ApplyToAllNodes(XElement element, PerformOperation func)
        {
            if (element == null) { }
            else
            {
                func(element);
                foreach (var node in element.Nodes()) 
                { 
                    if (node is XElement)
                    {
                        ApplyToAllNodes((XElement)node, func); 
                    }
                }
            } 
        }
        */
    }
}
