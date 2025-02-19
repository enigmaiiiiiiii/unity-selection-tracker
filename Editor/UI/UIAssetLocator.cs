using System;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor;

namespace Synaptafin.Editor.SelectionTracker {

  public class UIAssetLocator {

    public static Lazy<UIAssetLocator> instance = new(static () => new());
    public static UIAssetLocator Instance => instance.Value;

    private readonly UIAssetManager _uiAssetManager;

    public StyleSheet GlobalStyle => _uiAssetManager.globalStyle;
    public VisualTreeAsset PreferenceTemplate => _uiAssetManager.preferenceTemplate;
    public VisualTreeAsset TrackerTemplate => _uiAssetManager.TrackerTemplate;
    public VisualTreeAsset EntryTemplate => _uiAssetManager.entryTemplate;
    public VisualTreeAsset DetailInfoTemplate => _uiAssetManager.detailInfoTemplate;

    private UIAssetLocator() {
      string filter = $"t:{nameof(UIAssetManager)}";
      string guid = AssetDatabase.FindAssets(filter).FirstOrDefault();
      _uiAssetManager = AssetDatabase.LoadAssetAtPath<UIAssetManager>(AssetDatabase.GUIDToAssetPath(guid));
    }
  }
}
