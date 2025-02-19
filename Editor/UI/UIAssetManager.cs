using UnityEngine;
using UnityEngine.UIElements;

namespace Synaptafin.Editor.SelectionTracker {

  public class UIAssetManager : ScriptableObject {
    public StyleSheet globalStyle;
    public VisualTreeAsset preferenceTemplate;
    public VisualTreeAsset trackerTemplate;
    public VisualTreeAsset entryTemplate;
    public VisualTreeAsset detailInfoTemplate;
  }
}
