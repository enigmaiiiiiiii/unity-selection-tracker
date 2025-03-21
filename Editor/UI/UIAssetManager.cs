using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Synaptafin.Editor.SelectionTracker {

  public class UIAssetManager : ScriptableObject {
    public StyleSheet globalStyle;
    public VisualTreeAsset preferenceTemplate;
    public VisualTreeAsset windowTemplate;
    public VisualTreeAsset entryTemplate;
    public VisualTreeAsset detailInfoTemplate;
  }
}
