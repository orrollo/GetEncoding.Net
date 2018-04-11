using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ModelTrain
{
    public class CodeModel : Dictionary<int, Double>
    {
        
    }

    public class BayesModel 
    {
        public int CodeSize { get; set; }
        public Dictionary<int,double> P0 = new Dictionary<int, double>();
        public Dictionary<int,CodeModel> Codes = new Dictionary<int, CodeModel>();

        public BayesModel()
        {
            
        }

        public BayesModel(BinaryReader reader) : this()
        {
            Load(reader);
        }

        public void Save(Stream stream)
        {
            using (var wrt = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                Save(wrt);
            }
        }

        public void Save(BinaryWriter wrt)
        {
            wrt.Write(CodeSize);
            wrt.Write(P0.Count);
            foreach (var pair in P0)
            {
                wrt.Write(pair.Key);
                wrt.Write(pair.Value);
            }
            wrt.Write(Codes.Count);
            foreach (var pair in Codes)
            {
                wrt.Write(pair.Key);
                var model = pair.Value;
                wrt.Write(model.Count);
                foreach (var pair2 in model)
                {
                    wrt.Write(pair2.Key);
                    wrt.Write(pair2.Value);
                }
            }
            wrt.Flush();
        }

        public void Load(Stream stream)
        {
            using (var rdr = new BinaryReader(stream,Encoding.UTF8,true))
            {
                Load(rdr);
            }
        }

        public void Load(BinaryReader rdr)
        {
            P0.Clear();
            Codes.Clear();
            CodeSize = rdr.ReadInt32();
            int count = rdr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int key = rdr.ReadInt32();
                double value = rdr.ReadDouble();
                P0.Add(key, value);
            }
            count = rdr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int key = rdr.ReadInt32();
                var codeModel = new CodeModel();
                int count2 = rdr.ReadInt32();
                for (int j = 0; j < count2; j++) codeModel.Add(rdr.ReadInt32(), rdr.ReadDouble());
                Codes[key] = codeModel;
            }
        }

        public Dictionary<int,double> GetMarks(byte[] bytes)
        {
            var ret = new Dictionary<int,double>();

            int pos = 0;
            Func<int> readByte = () =>
            {
                int byteValue = pos < bytes.Length ? bytes[pos] : -1;
                pos++;
                return byteValue;
            };

            foreach (var pair in P0) ret[pair.Key] = Math.Log(pair.Value/(1.0 - pair.Value));
            NGramHelper.ForEachNGram(CodeSize, readByte, (size, value) =>
            {
                if (!Codes.ContainsKey(value)) return;
                foreach (var pair in Codes[value])
                {
                    if (!ret.ContainsKey(pair.Key)) continue;
                    ret[pair.Key] += Math.Log(pair.Value/(1.0 - pair.Value));
                }
            });

            var keys = ret.Keys.ToArray();
            foreach (var key in keys)
            {
                var v = Math.Min(100.0, ret[key]);
                var x = Math.Exp(v);
                ret[key] = x / (1.0 + x);
            }

            return ret;
        }

        public int GetBestMark(byte[] bytes)
        {
            return GetBestMark(GetMarks(bytes));
        }

        public int GetBestMark(Dictionary<int, double> marks)
        {
            int? ret = null;
            foreach (var pair in marks)
            {
                if (ret != null && marks[ret.Value] >= pair.Value) continue;
                ret = pair.Key;
            }
            return ret ?? -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var encodings = new Encoding[]
                {
                    Encoding.UTF8, Encoding.UTF32, Encoding.GetEncoding(1251), Encoding.GetEncoding(866),
                    Encoding.GetEncoding(20866)
                };

            List<BayesModel> models = new List<BayesModel>();
            var modelFileName = "model.dat";

            if (!File.Exists(modelFileName))
                BuildModels(models, modelFileName, encodings, 0.75);
            else
                ReadModels(models, modelFileName);

            var example = "Три кота, три хвоста";

            foreach (var bayesModel in models)
            {
                Console.WriteLine("mode code size: {0} byte(s)", bayesModel.CodeSize);
                foreach (var encoding in encodings)
                {
                    var bytes = encoding.GetBytes(example);
                    var marks = bayesModel.GetMarks(bytes);
                    var codePage = models[0].GetBestMark(marks);
                    Console.WriteLine("for codepage {0} defined codepage is {1} (p={2:f3})", encoding.CodePage, codePage, marks[codePage]);
                }
            }


            Console.WriteLine("press enter...");
            Console.ReadLine();
        }

        private static void ReadModels(List<BayesModel> models, string modelFileName)
        {
            using (var rdr = new BinaryReader(new FileStream(modelFileName, FileMode.Open), Encoding.UTF8, false))
            {
                var count = rdr.ReadInt32();
                for (int i = 0; i < count; i++) models.Add(new BayesModel(rdr));
            }
        }

        private static void BuildModels(List<BayesModel> models, string modelFileName, Encoding[] encodings, double borderValue)
        {
            var model = new Dictionary<int, Dictionary<int, Dictionary<Encoding, int>>>();
            var stats = new Dictionary<int, Dictionary<Encoding, int>>();

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

                        NGramHelper.ForEachNGram(ms, 1, ngProc);
                        NGramHelper.ForEachNGram(ms, 2, ngProc);
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

                var bayesModel = new BayesModel() {CodeSize = ngSize};

                var totalSum = stats[ngSize].Sum(x => x.Value);
                foreach (var encoding in encodings) bayesModel.P0[encoding.CodePage] = (stats[ngSize][encoding]*1.0)/totalSum;

                var sums = new Dictionary<int, double>();
                var codes = ngStat.Keys.ToList();
                foreach (var code in codes) sums[code] = ((double) ngStat[code].Sum(x => x.Value))/totalSum;
                codes.Sort((a, b) => sums[b].CompareTo(sums[a]));
                double acc = 0.0;
                foreach (var code in codes)
                {
                    acc += sums[code];
                    sums[code] = acc;
                }

                int left = 0, right = codes.Count - 1;
                while ((left + 1) < right)
                {
                    int middle = (left + right) >> 1;
                    int midcode = codes[middle];
                    if (sums[midcode] < borderValue)
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
                    bayesModel.Codes[code] = new CodeModel();
                    foreach (var encoding in encodings)
                    {
                        var cnt = (double) (counts.ContainsKey(encoding) ? counts[encoding] + 1 : 1);
                        bayesModel.Codes[code][encoding.CodePage] = cnt/sum;
                    }
                }

                models.Add(bayesModel);
            }

            using (var stream = new FileStream(modelFileName, FileMode.Create))
            {
                using (var wrt = new BinaryWriter(stream, Encoding.UTF8, true))
                {
                    wrt.Write(models.Count);
                    foreach (var bayesModel in models) bayesModel.Save(wrt);
                    wrt.Flush();
                }
                stream.Flush();
            }
        }
    }
}
