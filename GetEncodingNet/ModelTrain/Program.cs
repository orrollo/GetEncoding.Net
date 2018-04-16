using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var encodings = new Encoding[]
                {
                    Encoding.UTF8, Encoding.UTF32, Encoding.GetEncoding(1251), Encoding.GetEncoding(866),
                    Encoding.GetEncoding(20866)
                };

            BayesModels models = new BayesModels();
            var modelFileName = "model.dat";

            if (File.Exists(modelFileName))
                models.Load(modelFileName);
            else
            {
                models.TrainModels(encodings, 0.9, Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt", SearchOption.AllDirectories), Encoding.GetEncoding(1251));
                models.Save(modelFileName);
            }

            var example = "Трико";

            foreach (var bayesModel in models)
            {
                Console.WriteLine("model code size: {0} byte(s)", bayesModel.CodeSize);
                foreach (var encoding in encodings)
                {
                    var bytes = encoding.GetBytes(example);
                    var marks = bayesModel.GetMarks(bytes);
                    var codePage = models[0].GetBestMark(marks);
                    Console.WriteLine("for codepage {0} defined codepage is {1} (p={2:f3})", encoding.CodePage, codePage, marks[codePage]);
                }
            }

            Console.WriteLine("in composite mode");
            foreach (var encoding in encodings)
            {
                var bytes = encoding.GetBytes(example);
                var marks = models.GetMarks(bytes);
                var codePage = models.GetBestMark(marks);
                Console.WriteLine("for codepage {0} defined codepage is {1} (p={2:f3})", encoding.CodePage, codePage, marks[codePage]);
            }

            Console.WriteLine();

            Console.WriteLine("model 1 script will be saved to <script1.zzz>");
            File.WriteAllLines("script1.zzz", SrciptToCs.Process(models[0].ToScript(), "model_1"));
            Console.WriteLine("model 2 script will be saved to <script2.zzz>");
            File.WriteAllLines("script2.zzz", SrciptToCs.Process(models[1].ToScript(), "model_2"));
            Console.WriteLine("model 2 script will be saved to <script2b.zzz>");
            File.WriteAllLines("script2b.zzz", SrciptToCs.Process(models[1].ToScript(true), "model_2b"));

            Console.WriteLine();

            foreach (var encoding in encodings)
            {
                var bytes = encoding.GetBytes(example);
                int codePage;
                double prob;
                ResultClass.model_1(bytes, out codePage, out prob);
                Console.WriteLine("model_1: for codepage {0} defined codepage is {1} (p={2:f3})", encoding.CodePage, codePage, prob);
                ResultClass.model_2(bytes, out codePage, out prob);
                Console.WriteLine("model_2: for codepage {0} defined codepage is {1} (p={2:f3})", encoding.CodePage, codePage, prob);
            }

            // speed measure
            var src = File.ReadAllBytes(@".\texts\anna_kar.txt");

            int zcodePage;
            double zprob;
            var sw = new Stopwatch();

            Console.WriteLine();

            sw.Restart();
            ResultClass.model_1(src, out zcodePage, out zprob);
            sw.Stop();
            Console.WriteLine("model_1: defined codepage is {0} (p={1:f3}); time = {2}ms", zcodePage, zprob, sw.ElapsedMilliseconds);

            sw.Restart();
            var xmarks = models[0].GetMarks(src);
            var xbest = models[0].GetBestMark(xmarks);
            sw.Stop();
            Console.WriteLine("model_0: defined codepage is {0} (p={1:f3}); time = {2}ms", xbest, xmarks[xbest], sw.ElapsedMilliseconds);

            sw.Restart();
            ResultClass.model_2(src, out zcodePage, out zprob);
            sw.Stop();
            Console.WriteLine("model_2: defined codepage is {0} (p={1:f3}); time = {2}ms", zcodePage, zprob, sw.ElapsedMilliseconds);

            sw.Restart();
            ResultClass.model_2b(src, out zcodePage, out zprob);
            sw.Stop();
            Console.WriteLine("model_2b: defined codepage is {0} (p={1:f3}); time = {2}ms", zcodePage, zprob, sw.ElapsedMilliseconds);

            Console.WriteLine();
            Console.WriteLine("press enter...");
            Console.ReadLine();


        }

        //private static void ReadModels(List<BayesModel> models, string modelFileName)
        //{
        //    using (var rdr = new BinaryReader(new FileStream(modelFileName, FileMode.Open), Encoding.UTF8, false))
        //    {
        //        var count = rdr.ReadInt32();
        //        for (int i = 0; i < count; i++) models.Add(new BayesModel(rdr));
        //    }
        //}
    }
}
