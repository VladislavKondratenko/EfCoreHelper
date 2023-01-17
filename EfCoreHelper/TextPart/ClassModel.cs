using System.Text;
using System.Text.RegularExpressions;
using EfCoreHelper.App;

namespace EfCoreHelper.TextPart;

internal class ClassModel
{
	private readonly StringBuilder _stringBuilder;
	private readonly string _text;

	public ClassModel(string text)
	{
		_text = text;
		_stringBuilder = new StringBuilder(text);
	}
	
	public string GetNamespace()
	{
		var namespaceRegex = new Regex(@"namespace\s+([\w\.]+)\s*(\{|;)");
		var match = namespaceRegex.Match(_text);
		
		return match.Success ? match.Groups[1].Value : string.Empty;
	}

	public string ToRecord()
	{
		var ctor = Regex.Match(_text, @"public.\w*?\((\r\n|.)*?}").Value;

		if (string.IsNullOrEmpty(ctor) is not true)
			_stringBuilder.Remove(ctor);

		return _stringBuilder
				.Replace("partial class", "record")
				.ToString();
	}
}