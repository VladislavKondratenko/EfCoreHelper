using System.Text;
using System.Text.RegularExpressions;

namespace EfCoreHelper.TextPart;

public class ClassConfiguration
{
	private const string StandardUsing = @"using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;";

	private const string ClassHeaderTemplate = @"public class {Name}Configuration : IEntityTypeConfiguration<{Name}>
{
	public void Configure(EntityTypeBuilder<{Name}> builder)
	{
";

	private const string Footer = @"	}
}";

	private readonly string _module;

	private string _ns = null!;
	private string _classHeader = null!;
	private string _body = null!;
	private string _modelName = null!;

	public string ClassConfigName { get; set; } = null!;
	public string SourceModule => _module;
	public string ModelName => _modelName;

	public ClassConfiguration(string module, string @namespace)
	{
		_module = module;
		SetName();
		SetBody();
		SetNamespace(@namespace);
	}

	public override string ToString()
	{
		return new StringBuilder(StandardUsing)
				.Append("\n\n")
				.Append(_ns)
				.Append('\n')
				.Append(_classHeader)
				.Append(_body)
				.Append(Footer)
				.ToString();
	}

	private void SetName()
	{
		_modelName = Regex.Match(_module, @"<\w+>")
						.Value
						.Replace("<", string.Empty)
						.Replace(">", string.Empty);

		_classHeader = ClassHeaderTemplate.Replace("{Name}", _modelName);
		ClassConfigName = _modelName + "Configuration";
	}

	private void SetBody()
	{
		_body = Regex.Match(_module, @"{(.|\n)*?}")
					.Value
					.Replace("{", string.Empty)
					.Replace("}", string.Empty)
					.Replace("entity.", "builder.");
	}

	private void SetNamespace(string ns)
	{
		_ns = new StringBuilder(ns)
			.Replace("\n", string.Empty)
			.Replace("\r", string.Empty)
			.Append(';')
			.Append('\n')
			.ToString();
	}
}