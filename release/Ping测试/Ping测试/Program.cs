using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Ping测试
{
    static class Program
    {
        public static Form1 form1;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DirectoryInfo dir = new DirectoryInfo("./");
            foreach (FileInfo fi in dir.GetFiles())
            {
                if (fi.Name.Equals("shadowsocks.exe", StringComparison.CurrentCultureIgnoreCase))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(form1 = new Form1());
                    return;
                }
            }
            MessageBox.Show("请放置在Shadowsocks同文件夹运行");

        }
    }
}
