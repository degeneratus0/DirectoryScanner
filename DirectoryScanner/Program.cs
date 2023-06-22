using System;

namespace DirectoryScanner
{
    internal sealed class Program
    {
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static void Main(string[] args)
        {
            DirectoryNode baseNode = new DirectoryNode(BaseDirectory);
            baseNode.ScanDirectory();

            HtmlGenerator.GenerateDirectoryReport(baseNode, BaseDirectory);

            Console.WriteLine("Нажмите Q чтобы вывести список директорий и файлов, или нажмите любую другую кнопку чтобы выйти.");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                Console.Clear();
                baseNode.PrintDirectory();
                Console.WriteLine("Нажмите любую кнопку чтобы выйти");
                Console.ReadKey();
            }
        }
    }
}