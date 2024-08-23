using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;
using ChroMapper_SongDataChanger.Configuration;

namespace ChroMapper_SongDataChanger.Component
{
    public class DragWindowController : MonoBehaviour, IDragHandler
    {
        public Canvas _canvas { set; get; }
        public InputAction _dragEnableAction;
        public bool _dragWindowKeyEnable;
        public event Action OnDragWindow;

        public void Start()
        {
            this._dragEnableAction = new InputAction("DragEnableKey", binding: Options.Instance.dragEnableBinding);
            this._dragEnableAction.started += OnDragEnableKey;
            this._dragEnableAction.performed += OnDragEnableKey;
            this._dragEnableAction.canceled += OnDragEnableKey;
            this._dragEnableAction.Enable();
        }
        public void OnDestroy()
        {
            // SetMenuした直後にSetActive(false)しているので、メニューがActiveにならないとStartが呼ばれずにOnDestroyする
            // その場合_dragEnableActionがnullなのでnullチェックをする
            this._dragEnableAction?.Dispose();
        }
        public void OnDragEnableKey(InputAction.CallbackContext context)
        {
            this._dragWindowKeyEnable = context.ReadValueAsButton();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (this._dragWindowKeyEnable)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / this._canvas.scaleFactor;
                this.OnDragWindow?.Invoke();
            }
        }
    }
}
