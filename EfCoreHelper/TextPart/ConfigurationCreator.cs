using System.Text.RegularExpressions;

namespace EfCoreHelper.TextPart;

public class ConfigurationCreator
{
	private readonly string _source;
	private const string ModulePattern = @"modelBuilder\.Entity<(.|\n)*?\n\s{12}}\);";

	public ConfigurationCreator(string source)
	{
		_source = source;
	}

	public IEnumerable<ClassConfiguration> ExtractClassConfigurations()
	{
		var modules = ExtractModules();

		return modules.Select(ConvertToClassConfiguration);
	}

	private ClassConfiguration ConvertToClassConfiguration(string module)
	{
		var ns = Regex.Match(_source, @"namespace.*?(\n|;)").Value;

		return new ClassConfiguration(module, ns);
	}

	private IEnumerable<string> ExtractModules()
	{
		return Regex.Matches(_source, ModulePattern)
					.Select(m => m.Value)
					.ToArray();
	}
}