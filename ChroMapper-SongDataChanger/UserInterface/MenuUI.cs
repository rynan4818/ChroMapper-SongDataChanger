using ChroMapper_SongDataChanger.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChroMapper_SongDataChanger.UserInterface
{
    public class MenuUI : MonoBehaviour
    {
        public GameObject _menu;
        public void AnchoredPosSave()
        {
            Options.Instance.menuUIAnchoredPosX = _menu.GetComponent<RectTransform>().anchoredPosition.x;
            Options.Instance.menuUIAnchoredPosY = _menu.GetComponent<RectTransform>().anchoredPosition.y;
        }
        public void AddMenu(CanvasGroup topBarCanvas)
        {
            this._menu = Plugin.ui.SetMenu(this.gameObject, topBarCanvas, AnchoredPosSave, 170, 315, Options.Instance.menuUIAnchoredPosX, Options.Instance.menuUIAnchoredPosY);
            this._menu.SetActive(false);
            Plugin.ui._extensionBtn.Click = () =>
            {
                this._menu.SetActive(!this._menu.activeSelf);
            };
        }
    }
}
