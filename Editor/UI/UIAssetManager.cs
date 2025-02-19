using UnityEngine;
using UnityEngine.UIElements;

namespace Synaptafin.Editor.SelectionTracker {

  [CreateAssetMenu(fileName = "UIAssetManager", menuName = "SO/UIAssetManager")]
  public class UIAssetManager : ScriptableObject {

    public StyleSheet globalStyle;
    public VisualTreeAsset preferenceTemplate;
    public VisualTreeAsset TrackerTemplate;
    public VisualTreeAsset entryTemplate;
    public VisualTreeAsset detailInfoTemplate;
  }
}
