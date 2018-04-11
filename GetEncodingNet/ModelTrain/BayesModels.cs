using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelTrain
{
    public class BayesModels : List<BayesModel>
    {
        public BayesModels() : base()
        {
            
        }

        public BayesModels(string fileName) : this()
        {
            Load(fileName);
        }

        public BayesModels(Stream stream) : this()
        {
            Load(stream);
        }

        public int GetBestMark(byte[] bytes)
        {
            var marks = GetMarks(bytes);
            return GetBestMark(marks);
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

        public Dictionary<int, double> GetMarks(byte[] bytes)
        {
            var xmarks = new Dictionary<int, double>();
            foreach (var model in this)
            {
                var marks = model.GetMarks(bytes);
                var codePage = model.GetBestMark(marks);
                if (!xmarks.ContainsKey(codePage)) xmarks[codePage] = 0;
                xmarks[codePage] = 1.0 - (1.0 - xmarks[codePage])*(1.0 - marks[codePage]);
            }
            return xmarks;
        }

        public void Load(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                Load(stream);
            }
        }

        public void Load(Stream stream)
        {
            using (var rdr = new BinaryReader(stream, Encoding.UTF8, true))
            {
                Load(rdr);
            }
        }

        public void Load(BinaryReader rdr)
        {
            Clear();
            int count = rdr.ReadInt32();
            for (int i = 0; i < count; i++) Add(new BayesModel(rdr));
        }

        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                Save(stream);
            }
        }

        public void Save(Stream stream)
        {
            using (var wrt = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                Save(wrt);
            }
            stream.Flush();
        }

        public void Save(BinaryWriter wrt)
        {
            wrt.Write(Count);
            foreach (var model in this) model.Save(wrt);
            wrt.Flush();
        }

        class TrainContext
        {
            public readonly Dictionary<int, Dictionary<int, Dictionary<Encoding, int>>> Model = new Dictionary<int, Dictionary<int, Dictionary<Encoding, int>>>();
            public readonly Dictionary<int, Dictionary<Encoding, int>> Stats = new Dictionary<int, Dictionary<Encoding, int>>();
            public Encoding CurrentEncoding;
            public Encoding[] encodings;

            internal void ProcessNGram(int size, int value)
            {
                if (!Model.ContainsKey(size)) Model[size] = new Dictionary<int, Dictionary<Encoding, int>>();
                if (!Model[size].ContainsKey(value)) Model[size][value] = new Dictionary<Encoding, int>();
                if (!Model[size][value].ContainsKey(CurrentEncoding)) Model[size][value][CurrentEncoding] = 0;
                Model[size][value][CurrentEncoding]++;
                //
                if (!Stats.ContainsKey(size)) Stats[size] = new Dictionary<Encoding, int>();
                if (!Stats[size].ContainsKey(CurrentEncoding)) Stats[size][CurrentEncoding] = 0;
                Stats[size][CurrentEncoding]++;
            }

            internal void ProcessText(string text)
            {
                foreach (var encoding in encodings)
                {
                    var ms = new MemoryStream(encoding.GetBytes(text));
                    CurrentEncoding = encoding;
                    NGramHelper.ForEachNGram(ms, 1, ProcessNGram);
                    NGramHelper.ForEachNGram(ms, 2, ProcessNGram);
                }
            }
        }

        public void TrainModels(Encoding[] encodings, double borderValue, string[] files, Encoding srcEncoding)
        {
            var ctx = TrainFiles(encodings, files, srcEncoding);

            // build naive bayes models
            foreach (var basePair in ctx.Model)
            {
                var ngSize = basePair.Key;
                var ngStat = basePair.Value;

                var bayesModel = new BayesModel() {CodeSize = ngSize};

                double totalSum = ctx.Stats[ngSize].Sum(x => x.Value);
                foreach (var encoding in encodings) bayesModel.P0[encoding.CodePage] = ctx.Stats[ngSize][encoding]/totalSum;

                var sums = new Dictionary<int, double>();
                var codes = ngStat.Keys.ToList();
                foreach (var code in codes) sums[code] = ngStat[code].Sum(x => x.Value)/totalSum;
                codes.Sort((a, b) => sums[b].CompareTo(sums[a]));
                double acc = 0.0;

                int mode = 0;
                for (int index = 0; index < codes.Count; index++)
                {
                    var code = codes[index];
                    acc += sums[code];
                    if (mode == 0 && acc >= borderValue) mode = 1;
                    if (mode == 1) ngStat.Remove(code);
                }

                //foreach (var code in codes)
                //{
                //    acc += sums[code];
                //    sums[code] = acc;
                //}
                //int left = 0, right = codes.Count - 1;
                //while ((left + 1) < right)
                //{
                //    int middle = (left + right) >> 1;
                //    int midcode = codes[middle];
                //    if (sums[midcode] < borderValue)
                //        left = middle;
                //    else
                //        right = middle;
                //}
                //for (int idx = right; idx < codes.Count; idx++) ngStat.Remove(codes[idx]);

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

                Add(bayesModel);
            }

            //using (var stream = new FileStream(modelFileName, FileMode.Create))
            //{
            //    using (var wrt = new BinaryWriter(stream, Encoding.UTF8, true))
            //    {
            //        wrt.Write(models.Count);
            //        foreach (var bayesModel in models) bayesModel.Save(wrt);
            //        wrt.Flush();
            //    }
            //    stream.Flush();
            //}
        }

        private static TrainContext TrainFiles(Encoding[] encodings, string[] files, Encoding srcEncoding)
        {
            var ctx = new TrainContext();
            try
            {
                foreach (var file in files)
                {
                    var srcText = File.ReadAllText(file, srcEncoding);
                    ctx.encodings = encodings;
                    ctx.ProcessText(srcText.ToLower());
                    ctx.ProcessText(srcText.ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ctx;
        }
    }
}