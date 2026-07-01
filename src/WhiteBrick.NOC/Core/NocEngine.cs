using WhiteBrick.NOC.Core.Scene;
using WhiteBrick.NOC.Rendering;
using WhiteBrick.NOC.Services;

namespace WhiteBrick.NOC.Core;

public sealed class NocEngine
{
    public NocEngine(SceneGraph sceneGraph, DisplayZoneManager displayZoneManager, RenderScheduler renderScheduler, AssetManager assets, ThemeManager themes)
    {
        SceneGraph = sceneGraph;
        DisplayZoneManager = displayZoneManager;
        RenderScheduler = renderScheduler;
        Assets = assets;
        Themes = themes;
    }

    public SceneGraph SceneGraph { get; }
    public DisplayZoneManager DisplayZoneManager { get; }
    public RenderScheduler RenderScheduler { get; }
    public AssetManager Assets { get; }
    public ThemeManager Themes { get; }

    public string EngineVersion => "2.5.002";
    public int SceneNodeCount => SceneGraph.Nodes.Count;
    public int VisibleSceneNodeCount => SceneGraph.VisibleNodeCount;
    public int DisplayZoneCount => DisplayZoneManager.Zones.Count;

    public RenderContext Tick() => RenderScheduler.Tick();

    public static NocEngine CreateDefault()
    {
        var sceneGraph = SceneGraph.CreateDefaultWallScene();
        var displayZones = new DisplayZoneManager();
        var renderScheduler = new RenderScheduler(sceneGraph, displayZones);
        var assets = AssetManager.CreateDefault();
        var themes = new ThemeManager();
        themes.UseCinematicDark();
        return new NocEngine(sceneGraph, displayZones, renderScheduler, assets, themes);
    }
}
