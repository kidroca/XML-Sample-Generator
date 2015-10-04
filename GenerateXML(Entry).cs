namespace Microsoft.Xml.XMLGen
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    class GenerateXML
    {
        private const int DefaultThreshold = 10;

        private const int DefaultListLength = 5;

        private static int maxTreshold;

        private static int listLength;

        private static string schemaPath;

        private static string savePath;

        private static string rootElementName;

        /// <summary>
        /// Generates Sample XML from XSD schema, when started from the same folder as the xsd document
        /// you can use no parameters for the default configuration
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When no xsd file is found in the given path or in the root folder this exception is thrown
        /// </exception>
        /// <param name="args[0]">ElementName - the name of the element to generate</param>
        /// <param name="args[1]">MaxTreshold - how many elements to create</param>
        /// <param name="args[2]">
        /// ListLength - defines how many items should be generated for a simple type of variety
        /// list. The default value of this property is 5
        /// </param>
        /// <param name="args[3]">XSD file - file to open</param>
        /// <param name="args[4]">Folder to save to</param>
        /// of variety list</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("XmlGen.exe element, maxCount, innerListsLength, xsdSchemaPath, folderName");
            Console.WriteLine();
            Console.WriteLine("element - expected root element(a complexType) for the generated xml");

            Console.WriteLine("maxCount - number of items to be generated for elements that have a");
            Console.WriteLine("maxOccurs attribute set to a value default value 10");

            Console.WriteLine("innerListLength - defines how many items should be generated for a simple");
            Console.WriteLine("type of variety list. The default value of this property is 5");

            Console.WriteLine("xsdSchemaPath - path to the xsd schema to use as template");

            Console.WriteLine("folderName - folder to save the generated xml");
            Console.WriteLine();
            Console.WriteLine("This program can run without input arguments as long as it is in the same");
            Console.WriteLine("folder as the xsd schema");
            Console.WriteLine();

            OrganizeInputArgs(args);
               
            var root = new XmlQualifiedName(rootElementName);

            var generator = new XmlSampleGenerator(schemaPath, root);
            generator.MaxThreshold = maxTreshold;
            generator.ListLength = listLength;

            using (var xmlWriter = new XmlTextWriter("Sample.xml", Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;

                generator.WriteXml(xmlWriter);
            }

            Console.WriteLine("Success\n");
        }

        private static void OrganizeInputArgs(string[] args)
        {
            if (args.Length < 5)
            {
                args = ReplaceArgsWithSuccessor(args, 5);
            }

            rootElementName = args[0];

            if (!int.TryParse(args[1], out maxTreshold))
            {
                maxTreshold = DefaultThreshold;
            }

            if (!int.TryParse(args[2], out listLength))
            {
                listLength = DefaultListLength;
            }

            schemaPath = args[3];
            if (schemaPath == null)
            {
                schemaPath = Directory.GetFiles(
                    Environment.CurrentDirectory
                        , "*.xsd"
                        , SearchOption.TopDirectoryOnly)
                    .First();
            }

            savePath = args[4];
            if (savePath == null)
            {
                savePath = Environment.CurrentDirectory;
            }
        }

        private static string[] ReplaceArgsWithSuccessor(string[] args, int correctLength)
        {
            string[] successor = new string[correctLength];

            for (int i = 0; i < args.Length; i++)
            {
                successor[i] = args[i];
            }

            return successor;
        }
    }
}
