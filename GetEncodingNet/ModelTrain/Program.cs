using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ModelTrain
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Dictionary<int, Dictionary<int, Dictionary<Encoding, int>>>();
            var stats = new Dictionary<int, Dictionary<Encoding, int>>();

            var encodings = new Encoding[]
            {
                Encoding.UTF8, Encoding.UTF32, Encoding.GetEncoding(1251), Encoding.GetEncoding(866),
                Encoding.GetEncoding(20866)
            };

            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine("processing {0}...", dir);
            try
            {
                var files = Directory.GetFiles(dir, "*.txt", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    Console.WriteLine("processing file {0}...", file);
                    var text = File.ReadAllText(file, Encoding.GetEncoding(1251));

                    foreach (var encoding in encodings)
                    {
                        var ms = new MemoryStream(encoding.GetBytes(text));
                        
                        var currentEncoding = encoding;
                        Action<int, int> ngProc = (size, value) =>
                        {
                            if (!model.ContainsKey(size)) model[size] = new Dictionary<int, Dictionary<Encoding, int>>();
                            if (!model[size].ContainsKey(value)) model[size][value] = new Dictionary<Encoding, int>();
                            if (!model[size][value].ContainsKey(currentEncoding)) model[size][value][currentEncoding] = 0;
                            model[size][value][currentEncoding]++;
                            //
                            if (!stats.ContainsKey(size)) stats[size] = new Dictionary<Encoding, int>();
                            if (!stats[size].ContainsKey(currentEncoding)) stats[size][currentEncoding] = 0;
                            stats[size][currentEncoding]++;
                        };

                        ForEachNGram(ms, 1, ngProc);
                        ForEachNGram(ms, 2, ngProc);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error processing directory: {0}", ex.ToString());
            }

            // build naive bayes models
            foreach (var basePair in model)
            {
                var ngSize = basePair.Key;
                var ngStat = basePair.Value;

                var ln0 = new Dictionary<Encoding,double>();
                var pw = new Dictionary<int,Dictionary<Encoding,double>>();

                var totalSum = stats[ngSize].Sum(x => x.Value);
                foreach (var encoding in encodings) ln0[encoding] = Math.Log((stats[ngSize][encoding]*1.0) / (totalSum - stats[ngSize][encoding]));

                var sums = new Dictionary<int,double>();
                var codes = ngStat.Keys.ToList();
                foreach (var code in codes) sums[code] = ((double)ngStat[code].Sum(x => x.Value))/totalSum;
                codes.Sort((a, b) => sums[b].CompareTo(sums[a]));
                double acc = 0.0;
                foreach (var code in codes)
                {
                    acc += sums[code];
                    sums[code] = acc;
                }

                int left = 0, right = codes.Count - 1;
                while ((left+1) < right)
                {
                    int middle = (left + right) >> 1;
                    int midcode = codes[middle];
                    if (sums[midcode] < 0.95)
                        left = middle;
                    else
                        right = middle;
                }

                for (int idx = right; idx < codes.Count; idx++) ngStat.Remove(codes[idx]);

                foreach (var pair2 in ngStat)
                {
                    var code = pair2.Key;
                    var counts = pair2.Value;
                    var sum = counts.Sum(x => x.Value) + encodings.Length;
                    pw[code] = new Dictionary<Encoding, double>();
                    foreach (var encoding in encodings)
                    {
                        var cnt = (double)(counts.ContainsKey(encoding) ? counts[encoding] + 1 : 1);
                        pw[code][encoding] = cnt/sum;
                    }
                }


            }

            Console.WriteLine("press enter...");
            Console.ReadLine();
        }

        private static int ForEachNGram(MemoryStream stream, int ngSize, Action<int, int> ngProc)
        {
            stream.Position = 0;
            int idx = 0, mask = (1 << (8 * ngSize)) - 1, cur = 0;
            while (stream.Position < stream.Length)
            {
                var bt = stream.ReadByte();
                if (bt == -1) break;
                idx++;
                cur = ((cur << 8) & mask) | bt;
                if (idx >= ngSize) ngProc(ngSize, cur);
            }
            return idx - ngSize + 1;
        }
    }
}
