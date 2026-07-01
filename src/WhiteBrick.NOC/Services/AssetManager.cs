namespace WhiteBrick.NOC.Services;

public sealed class AssetManager
{
    private readonly HashSet<string> _registeredAssets = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string assetKey) => _registeredAssets.Add(assetKey);
    public IReadOnlyCollection<string> RegisteredAssets => _registeredAssets;

    public static AssetManager CreateDefault()
    {
        var manager = new AssetManager();
        manager.Register("theme.whitebrick.dark.cinematic");
        manager.Register("config.noc.settings");
        manager.Register("scene.wall.default");
        return manager;
    }
}
