using System.Text;
using System.Text.RegularExpressions;
using EfCoreHelper.App;

namespace EfCoreHelper.TextPart;

public class ClassContext
{
	private readonly string _source;
	private readonly StringBuilder _result;
	private List<ClassConfiguration> _configurations = null!;

	public List<ClassConfiguration> Configurations => _configurations;

	public ClassContext(string source)
	{
		_source = source;
		_result = new StringBuilder(_source);
	}

	public string ToPerfectLook()
	{
		WorkOnModelCreating();
		WorkOnMethodConfiguration();
		AddConfigurationsUsing();
		ClearFooter();

		return _result
				.Replace("partial class", "class")
				.ToString();
	}

	private void AddConfigurationsUsing()
	{
		var ns = AppProcess.CurrentSession.Configurations.First().Namespace;
		var usingString = $"using {ns.Remove("namespace")}";
		_result.Insert(0, usingString);
	}

	private void ClearFooter()
	{
		const string oldCase = @"

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);";
		
		const string newCase = @"

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);";

		_result.Remove(oldCase);
		_result.Remove(newCase);
	}

	private void WorkOnMethodConfiguration()
	{
		var methodWithBody = Regex.Match(_source, @"protected.override.void.OnConfiguring(\r\n|.)*?}\r\n.*?}");

		if (methodWithBody.Success)
			_result.Remove(methodWithBody.Value);

		var methodWithLambda =
			Regex.Match(_source, @"protected.override.void.OnConfiguring(\r\n|.)*?\r\n.*?=>(\r\n|.)*?\);");

		if (methodWithLambda.Success)
			_result.Remove(methodWithLambda.Value);
	}

	private void WorkOnModelCreating()
	{
		var creator = new ConfigurationCreator(_source);
		_configurations = creator.ExtractClassConfigurations().ToList();

		foreach (var config in _configurations)
		{
			_result.Replace($"{config.SourceModule}\r\n",
				$"modelBuilder.ApplyConfiguration(new {config.ClassConfigName}());");
		}

		RemoveCallOnModelCreating();
	}

	private void RemoveCallOnModelCreating()
	{
		const string oldCase = @"
            OnModelCreatingPartial(modelBuilder);";
		
		const string newCase = @"
        OnModelCreatingPartial(modelBuilder);";

		_result.Remove(oldCase);
		_result.Remove(newCase);
	}
}