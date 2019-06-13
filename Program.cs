using System;
using CSRedis;
using Newtonsoft.Json;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello CSRedis!");

      CSRedisClient rds;
      test_cs(out rds);
      WebPost doc = new WebPost { OpType = "DOC",BID = 102, PointID=501, OpContent = "1", Describe="DO 控制" };
      rds.PublishAsync("web", $"{JsonConvert.SerializeObject(doc)}");
      WebPost tmps = new WebPost { OpType = "TMPS", BID = 102, PointID = 8503016, OpContent = "45.0,38", Describe = "温控联动开启" };
      rds.PublishAsync("web", $"{JsonConvert.SerializeObject(tmps)}");
      WebPost tmpc = new WebPost { OpType = "TMPC", BID = 102, PointID = 8503016, OpContent = "0,0", Describe = "温控联动取消" };
      rds.PublishAsync("web", $"{JsonConvert.SerializeObject(tmpc)}");
      Console.ReadKey();
      rds.Dispose();
    }

    static void test_cs(out CSRedisClient rds)
    {
      rds = new CSRedis.CSRedisClient("127.0.0.1:6379,password=lead-it_rds");
      //   var sub1 = redis.SubscribeList("web", msg => Console.WriteLine($"sub1 -> list1 : {msg}"));

      var sub6 = rds.Subscribe(("web", msg => Rcv(msg.Body,msg.Channel)));
      rds.PublishAsync("web", $"add at{DateTime.Now}");
    }
    static void Rcv(string Msg,string channel)
      {
      Console.WriteLine($"{DateTime.Now.ToLongDateString()}|Rcv:{channel},Msg:{Msg}");
  }
    public class WebPost
    {
      public string OpType;
      public int BID;
      public int PointID;
      public string OpContent;
      public string Describe;
    }
  }
}
