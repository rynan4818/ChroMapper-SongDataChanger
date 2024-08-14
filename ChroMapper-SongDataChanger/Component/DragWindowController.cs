using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace ChroMapper_SongDataChanger.Component
{
    public class DragWindowController : MonoBehaviour, IDragHandler
    {
        public Canvas canvas { set; get; }
        public event Action onDragWindow;

        public void OnDrag(PointerEventData eventData)
        {
            if (Plugin.songDataController.dragWindowKeyEnable)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / this.canvas.scaleFactor;
                this.onDragWindow?.Invoke();
            }
        }
    }
}
