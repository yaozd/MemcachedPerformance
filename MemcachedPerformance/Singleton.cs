using System;

namespace MemcachedPerformance
{
    public sealed class Singleton
    {
        //重新加载实体
        private static readonly object LockObject = new object();

        private static readonly Lazy<Singleton> lazy =
            new Lazy<Singleton>(() => new Singleton());

        //客户端数量
        private int _clientNum = 2;
        //客户端当前的索引
        private int _index = 0;
        private MemcachedClientCache _client;
        private MemcachedClientCache[] _clients;

        public static Singleton Instance
        {
            get { return lazy.Value; }
        }

        private Singleton()
        {
            InitMemcachedClients();
        }

        public MemcachedClientCache MemcachedClient()
        {
            //解决“Memcached黑色3秒事件”问题
            //Memcached的socket的连接时间大约为12小时间会自动回收断开断开后会有3秒内无法插入读取
            //当前时间是指在同一分钟内使用相同的客户端实例对象
            //如果下个时间点nextTime不在同一分钟内，则进行预热加载客户端实例对象
            var currentTime = DateTime.Now;
            var nextTime = currentTime.AddMinutes(10);
            var t = currentTime.Hour%_clientNum;
            var tt = nextTime.Hour%_clientNum;
            //var t = currentTime.Second % _clientNum;
            //var tt = nextTime.Second % _clientNum;
            _client = _clients[t];
            //目前每隔1分钟转换一个实体
            if (tt != _index)
            {
                //通过锁来控制客户端的释放
                lock (LockObject)
                {
                    //用最小的性能代价-尽可能的保证线程安全
                    if (tt != _index)
                    {
                        _index = tt;
                        var t1 = (t + 1)%_clientNum;
                        //重新加载实体
                        _clients[t1].Dispose();
                        _clients[t1] = null;
                        _clients[t1] = LoadClient();
                    }
                }
            }
            if (_client == null) SetMemcachedClient();
            return _client;
        }

        private void SetMemcachedClient()
        {
            _client = LoadClient();
        }

        private void InitMemcachedClients()
        {
            _clients = new MemcachedClientCache[_clientNum];
            for (int i = 0; i < _clientNum; i++)
            {
                _clients[i] = LoadClient();
            }
        }

        private static MemcachedClientCache LoadClient()
        {
            return new MemcachedClientCache(new[] {"10.48.251.81:11211"});
        }
    }
}