using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Canvas
{
	static class Program
	{
		public static int TracePaint = 1;
		public static string AppName = "GLUE";
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//CommonTools.Tracing.EnableTrace();
			CommonTools.Tracing.AddId(TracePaint);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            MainWin thisisthemainwindow = new MainWin();
            Application.Run(thisisthemainwindow);

			CommonTools.Tracing.Terminate();
		}
	}
}