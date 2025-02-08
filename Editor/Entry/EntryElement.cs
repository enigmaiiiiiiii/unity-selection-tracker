using System;
using Synaptafin.Editor.SelectionTracker;
using UnityEditor;
using static Synaptafin.Editor.SelectionTracker.Constants;
using static Synaptafin.Editor.SelectionTracker.UnityBuiltInIcons;

namespace UnityEngine.UIElements {

  [UxmlElement("Entry")]
  public partial class EntryElement : VisualElement {

    private readonly VisualElement _entryRoot;
    private readonly IEntryService _entryService;
    private readonly Label _entryText;
    private readonly Image _entryIcon;
    private readonly Image _pingIcon;
    private readonly Image _openIcon;
    private readonly Image _favoriteIcon;
    private readonly VisualElement _entryPopupRoot;
    private bool _isFavorite = false;

    public int Index { get; set; }
    public string EntryLabel => _entryText.text;

    private Entry _entry;
    public Entry Entry {
      get => _entry;
      set => SetupEntry(value);
    }

    public EntryElement() {
      _entryRoot = UIAssetManager.instance.entryTemplate.Instantiate();
      this.AddManipulator(new ContextualMenuManipulator(evt => {
        evt.menu.AppendAction("Remove", _ => _entryService.RemoveEntry(Entry), DropdownMenuAction.AlwaysEnabled);
        evt.StopPropagation();
      }));

      VisualElement info = _entryRoot.Q<VisualElement>("Info");
      info?.AddManipulator(new DragAndLeftClickManipulator(this));
      // info?.AddManipulator(new OnHoverManipulator(this));

      RegisterCallback<AttachToPanelEvent>(evt => {
        PreferencePersistence.instance.onUpdated += PreferenceUpdatedCallback;
      });
      RegisterCallback<DetachFromPanelEvent>(evt => {
        PreferencePersistence.instance.onUpdated -= PreferenceUpdatedCallback;
      });

      _entryText = _entryRoot.Q<Label>("Name");
      _entryIcon = _entryRoot.Q<Image>("Icon");
      _pingIcon = _entryRoot.Q<Image>("PingIcon");
      _openIcon = _entryRoot.Q<Image>("OpenPrefabIcon");
      _favoriteIcon = _entryRoot.Q<Image>("FavoriteIcon");
      _entryPopupRoot = _entryRoot.Q<VisualElement>("PopupDetail");

      if (_pingIcon != null) {
        _pingIcon.image = EditorGUIUtility.IconContent(SEARCH_ICON_NAME).image;
        _pingIcon.RegisterCallback<MouseUpEvent>(PingIconMouseUpCallback);
      }

      if (_openIcon != null) {
        _openIcon.image = EditorGUIUtility.IconContent(OPEN_ASSET_ICON_NAME).image;
        _openIcon.RegisterCallback<MouseUpEvent>(OpenIconMouseUpCallback);
      }

      if (_favoriteIcon != null) {
        _favoriteIcon.image = EditorGUIUtility.IconContent(FAVORITE_EMPTY_ICON_NAME).image;
        _favoriteIcon?.RegisterCallback<MouseUpEvent>(FavoriteIconCallback);
      }

      Add(_entryRoot);
    }

    public EntryElement(int index, IEntryService service) : this() {
      Index = index;
      _entryService = service;
    }

    public void Reset() {
      Entry = null;
    }

    public IEntryService GetEntryService() {
      return _entryService;
    }

    public void PopupDetail() {
      _entryPopupRoot.style.display = DisplayStyle.Flex;
    }

    public void HideDetail() {
      _entryPopupRoot.style.display = DisplayStyle.None;
    }

    private void FavoriteIconCallback(MouseUpEvent evt) {
      if (Entry == null) {
        return;
      }

      _isFavorite = !_isFavorite;
      EntryServicePersistence.instance.RecordFavorites(Entry, _isFavorite);
    }

    private void SetupEntry(Entry value) {
      if (value == null) {
        style.display = DisplayStyle.None;
        _entry?.onFavoriteChanged.RemoveListener(FavoriteChangedCallback);
        _entry = null;
        _entryText.text = string.Empty;
        _entryIcon.image = null;
        return;
      }

      _entry?.onFavoriteChanged.RemoveListener(FavoriteChangedCallback);
      _entry = value;
      _entry.onFavoriteChanged.AddListener(FavoriteChangedCallback);

      if (_entryText != null) {
        SetNameLabel(value);
      }

      if (_entryIcon != null) {
        _entryIcon.image = value.Icon;
      }

      if (_openIcon != null) {
        // Debug.Log($"{Entry.DisplayName} - {Enum.GetName(typeof(RefState), Entry.RefState)}");
        _openIcon.style.display = value.RefState.HasFlag(RefState.GameObject)
          ? DisplayStyle.None
          : DisplayStyle.Flex;
      }

      if (_favoriteIcon != null) {
        _isFavorite = value.IsFavorite;
        _favoriteIcon.image = _isFavorite
          ? EditorGUIUtility.IconContent(FAVORITE_ICON_NAME).image
          : EditorGUIUtility.IconContent(FAVORITE_EMPTY_ICON_NAME).image;
      }
    }

    private void SetNameLabel(Entry value) {
      if (Entry == null) {
        return;
      }
      _entryText.text = value.DisplayName;
      // Debug.Log($"entry type of {value.DisplayName} is {value.GetType()}");

      _entryText.style.color = Entry.RefState switch {
        RefState.Loaded => (StyleColor)SCENE_OBJECT_COLOR,
        RefState.Staged => (StyleColor)SCENE_OBJECT_COLOR,
        RefState.Unloaded => (StyleColor)Color.grey,
        RefState.Unstaged => (StyleColor)Color.grey,
        RefState.Destroyed => (StyleColor)DELETED_OR_DESTROYED_COLOR,
        RefState.Deleted => (StyleColor)DELETED_OR_DESTROYED_COLOR,
        _ => (StyleColor)Color.white,
      };
    }


    private void PingIconMouseUpCallback(MouseUpEvent evt) {
      if (Entry == null) {
        return;
      }

      Entry.Ping();
      evt.StopPropagation();
    }

    private void OpenIconMouseUpCallback(MouseUpEvent evt) {
      Entry.Open();
      evt.StopPropagation();
    }

    private void FavoriteChangedCallback(bool value) {
      _favoriteIcon.image = value
        ? EditorGUIUtility.IconContent(FAVORITE_ICON_NAME).image
        : EditorGUIUtility.IconContent(FAVORITE_EMPTY_ICON_NAME).image;
    }

    private void PreferenceUpdatedCallback() {
      _favoriteIcon.style.display = PreferencePersistence.instance.GetToggleValue(DRAW_FAVORITES_KEY)
        ? DisplayStyle.Flex
        : DisplayStyle.None;
    }

  }
}
