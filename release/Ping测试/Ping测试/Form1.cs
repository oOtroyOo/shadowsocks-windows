using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Shadowsocks.Model;

using Timer = System.Windows.Forms.Timer;

namespace Ping测试
{
    public partial class Form1 : Form
    {

        private Thread mainThread;
        private Configuration conf;
        public Form1()
        {
            InitializeComponent();
            mainThread = Thread.CurrentThread;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }

        public void LoadData()
        {
            timer1.Stop();
            Item.Clear();
            checkedListBox1.Items.Clear();
            try
            {
                conf = Configuration.Load();
                if (conf.configs.Count < 2)
                {
                    MessageBox.Show("没有读取到服务器信息");
                }
                foreach (Server confConfig in conf.configs)
                {
                    Item item = new Item((confConfig.server));
                    checkedListBox1.Items.Add(item);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        public void SetItem(Item item)
        {
            if (Thread.CurrentThread.ManagedThreadId != mainThread.ManagedThreadId)
            {
                Invoke(new Action(() => { SetItem(item); }));
                return;
            }
            checkedListBox1.SetItemChecked(item.index, item.isValid);
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;

            LoadData();
            timer.Stop();

        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.checkedListBox1.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Server server = conf.configs[index];

                if (MessageBox.Show("是否应用" + server.server, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conf = Configuration.Load();
                    conf.index = index;
                    Configuration.Save(conf);
                    Process[] p = Process.GetProcessesByName("shadowsocks");
                    string filename = "shadowsocks.exe";
                    for (int i = 0; i < p.Length; i++)
                    {
                        filename = p[i].MainModule.FileName;
                        p[i].Kill();
                    }
                    Thread.Sleep(500);
                    Process.Start(filename);
                }
            }
        }


    }
}
