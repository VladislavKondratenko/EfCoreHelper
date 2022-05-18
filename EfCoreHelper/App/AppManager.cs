using EfCoreHelper.FilePart;

namespace EfCoreHelper.App;

public class AppManager
{
	private string _path = null!;

	public void Run()
	{
		bool isWrongPath;

		do
		{
			isWrongPath = TryAskPath();
		} while (isWrongPath);

		RunNewSession();

		PrintStatistic();
		PrintFinalMessages();
	}

	private void RunNewSession()
	{
		AppProcess.NewSession();
		var fileContext = new FileManager(_path ?? throw new InvalidOperationException());
		fileContext.DoWind();
	}

	private bool TryAskPath()
	{
		PrintAskMessage();
		_path = Console.ReadLine()!;

		var isWrongPath = string.IsNullOrWhiteSpace(_path) || Directory.Exists(_path) is not true;

		if (isWrongPath)
			PrintWrongDirectory();

		return isWrongPath;
	}

	private void PrintStatistic()
	{
		var session = AppProcess.CurrentSession;
		Console.WriteLine("Ok");
		Console.WriteLine($"{session.Contexts.Count} contexts were processed.");
		Console.WriteLine($"{session.Models.Count} entities were processed.");
		Console.WriteLine($"{session.Configurations.Count} configurations were created.");
	}

	private static void PrintAskMessage()
	{
		Console.Clear();
		Console.WriteLine("Put an absolute path to the root directory with contexts and models:");
	}

	private static void PrintFinalMessages()
	{
		Console.WriteLine();
		Console.WriteLine("You should only do clean up the root directory");
		
		AskToStopOrContinue();
	}

	private void PrintWrongDirectory()
	{
		Console.WriteLine("The path is not a directory.");
		Console.WriteLine("Press any key to try again.");
		AskToStopOrContinue();
	}

	private static void AskToStopOrContinue()
	{
		Console.WriteLine("Press any key to do else or Esc to exit");
		var consoleKeyInfo = Console.ReadKey();

		if (consoleKeyInfo.Key is ConsoleKey.Escape)
			Environment.Exit(0);
	}
}