using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using ChroMapper_SongDataChanger.Component;
using TMPro;
using UnityEngine.Events;

namespace ChroMapper_SongDataChanger.UserInterface
{
    public class UI : MonoBehaviour
    {
        public readonly ExtensionButton _extensionBtn = new ExtensionButton();

        //TextInput時のChroMapperショートカットキー制御用
        public readonly Type[] _actionMapsEnabledWhenNodeEditing =
        {
            typeof(CMInput.ICameraActions), typeof(CMInput.IBeatmapObjectsActions),
            typeof(CMInput.ISavingActions), typeof(CMInput.ITimelineActions)
        };
        public Type[] _actionMapsDisabled => typeof(CMInput).GetNestedTypes()
            .Where(x => x.IsInterface && !this._actionMapsEnabledWhenNodeEditing.Contains(x)).ToArray();
        public List<Type> _queuedToDisable = new List<Type>();
        public List<Type> _queuedToEnable = new List<Type>();
        public void Init(string extButtonResource, string extButtonTooltip)
        {
            DontDestroyOnLoad(this.gameObject);
            //拡張ボタンの設定
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(extButtonResource);
            var data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            var texture2D = new Texture2D(256, 256);
            texture2D.LoadImage(data);
            this._extensionBtn.Icon = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0), 100.0f);
            this._extensionBtn.Tooltip = extButtonTooltip;
            ExtensionButtons.AddButton(this._extensionBtn);
        }
        public void Update()
        {
            this.QueuedActionMaps();
        }
        public void QueuedActionMaps()
        {
            if (this._queuedToDisable.Any())
            {
                CMInputCallbackInstaller.DisableActionMaps(typeof(UI), this._queuedToDisable.ToArray());
                this._queuedToDisable.Clear();
            }
            if (this._queuedToEnable.Any())
            {
                CMInputCallbackInstaller.ClearDisabledActionMaps(typeof(UI), this._queuedToEnable.ToArray());
                this._queuedToEnable.Clear();
            }
        }
        public void DisableAction(Type[] actionMaps)
        {
            foreach (Type actionMap in actionMaps)
            {
                this._queuedToEnable.Remove(actionMap);
                if (!this._queuedToDisable.Contains(actionMap))
                    this._queuedToDisable.Add(actionMap);
            }
        }
        public void EnableAction(Type[] actionMaps)
        {
            foreach (Type actionMap in actionMaps)
            {
                this._queuedToDisable.Remove(actionMap);
                if (!this._queuedToEnable.Contains(actionMap))
                    this._queuedToEnable.Add(actionMap);
            }
        }
        public UIButton AddButton(Transform parent, string name, string text, UnityAction onClick)
        {
            return this.AddButton(parent, name, text, 12, onClick);
        }
        public UIButton AddButton(Transform parent, string name, string text, float fontSize, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, UnityAction onClick, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var button = this.AddButton(parent, name, text, fontSize, onClick);
            this.MoveTransform(button.transform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return button;
        }
        public UIButton AddButton(Transform parent, string name, string text, float fontSize, UnityAction onClick)
        {
            var button = UnityEngine.Object.Instantiate(PersistentUI.Instance.ButtonPrefab, parent);
            button.name = name;
            button.Button.onClick.AddListener(onClick);
            button.SetText(text);
            button.Text.enableAutoSizing = false;
            button.Text.fontSize = fontSize;
            return button;
        }
        public (RectTransform, TextMeshProUGUI) AddLabel(Transform parent, string name, string text, Vector2 pos, float width = 110, float height = 24)
        {
            var label = this.AddLabel(parent, name, text);
            this.MoveTransform(label.Item1, width, height, 0.5f, 1, pos.x, pos.y);
            return label;
        }
        public (RectTransform, TextMeshProUGUI) AddLabel(Transform parent, string name, string text, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, TextAlignmentOptions alignment = TextAlignmentOptions.Center, float fontSize = 16, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var label = this.AddLabel(parent, name, text, alignment, fontSize);
            this.MoveTransform(label.Item1, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return label;
        }
        public (RectTransform, TextMeshProUGUI) AddLabel(Transform parent, string name, string text, TextAlignmentOptions alignment = TextAlignmentOptions.Center, float fontSize = 16)
        {
            var entryLabel = new GameObject(name + " Label", typeof(TextMeshProUGUI));
            var rectTransform = (RectTransform)entryLabel.transform;
            rectTransform.SetParent(parent);
            var textComponent = entryLabel.GetComponent<TextMeshProUGUI>();
            textComponent.name = name;
            textComponent.font = PersistentUI.Instance.ButtonPrefab.Text.font;
            textComponent.alignment = alignment;
            textComponent.fontSize = fontSize;
            textComponent.text = text;
            return (rectTransform, textComponent);
        }
        public (RectTransform, TextMeshProUGUI, UITextInput) AddTextInput(Transform parent, string title, string text, string value, UnityAction<string> onChange, string focusMove = null, int? roundDigits = null)
        {
            var label = this.AddLabel(parent, title, text, TextAlignmentOptions.Right, 12);
            return (label.Item1, label.Item2, this.AddTextInput(parent, title, value, TextAlignmentOptions.Left, 10, onChange, focusMove, roundDigits));
        }
        public UITextInput AddTextInput(Transform parent, string title, string value, TextAlignmentOptions alignment, float fontSize, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, UnityAction<string> onChange, string focusMove = null, int? roundDigits = null, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var textInput = this.AddTextInput(parent, title, value, alignment, fontSize, onChange, focusMove, roundDigits);
            this.MoveTransform(textInput.transform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return textInput;
        }
        public UITextInput AddTextInput(Transform parent, string title, string value, TextAlignmentOptions alignment, float fontSize, UnityAction<string> onChange, string focusMove = null, int? roundDigits = null)
        {
            var textInput = UnityEngine.Object.Instantiate(PersistentUI.Instance.TextInputPrefab, parent);
            textInput.GetComponent<Image>().pixelsPerUnitMultiplier = 3;
            textInput.name = title;
            textInput.InputField.text = value;
            textInput.InputField.onFocusSelectAll = false;
            textInput.InputField.textComponent.alignment = alignment;
            textInput.InputField.textComponent.fontSize = fontSize;
            textInput.InputField.onValueChanged.AddListener(onChange);
            textInput.InputField.onEndEdit.AddListener(delegate {
                this.EnableAction(this._actionMapsDisabled);
            });
            textInput.InputField.onSelect.AddListener(delegate {
                this.DisableAction(this._actionMapsDisabled);
            });
            return textInput;
        }
        public (RectTransform, TextMeshProUGUI, Toggle) AddCheckbox(Transform parent, string title, string text, Vector2 pos, bool value, UnityAction<bool> onClick)
        {
            var checkBox = this.AddCheckbox(parent, title, text, value, onClick);
            this.MoveTransform(checkBox.Item1, 80, 16, 0.5f, 1, pos.x + 10, pos.y + 5);
            this.MoveTransform(checkBox.Item3.transform, 100, 25, 0.5f, 1, pos.x, pos.y);
            return checkBox;
        }
        public (RectTransform, TextMeshProUGUI, Toggle) AddCheckbox(Transform parent, string title, string text, bool value, UnityAction<bool> onClick)
        {
            var label = this.AddLabel(parent, title, text, TextAlignmentOptions.Left, 12);
            return (label.Item1, label.Item2, this.AddCheckbox(parent, value, onClick));
        }
        public Toggle AddCheckbox(Transform parent, bool value, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, UnityAction<bool> onClick, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var checkbox = this.AddCheckbox(parent, value, onClick);
            this.MoveTransform(checkbox.transform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return checkbox;
        }
        public Toggle AddCheckbox(Transform parent, bool value, UnityAction<bool> onClick)
        {
            var original = GameObject.Find("Strobe Generator").GetComponentInChildren<Toggle>(true);
            var toggleObject = UnityEngine.Object.Instantiate(original, parent.transform);
            var toggleComponent = toggleObject.GetComponent<Toggle>();
            var colorBlock = toggleComponent.colors;
            colorBlock.normalColor = Color.white;
            toggleComponent.colors = colorBlock;
            toggleComponent.isOn = value;
            toggleComponent.onValueChanged.AddListener(onClick);
            return toggleComponent;
        }
        public UIDropdown AddDropdown(Transform parent, List<string> options, int value, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, UnityAction<int> onChange, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var dropdown = this.AddDropdown(parent, options, value, onChange);
            this.MoveTransform(dropdown.transform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return dropdown;
        }
        public UIDropdown AddDropdown(Transform parent, List<string> options, int value, UnityAction<int> onChange)
        {
            var dropdown = UnityEngine.Object.Instantiate(PersistentUI.Instance.DropdownPrefab, parent);
            dropdown.SetOptions(options);
            dropdown.Dropdown.onValueChanged.AddListener(onChange);
            dropdown.Dropdown.SetValueWithoutNotify(value);
            var image = dropdown.GetComponent<Image>();
            image.color = new Color(0.35f, 0.35f, 0.35f, 1f);
            image.pixelsPerUnitMultiplier = 1.5f;
            return dropdown;
        }
        public GameObject SetMenu(GameObject obj, CanvasGroup canvas, Action posSave, float sizeX, float sizeY, float anchorPosX, float anchorPosY, float anchorX = 1, float anchorY = 1, float pivotX = 1, float pivotY = 1)
        {
            this.SetMenu(obj, canvas, posSave);
            this.AttachTransform(obj, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            this.AttachImage(obj, new Color(0.24f, 0.24f, 0.24f));
            return obj;
        }
        public GameObject SetMenu(GameObject obj, CanvasGroup canvas, Action posSave)
        {
            obj.transform.parent = canvas.transform;
            var dragWindow = obj.AddComponent<DragWindowController>();
            dragWindow._canvas = canvas.GetComponent<Canvas>();
            dragWindow.OnDragWindow += posSave;
            return obj;
        }
        public void AttachImage(GameObject obj, Color color)
        {
            var imageSetting = obj.AddComponent<Image>();
            imageSetting.sprite = PersistentUI.Instance.Sprites.Background;
            imageSetting.type = Image.Type.Sliced;
            imageSetting.color = color;
        }
        public RectTransform AttachTransform(GameObject obj, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            var rectTransform = obj.AddComponent<RectTransform>();
            this.MoveTransform(rectTransform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
            return rectTransform;
        }
        public void MoveTransform(Transform transform, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if (!(transform is RectTransform rectTransform)) return;
            this.MoveTransform(rectTransform, sizeX, sizeY, anchorX, anchorY, anchorPosX, anchorPosY, pivotX, pivotY);
        }
        public void MoveTransform(RectTransform rectTransform, float sizeX, float sizeY, float anchorX, float anchorY, float anchorPosX, float anchorPosY, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
            rectTransform.pivot = new Vector2(pivotX, pivotY);
            rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(anchorX, anchorY);
            rectTransform.anchoredPosition = new Vector3(anchorPosX, anchorPosY, 0);
        }
    }
}
