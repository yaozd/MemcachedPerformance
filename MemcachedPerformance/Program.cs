using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemcachedPerformance
{
    class Program
    {
        static readonly MyLog log = new MyLog();
        //原子计数
        public static int Count;
        //原子计数
        public static int ErrorCount;
        //
        public static int TestDataSize = 1 * 1024;
        //
        public static Byte[] TestData;
        static MemcachedClientCache client = new MemcachedClientCache(new[] { "10.48.251.81:11211" });
        static void Main(string[] args)
        {
            log.Write(string.Format("1-MemcachedPerformanceTest-{0}",DateTime.Now));
            BuildTestData();
            while (true)
            {
                try
                {
                    //
                    PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
                    PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
                    //
                    throw new Exception("aa");
                }
                catch (Exception ex)
                {
                    var current = Interlocked.Increment(ref ErrorCount);
                    log.Write(string.Format("2-Error-Count-{0}-{1}",current,DateTime.Now));
                    log.Write(string.Format("2-Error-Count-{0}-{1}",current,ex.Message));
                }

            }
            //
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            PerformanceTest.Time("Test_Set", 40, 5000, (Test_Set));
            PerformanceTest.Time("Test_Get", 40, 5000, (Test_Get));
            //
            Console.WriteLine("Test End");
            Console.Read();
        }

        static void Test_Set()
        {
            MemcachedClientCache client = Singleton.Instance.MemcachedClient();
            var s = client.Set("201606141340", TestData, new TimeSpan(0, 1, 0));
            if (!s)
            {
                var current = Interlocked.Increment(ref Count);
                Console.WriteLine("{0} {1}", current, s);
                log.Write(string.Format("Test_Set--{0} {1}--Time:{2}", current, s, DateTime.Now));
            }
            //
        }

        static void Test_Get()
        {
            MemcachedClientCache client = Singleton.Instance.MemcachedClient();
            var s = client.Get("201606141340");
            if (s == null)
            {
                Console.Write(s);
                log.Write(string.Format("Test_Get--{0}--Time:{1} ", s, DateTime.Now));
            }
            //
        }
        static void Test_DeadTimeout()
        {
            Thread.Sleep(1000);
            MemcachedClientCache client = Singleton.Instance.MemcachedClient();
            var s = client.Set("201606141340", TestData, new TimeSpan(0, 10, 0));
            if (!s)
            {
                var current = Interlocked.Increment(ref Count);
                Console.Write("{0} {1}", current, s);
            }
            Console.WriteLine(s);
            //
        }
        private static void BuildTestData()
        {
            TestData = new byte[TestDataSize];
            for (int i = 0; i < TestDataSize; i++)
            {
                TestData[i] = 0x30;
            }
        }
        private static void Example()
        {
            MemcachedClientCache c = new MemcachedClientCache(new[] { "10.48.251.81:11211" });
            c.Add("201606141340", "1", new TimeSpan(0, 10, 0));
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var s = c.Set("201606141340", "1", new TimeSpan(0, 10, 0));
                if (!s)
                {
                    Console.Write(s);
                }
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}
