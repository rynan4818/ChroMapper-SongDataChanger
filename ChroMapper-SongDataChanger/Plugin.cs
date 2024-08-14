using UnityEngine;
using UnityEngine.SceneManagement;
using ChroMapper_SongDataChanger.Component;
using ChroMapper_SongDataChanger.UserInterface;

namespace ChroMapper_SongDataChanger
{
    [Plugin("Song Data Changer")]
    public class Plugin
    {
        public static SongDataController songDataController;
        public static UI ui;
        public static MenuUI menuUI;

        [Init]
        private void Init()
        {
            ui = new GameObject("SongDataChangerUI").AddComponent<UI>();
            ui.Init("ChroMapper_SongDataChanger.Resources.Icon.png", "SongDataChanger");
            SceneManager.sceneLoaded += SceneLoaded;
            Debug.Log("SongDataChanger Plugin has loaded!");
        }
        [Exit]
        private void Exit()
        {
            Debug.Log("SongDataChanger Plugin has closed!");
        }
        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex != 3) // Mapper scene
                return;
            if (songDataController != null && songDataController.isActiveAndEnabled)
                return;
            songDataController = new GameObject("SongDataController").AddComponent<SongDataController>();
            menuUI = new GameObject("SongDataChanger Menu").AddComponent<MenuUI>();
            var mapEditorUI = UnityEngine.Object.FindObjectOfType<MapEditorUI>();
            menuUI.AddMenu(mapEditorUI.MainUIGroup[5]);
        }
    }
}
