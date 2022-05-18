using EfCoreHelper.TextPart;

namespace EfCoreHelper.App;

internal class Session
{
	private readonly List<ClassModel> _models = new();
	private readonly List<ClassContext> _contexts = new();
	private readonly List<ClassConfiguration> _configurations = new();

	public List<ClassModel> Models => _models;
	public List<ClassContext> Contexts => _contexts;
	public List<ClassConfiguration> Configurations => _configurations;

	public void AddModel(ClassModel model)
	{
		_models.Add(model);
	}

	public void AddContext(ClassContext context)
	{
		_contexts.Add(context);
	}

	public void AddConfiguration(ClassConfiguration configuration)
	{
		_configurations.Add(configuration);
	}
}