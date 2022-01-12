using EfCoreHelper.FilePart;

string path;

do
{
	Console.Clear();
	Console.WriteLine("Give an absolute path to the root directory with contexts and models:");
	path = Console.ReadLine()!;
	
} while (string.IsNullOrWhiteSpace(path) || Directory.Exists(path) is not true);

var fileContext = new FileManager(path ?? throw new InvalidOperationException());
fileContext.DoWind();

Console.WriteLine("Ok");
Console.WriteLine("You should only add the missing references in the configurations and do clean up the root directory");
Console.ReadKey();