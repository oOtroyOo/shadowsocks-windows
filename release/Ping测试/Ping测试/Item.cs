using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ping测试
{
    public class Item
    {
        public static List<Item> items = new List<Item>();
        static List<Ping> pingquene = new List<Ping>();
        public int index;
        private string _ip;
        private long pingtime;
        private Task task;
        private CancellationTokenSource token;
        public bool isValid
        {
            get { return status == IPStatus.Success; }
        }
        private IPStatus status;
        private Item()
        {

        }
        public Item(string ip)
        {
            index = items.Count;
            items.Add(this);

            _ip = ip;
            token = new CancellationTokenSource();

            task = Task.Run(PingTask, token.Token);
        }

        public string GetIp()
        {
            return _ip;
        }

        async Task PingTask()
        {

            Ping ping = new Ping();
            status = IPStatus.Unknown;
            while (pingquene.Count > 5)
            {
                await Task.Delay(100);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
            pingquene.Add(ping);
         
            var pingres = await ping.SendPingAsync(_ip);
          
            pingtime = pingres.RoundtripTime;
            status = pingres.Status;
            pingquene.Remove(ping);
            if (token.IsCancellationRequested)
            {
                return;
            }
            Program.form1.SetItem(this);
        }

        public override string ToString()
        {
            return _ip + " " + (isValid ? pingtime.ToString() : Enum.GetName(typeof(IPStatus), status));
        }

        public static void Clear()
        {
            foreach (Item item in items)
            {
                item.token.Cancel();
            }
            pingquene.Clear();
            items.Clear();
        }
        
    }
}
