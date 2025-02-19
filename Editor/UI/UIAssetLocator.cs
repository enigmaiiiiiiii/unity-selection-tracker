using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Synaptafin.Editor.SelectionTracker {

  public class UIAssetLocator {

    public static Lazy<UIAssetLocator> instance = new(static () => new());
    public static UIAssetLocator Instance => instance.Value;

    private readonly UIAssetManager _uiAssetManager;

    public StyleSheet GlobalStyle => _uiAssetManager.globalStyle;
    public VisualTreeAsset PreferenceTemplate => _uiAssetManager.preferenceTemplate;
    public VisualTreeAsset TrackerTemplate => _uiAssetManager.trackerTemplate;
    public VisualTreeAsset EntryTemplate => _uiAssetManager.entryTemplate;
    public VisualTreeAsset DetailInfoTemplate => _uiAssetManager.detailInfoTemplate;

    private UIAssetLocator() {
      string filter = $"t:{typeof(UIAssetManager).FullName}";
      string guid = AssetDatabase.FindAssets(filter).FirstOrDefault();
      _uiAssetManager = AssetDatabase.LoadAssetAtPath<UIAssetManager>(AssetDatabase.GUIDToAssetPath(guid));
    }
  }
}
