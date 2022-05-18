using System.Text;
using System.Text.RegularExpressions;
using EfCoreHelper.App;
using EfCoreHelper.TextPart;

namespace EfCoreHelper.FilePart;

public class FileManager
{
	private readonly string _rootPath;

	public FileManager(string rootPath)
	{
		_rootPath = rootPath;
	}

	public void DoWind()
	{
		var paths = FindAllClasses(_rootPath).ToList();
		ProcessModels(paths);
		ProcessContexts(paths);
	}

	private static void ProcessModels(IEnumerable<string> paths)
	{
		var modelPaths = paths
						.Where(e => e != string.Empty)
						.Where(p => Regex.Match(p, @"(Models|Entities)").Success);

		foreach (var path in modelPaths)
			RewriteModel(path);
	}

	private static void RewriteModel(string path)
	{
		var text = File.ReadAllText(path);
		
		var classModel = new ClassModel(text);
		
		AppProcess.CurrentSession.AddModel(classModel);
		
		var recordModel = classModel.ToRecord();
		File.WriteAllText(path, recordModel, Encoding.UTF8);
	}

	private static void ProcessContexts(IEnumerable<string> paths)
	{
		var contexts = paths
						.Select(p => Regex.Match(p, @"^.*Context\.cs$").Value)
						.Where(e => e != string.Empty);

		foreach (var context in contexts)
			RewriteContext(context);
	}

	private static IEnumerable<string> FindAllClasses(string path)
	{
		foreach (var file in Directory.EnumerateFiles(path))
			yield return file;

		foreach (var directory in Directory.EnumerateDirectories(path))
		{
			foreach (var file in FindAllClasses(directory))
				yield return file;
		}
	}

	private static void RewriteContext(string context)
	{
		var text = File.ReadAllText(context);
		var classContext = new ClassContext(text);
		AppProcess.CurrentSession.AddContext(classContext);
		
		var perfectLook = classContext.ToPerfectLook();

		File.WriteAllText(context, perfectLook, Encoding.UTF8);

		var contextRoot = Regex.Match(context, @"^.*\\").Value;

		CreateConfigurations(contextRoot, classContext.Configurations);
	}

	private static void CreateConfigurations(string contextRoot, List<ClassConfiguration> configurations)
	{
		foreach (var configuration in configurations)
		{
			Directory.CreateDirectory($@"{contextRoot}\Configurations\");
			var path = $@"{contextRoot}\Configurations\{configuration.ClassConfigName}.cs";
			File.WriteAllText(path, configuration.ToString(), Encoding.UTF8);
		}
	}
}