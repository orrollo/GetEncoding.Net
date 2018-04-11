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
                Console.WriteLine("mode code size: {0} byte(s)", bayesModel.CodeSize);
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
