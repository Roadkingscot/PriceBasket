using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Tests1
{
	/// <span class="code-SummaryComment"><summary></span>
	/// Testing application BuildAdvantage.Console.exe
	/// <span class="code-SummaryComment"></summary></span>
	[TestFixture]
	public class ProgramTests
	{

		TextWriter m_normalOutput;
		StringWriter m_testingConsole;
		StringBuilder m_testingSB;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			try
			{
			// Set current folder to testing folder
			string assemblyCodeBase = 
				System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

			// Get directory name
			string dirName = Path.GetDirectoryName(assemblyCodeBase);

			// remove URL-prefix if it exists
			if (dirName.StartsWith("file:\\"))
				dirName = dirName.Substring(6);

			// set current folder
				dirName = "/Users/scothaynes/Projects/PriceBasket/PriceBasket/bin/debug";
			Environment.CurrentDirectory = dirName;

			// Initialize string builder to replace console
			System.Console.Clear();
			m_testingSB = new StringBuilder();
			m_testingConsole = new StringWriter(m_testingSB);

			// swap normal output console with testing console - to reuse 
			// it later
			m_normalOutput = System.Console.Out;
			System.Console.SetOut(m_testingConsole);
			}
			catch (Exception ex)
			{
					string myerr = ex.ToString();
			}
		}
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			// set normal output stream to the console
			System.Console.SetOut(m_normalOutput);
		}

		[SetUp]
		public void SetUp()
		{
			// clear string builder
			m_testingSB.Remove(0, m_testingSB.Length);
		}

		[TearDown]
		public void TearDown()
		{
			// Verbose output in console
			m_normalOutput.Write(m_testingSB.ToString());
		}
		private int StartConsoleApplication(string arguments)
		{
			Process proc = new Process();
			try {
			// Initialize process here
			proc.StartInfo.FileName = "PriceBasket.exe";
			// add arguments as whole string
			proc.StartInfo.Arguments = arguments;

			// use it to start from testing environment
			proc.StartInfo.UseShellExecute = false;

			// redirect outputs to have it in testing console
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;

			// set working directory
			proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

			// start and wait for exit
			proc.Start();
			proc.WaitForExit();

			// get output to testing console.
			System.Console.WriteLine(proc.StandardOutput.ReadToEnd());
			System.Console.Write(proc.StandardError.ReadToEnd());
				// return exit code
				return proc.ExitCode;

		}
			catch(Exception ex) 
		{
				string myerr = ex.ToString();
				return 0;
		}

		}
		[Test]
		public void ShowCmdHelpIfNoArguments()
		{
			// Check exit is normal
			Assert.AreEqual(0, StartConsoleApplication("Soup"));

			// Check that help information shown correctly.
			Assert.IsTrue(m_testingSB.ToString().Contains(
				"Subtotal"));
		}

	}
}