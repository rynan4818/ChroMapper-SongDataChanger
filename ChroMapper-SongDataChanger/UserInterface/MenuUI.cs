using ChroMapper_SongDataChanger.Configuration;
using UnityEngine;

namespace ChroMapper_SongDataChanger.UserInterface
{
    public class MenuUI : MonoBehaviour
    {
        public GameObject _menu;
        public UIDropdown _dropdown;
        public void AnchoredPosSave()
        {
            Options.Instance.menuUIAnchoredPosX = this._menu.GetComponent<RectTransform>().anchoredPosition.x;
            Options.Instance.menuUIAnchoredPosY = this._menu.GetComponent<RectTransform>().anchoredPosition.y;
        }
        public void AddMenu(MapEditorUI mapEditorUI)
        {
            var ui = Plugin.ui;
            this._menu = ui.SetMenu(this.gameObject, mapEditorUI.MainUIGroup[5], AnchoredPosSave, 170, 100, Options.Instance.menuUIAnchoredPosX, Options.Instance.menuUIAnchoredPosY);
            this._dropdown = ui.AddDropdown(this._menu.transform, Plugin.songDataController.songFiles, Plugin.songDataController.defalutSongIndex, (change) =>
            {

            });
            ui.MoveTransform(this._dropdown.transform, 150, 20, 1, 1, -84, -35);
            var button = ui.AddButton(this._menu.transform, "Change", "Change", () =>
            {
                var dropdown = this._dropdown.Dropdown;
                StartCoroutine(Plugin.songDataController.LoadAudio(dropdown.options[dropdown.value].text));
            });
            ui.MoveTransform(button.transform, 90, 20, 1, 1, -87f, -68f);
            this._menu.SetActive(false);
            ui._extensionBtn.Click = () =>
            {
                this._menu.SetActive(!this._menu.activeSelf);
            };
        }
    }
}
