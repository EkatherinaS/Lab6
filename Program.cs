using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Queries
{
    public class Program
    {
        static void Main(string[] args)
        {

        }


        public static void CreateTestDocument03_53()
        {
            XNamespace coversNS = "namespace_covers";
            XNamespace titlesNS = "namespace_titles";
            XDocument doc = new XDocument
            (
                new XDeclaration("1.0", "windows-1251", null), 
                new XElement("описания-книг",
                    new XElement(coversNS + "book-cover",
                        new XAttribute(XNamespace.Xmlns + "cn", coversNS),
                        new XAttribute("color", "123.22"), 
                        new XAttribute("material", "55.43")),
                    new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='hello.xsl'"),
                    new XElement(titlesNS + "title-info",
                        new XAttribute(XNamespace.Xmlns + "tn", titlesNS),
                        new XAttribute("title", "1"),
                        new XElement("genre", new XAttribute("match", "90"), "sf_fantasy"),
                        new XElement(titlesNS + "author",
                            new XElement(titlesNS + "first-name",
                            new XAttribute("date-of-birth", "1892"), "Джон"),
                            new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='hello.xsl'"),
                            new XElement(titlesNS + "middle-name", "Рональд Руэл"),
                            new XElement(titlesNS + "last-name", "Толкин")
                        ),
                        new XElement(titlesNS + "book-title", "Возвращение Короля"),
                        new XElement(titlesNS + "lang", "ru"),
                        new XElement(titlesNS + "sequence",
                            new XAttribute("name", "-522.4"),
                            new XAttribute("number", "-58.3")
                        )
                    )
                )
            );
            doc.Save("TestDocument.xml");
            Console.WriteLine(doc);
        }


        public static void CreateTestDocument63()
        {
            XDocument doc = new XDocument
            (
                new XDeclaration("1.0", "windows-1251", null),
                new XElement("document",
                    new XElement("record",
                        new XAttribute("date", "2000-05-01T00:00:00"),
                        new XAttribute("id", "10"),
                        new XAttribute("time", "PT5H13M")
                    ),
                    new XElement("record",
                        new XAttribute("date", "2000-06-01T00:00:00"),
                        new XAttribute("id", "10"),
                        new XAttribute("time", "PT5H23M")
                    ),
                    new XElement("record",
                        new XAttribute("date", "2001-03-01T00:00:00"),
                        new XAttribute("id", "10"),
                        new XAttribute("time", "PT5H24M")
                    ),
                    new XElement("record",
                        new XAttribute("date", "2001-03-01T00:00:00"),
                        new XAttribute("id", "11"),
                        new XAttribute("time", "PT5H24M")
                    )
                )
            );
            doc.Save("TestDocument.xml");
            Console.WriteLine(doc);
        }


        public static void CreateTestDocument73()
        {

            XDocument doc = new XDocument
            (
                new XDeclaration("1.0", "windows-1251", null),
                new XElement("streets",
                    new XElement("ул.Чехова",
                        new XElement("company",
                            new XAttribute("name", "Лидер"),
                                new XElement("brand", "92"),
                                new XElement("price", "2200"))),
                    new XElement("ул.Чехова",
                        new XElement("company",
                            new XAttribute("name", "Лидер"),
                                new XElement("brand", "91"),
                                new XElement("price", "2200"))),
                    new XElement("ул.Чехова",
                        new XElement("company",
                            new XAttribute("name", "Лидер"),
                                new XElement("brand", "94"),
                                new XElement("price", "2200"))),
                    new XElement("ул.Чехова",
                        new XElement("company",
                            new XAttribute("name", "Mai"),
                                new XElement("brand", "92"),
                                new XElement("price", "2210"))),
                    new XElement("ул.A",
                        new XElement("company",
                            new XAttribute("name", "Лидер"),
                                new XElement("brand", "92"),
                                new XElement("price", "2200"))),
                    new XElement("ул.A",
                        new XElement("company",
                            new XAttribute("name", "Лидер"),
                                new XElement("brand", "95"),
                                new XElement("price", "2200")))
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
        public static void Task03(string nameTextFile= "fileTask3.txt", string nameXMLDocument = "XMLDocumentTask3")
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
        public static void Task13(string nameXMLDocument = "TestDocument.xml")
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
        public static void Task23(string nameXMLDocument = "TestDocument.xml")
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
        public static void Task33(string nameXMLDocument = "TestDocument.xml")
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
        public static void Task43(string nameXMLDocument = "TestDocument.xml")
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
        public static void Task53(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);
                XNamespace prefix;

                doc.Root.Nodes()
                        .Where(node => node is XElement)
                        .ToList()
                        .ForEach(x => ((XElement)x).Add(new XElement((prefix = ((XElement)x).CreateNavigator().NamespaceURI) + "namespace", ((XElement)x).CreateNavigator().NamespaceURI)));

                Console.WriteLine(doc);
                doc.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml63.
        /// Дан XML-документ с информацией о клиентах фитнес-центра. Образец элемента первого уровня (смысл данных тот же, что и в LinqXml61):
        /// <record date = "2000-05-01T00:00:00" id="10" time="PT5H13M" />
        /// Преобразовать документ, выполнив группировку данных по кодам клиентов и изменив элементы первого уровня следующим образом:
        /// <client id = "10" > < time year="2000" month="5">PT5H13M</time>... </client>
        /// Элементы первого уровня должны быть отсортированы по возрастанию кода клиента, их дочерние элементы — по возрастанию номера года, 
        /// а для одинаковых значений года — по возрастанию номера месяца.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        public static void Task63(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                var newData =
                    new XElement("record",
                        from data in doc.Root.Elements()
                        group data by data.Attribute("id").Value into groupedData
                        orderby groupedData.Key
                        select new XElement("Client",
                            new XAttribute("Id", groupedData.Key),
                            from g in groupedData
                            orderby Convert.ToDateTime(g.Attribute("date").Value).Year, Convert.ToDateTime(g.Attribute("date").Value).Month
                            select new XElement("time",
                                new XAttribute("year", Convert.ToDateTime(g.Attribute("date").Value).Year),
                                new XAttribute("month", Convert.ToDateTime(g.Attribute("date").Value).Month),
                                g.Attribute("time").Value)
                        )
                    );
                Console.WriteLine(newData);
                newData.Save(nameXMLDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }


        /// <summary>
        /// LinqXml73.
        /// Дан XML-документ с информацией о ценах авто-заправочных станций на бензин. 
        /// Образец элемента первого уровня (смысл данных тот же, что и в LinqXml68, данные сгруппированы по названиям улиц; 
        /// названия улиц указываются в качестве имен элементов первого уровня):
        /// <ул.Чехова> <company name = "Лидер" > < brand > 92 </ brand > < price > 2200 </ price > </ company > ... </ул.Чехова>
        /// 
        /// Преобразовать документ, сгруппировав данные по названиям компаний и названиям улиц и оставив сведения только о тех АЗС, 
        /// в которых предлагаются не менее двух марок бензина.
        /// 
        /// Изменить элементы первого уровня следующим образом:
        /// <Лидер_ул.Чехова brand-count="2"> <b92 price = "2200" /> < b95 price="2450" /> </Лидер_ул.Чехова>
        /// Имя элемента первого уровня содержит название компании, после которого следует символ подчеркивания и название улицы; 
        /// имя элемента второго уровня должно иметь префикс b, после которого указывается марка бензина.
        /// Атрибут brand-count должен содержать количество марок бензина, предлагаемых на данной АЗС. 
        /// 
        /// Элементы первого уровня должны быть отсортированы в алфавитном порядке названий компаний, 
        /// а для одинаковых названий компаний — в алфавитном порядке названий улиц; 
        /// их дочерние элементы должны быть отсортированы по возрастанию марок бензина.
        /// </summary>
        /// <param nameXMLDocument="TestDocument.xml">Имя существующего XML-документа</param>
        public static void Task73(string nameXMLDocument = "TestDocument.xml")
        {
            XDocument doc = XDocument.Load(nameXMLDocument);

            var newData =
                new XElement("record",
                    from street in doc.Root.Elements()
                    from company in street.Elements()
                    select new XElement(company.Attribute("name").Value + "-" + street.Name,
                        from b in company.Elements().Where(g => g.Name == "brand")
                        select new XElement("b" + b.Value,
                        from p in company.Elements().Where(g => g.Name == "price")
                        select new XAttribute("price", p.Value))));

            newData =
                new XElement("record",
                    from data in newData.Elements()
                    from brand in data.Elements()
                    group brand by data.Name into groupedData
                    orderby groupedData.Key.ToString()
                    where groupedData.Count() > 1
                    select new XElement(groupedData.Key,
                           new XAttribute("brand-count", groupedData.Count()),
                           groupedData));

            foreach (var trans in newData.Elements())
            {
                trans.ReplaceAll(trans.Elements().OrderBy(x => x.Name.LocalName));
            }

            Console.WriteLine(newData);
            newData.Save(nameXMLDocument);
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

        
        static void PrintNamespace(string nameXMLDocument = "TestDocument.xml")
        {
            try
            {
                XDocument doc = XDocument.Load(nameXMLDocument);

                void CounterNode(XElement node) => node.Nodes()
                        .Where(node => node is XElement)
                        .ToList()
                        .ForEach(x => Console.WriteLine(((XElement)x).CreateNavigator().NamespaceURI));

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
