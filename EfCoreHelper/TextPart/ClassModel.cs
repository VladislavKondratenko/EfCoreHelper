using System.Text;
using System.Text.RegularExpressions;

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

	public string ToRecord()
	{
		var ctor = Regex.Match(_text, @"public.*?\((\r\n|.)*?}").Value;

		if (string.IsNullOrEmpty(ctor) is not true)
			_stringBuilder.Replace(ctor, string.Empty);

		return _stringBuilder
				.Replace("partial class", "record")
				.ToString();
	}
}