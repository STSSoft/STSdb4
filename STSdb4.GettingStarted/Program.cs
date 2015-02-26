﻿using DatabaseBenchmark;
using STSdb4.Data;
using STSdb4.Database;
using STSdb4.General.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using STSdb4.Storage;
using System.Globalization;
using System.Threading;
using STSdb4.WaterfallTree;
using STSdb4.General.Collections;
using STSdb4.General.Comparers;

namespace STSdb4.GettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            Example(1000000, KeysType.Random);

            Console.ReadKey();
        }

        private static void Example(int tickCount, KeysType keysType)
        {
            Stopwatch sw = new Stopwatch();
            const string FILE_NAME = "test.stsdb4";
            File.Delete(FILE_NAME);

            //insert
            Console.WriteLine("Inserting...");
            sw.Reset();
            sw.Start();
            int c = 0;
            using (IStorageEngine engine = STSdb.FromFile(FILE_NAME))
            {
                ITable<long, Tick> table = engine.OpenXTable<long, Tick>("table");

                foreach (var kv in TicksGenerator.GetFlow(tickCount, keysType)) //generate random records
                {
                    table[kv.Key] = kv.Value;

                    c++;
                    if (c % 100000 == 0)
                        Console.WriteLine("Inserted {0} records", c);
                }

                engine.Commit();
            }
            sw.Stop();
            Console.WriteLine("Insert speed:{0} rec/sec", sw.GetSpeed(tickCount));

            //read
            Console.WriteLine("Reading...");
            sw.Reset();
            sw.Start();
            c = 0;
            using (IStorageEngine engine = STSdb.FromFile(FILE_NAME))
            {
                ITable<long, Tick> table = engine.OpenXTable<long, Tick>("table");

                foreach (var row in table) //table.Forward(), table.Backward()
                {
                    //Console.WriteLine("{0} {1}", row.Key, row.Value);

                    c++;
                    if (c % 100000 == 0)
                        Console.WriteLine("Read {0} records", c);
                }
            }
            sw.Stop();
            Console.WriteLine("Read speed:{0} records", sw.GetSpeed(c));
        }
    }
}
