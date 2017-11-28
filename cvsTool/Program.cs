using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using cvsTool.Controllor;
using cvsTool.View;

namespace cvsTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PersonControllor controllor = new PersonControllor(new PersonForm());
            Application.Run(controllor.View);
        }
    }
}
