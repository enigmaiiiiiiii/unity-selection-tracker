using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synaptafin.Editor.SelectionTracker {

  /// <summary>
  /// Represents a entry of gameobject in prefab stage
  /// </summary>
  [Serializable]
  public class PrefabContentEntry : GameObjectEntry {

    [SerializeField] private string _cachedPrefabAssetPath;

    public override string DisplayName {
      get {
        string extName = string.IsNullOrEmpty(_cachedSceneName)
          ? _cachedName
          : string.Concat(_cachedSceneName, "/", _cachedName);

        return RefState.HasFlag(RefState.Destroyed)
          ? $"<s>{extName}</s>"
          : extName;
      }
    }

    public override RefState RefState {
      get {
        if (_cachedRef == null) {
          TryRestoreAndCache();
        }

        if (AssetDatabase.LoadAssetAtPath<Object>(_cachedPrefabAssetPath) == null) {
          return RefState.Deleted;
        }

        return _cachedRef == null && !_cachedScene.isLoaded
          ? RefState.Unstaged
          : RefState.Staged;
      }
    }

    public PrefabContentEntry(GameObject obj, GlobalObjectId id, PrefabStage prefabStage) : base(obj, id) {
      CacheGameObjectRefInfo(obj);
      CachePrefabContentRefInfo(prefabStage);
    }

    public override void Open() {

      if (RefState.HasFlag(RefState.Loaded)) {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
          PrefabStageUtility.OpenPrefab(_cachedPrefabAssetPath);
          Selection.activeObject = Ref;
          return;
        }
      }

      if (RefState.HasFlag(RefState.Unloaded)) {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
          PrefabStageUtility.OpenPrefab(_cachedPrefabAssetPath);
          Selection.activeObject = Ref;
          return;
        }
      }

      if (RefState.HasFlag(RefState.Loaded)) {
        Ping();
      }
    }

    public override bool Equals(Entry other) {

      if (other.Ref != null && Ref != null) {
        return other.Ref == Ref;
      }

      return other is PrefabContentEntry prefabContentEntry
        && _cachedPrefabAssetPath == prefabContentEntry._cachedPrefabAssetPath
        && DisplayName == prefabContentEntry.DisplayName;
    }

    private void CachePrefabContentRefInfo(PrefabStage prefabStage) {
      // _cachedPrefabStage = prefabStage;
      // _cachedPrefabStageName = prefabStage.name;
      _cachedPrefabAssetPath = prefabStage.assetPath;

      _cachedScene = prefabStage.scene;
      _cachedScenePath = prefabStage.scene.path;
    }
  }
}
