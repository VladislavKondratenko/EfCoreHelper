using EfCoreHelper.App;
using EfCoreHelper.FilePart;

string path;
bool isFile;

do
{
	Console.Clear();
	Console.WriteLine("Give an absolute path to the root directory with contexts and models:");
	path = Console.ReadLine()!;

	isFile = string.IsNullOrWhiteSpace(path) || Directory.Exists(path) is not true;

	if (isFile)
		PrintWrongDirectory();
} while (isFile);

AppProcess.NewSession();
var fileContext = new FileManager(path ?? throw new InvalidOperationException());
fileContext.DoWind();

Console.WriteLine("Ok");
Console.WriteLine("You should only do clean up the root directory");
Console.ReadKey();

static void PrintWrongDirectory()
{
	Console.WriteLine("The path is not a directory.");
	Console.WriteLine("Press any key to try again.");
	Console.ReadKey();
}