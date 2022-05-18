namespace EfCoreHelper.App;

internal static class AppProcess
{
	private static readonly List<Session> _sessions = new();

	public static Session CurrentSession => _sessions.Last();

	public static void NewSession()
	{
		_sessions.Add(new Session());
	}
}