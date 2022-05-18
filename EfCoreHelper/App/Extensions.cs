using System.Text;

namespace EfCoreHelper.App;

public static class Extensions
{
	public static string Remove(this string str, string remove)
	{
		return str.Replace(remove, string.Empty);
	}
	
	public static StringBuilder Remove(this StringBuilder sb, string remove)
	{
		return sb.Replace(remove, string.Empty);
	}
}