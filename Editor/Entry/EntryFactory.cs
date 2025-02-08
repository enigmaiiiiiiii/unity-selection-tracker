using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Synaptafin.Editor.SelectionTracker {
  public static class EntryFactory {
    public static Entry Create(Object obj) {
      GlobalObjectId id = GlobalObjectId.GetGlobalObjectIdSlow(obj);
      if (obj is GameObject go) {

        // prefab instance is treated as gameobject
        if (id.identifierType == 2) {
          return new GameObjectEntry(go, id);
        }

        // prefab content is gameobject in prefab edit mode
        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null) {
          return new PrefabContentEntry(go, id, prefabStage);
        }
      }

      if (id.identifierType is 1 or 3) {
        return new NormalAssetEntry(obj, id);
      }
      return null;
    }
  }
}

