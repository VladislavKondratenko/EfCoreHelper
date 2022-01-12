using System.Text;
using System.Text.RegularExpressions;

namespace EfCoreHelper.TextPart;

public class ClassContext
{
	private readonly string _source;
	private readonly StringBuilder _result;
	private string _connectionParams = null!;
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
		ClearFooter();

		return _result
				.Replace("partial class", "class")
			.ToString();
	}

	private void ClearFooter()
	{
		_result.Replace(@"

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);", string.Empty);
	}

	private void WorkOnMethodConfiguration()
	{
		_connectionParams = Regex.Match(_source, @"optionsBuilder\.Use.*?\);").Value;
		var method = Regex.Match(_source, @"protected.override.void.OnConfiguring(\r\n|.)*?}\r\n.*?}").Value;
		_result.Replace(method, string.Empty);
	}

	private void WorkOnModelCreating()
	{
		var creator = new ConfigurationCreator(_source);
		_configurations = creator.ExtractClassConfigurations().ToList();
		
		foreach (var config in _configurations)
			_result.Replace(config.SourceModule + "\r\n", $"modelBuilder.ApplyConfiguration(new {config.ClassConfigName}());");

		_result.Replace(@"
            OnModelCreatingPartial(modelBuilder);", string.Empty);
	}
}