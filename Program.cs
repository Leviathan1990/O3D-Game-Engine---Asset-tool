using System;
using System.Windows.Forms;

namespace AssetTool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form1 = new Form1(null);
            Form3 form3 = new Form3(form1);
            form1 = new Form1(form3);

            Application.Run(form1);
        }
    }
}
