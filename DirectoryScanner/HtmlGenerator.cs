using RazorEngine.Templating;
using RazorEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DirectoryScanner
{
    public static class HtmlGenerator
    {
        private static StringBuilder directoryTraversalStringBuilder = new StringBuilder();
        private const string ReportPath = "Directory Report";

        public static void GenerateDirectoryReport(DirectoryNode node, string path)
        {
            directoryTraversalStringBuilder.AppendLine($"<h1 class=\"display-6\">Корневая директория: {node.Name}, общий размер - {node.Size} байт</h1>");
            TraverseDirectoryHtml(node);
            string result = Engine.Razor.RunCompile(File.ReadAllText("Templates/Report.cshtml"), "Report", typeof(List<Statistics>), Statistics.GetStats(node));
            Directory.CreateDirectory(ReportPath);
            File.WriteAllText(path + ReportPath + "/report.html", result);
            Console.WriteLine("Отчет успешно сгенерирован и сохранен в папке " + ReportPath);
        }

        public static string GetDirectoryTraversalString()
        {
            return directoryTraversalStringBuilder.ToString();
        }

        private static void TraverseDirectoryHtml(DirectoryNode node)
        {
            foreach (DirectoryNode item in node.Children)
            {
                if (!item.IsDirectory)
                {
                    directoryTraversalStringBuilder.AppendLine($"<span class=\"list-group-item\">");
                    directoryTraversalStringBuilder.AppendLine($"{item.Name} - {item.Size} bytes - {item.MimeType}");
                }
                else
                {
                    directoryTraversalStringBuilder.AppendLine($"<a href=\"#item-{item.GetHashCode()}\" class=\"list-group-item\" data-bs-toggle=\"collapse\">");
                    directoryTraversalStringBuilder.AppendLine($"<i class=\"bi bi-chevron-right\"></i> {item.Name} - {item.Size} байт");
                }
                directoryTraversalStringBuilder.AppendLine(item.IsDirectory ? "</a>" : "</span>");
                if (item.Children.Any())
                {
                    directoryTraversalStringBuilder.AppendLine($"<div class=\"list-group collapse\" id=\"item-{item.GetHashCode()}\">");
                    TraverseDirectoryHtml(item);
                    directoryTraversalStringBuilder.AppendLine("</div>");
                }
            }
        }
    }
}
