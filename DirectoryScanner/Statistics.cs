using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectoryScanner
{
    public class Statistics
    {
        public string MimeType { get => mimeType; }
        public int Frequency { get => frequency; }
        public double FrequencyPercent { get => frequencyPercent; }
        public long AverageSize { get => averageSize; }

        private static List<Statistics> stats = new List<Statistics>();

        private string mimeType;
        private int frequency;
        private double frequencyPercent;
        private long averageSize;

        public static List<Statistics> GetStats(DirectoryNode node)
        {
            stats = new List<Statistics>();
            GetStatsTraverseDirectory(node);
            foreach (Statistics stat in stats)
            {
                stat.averageSize /= stat.Frequency;
                stat.frequencyPercent = Math.Round((double)stat.Frequency / stats.Sum(x => x.Frequency) * 100, 2);
            }
            return stats;
        }

        private static void GetStatsTraverseDirectory(DirectoryNode node)
        {
            if (!node.IsDirectory)
            {
                if (stats.FirstOrDefault(x => x.MimeType == node.MimeType) == null)
                {
                    stats.Add(new Statistics() { mimeType = node.MimeType });
                }
                Statistics statistics = stats.Single(x => x.MimeType == node.MimeType);
                statistics.averageSize += node.Size;
                statistics.frequency++;
            }
            foreach (DirectoryNode child in node.Children)
            {
                GetStatsTraverseDirectory(child);
            }
        }
    }
}
