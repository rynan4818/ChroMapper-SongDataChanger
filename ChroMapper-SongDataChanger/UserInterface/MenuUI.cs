using SFB;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using ChroMapper_SongDataChanger.Configuration;
using ChroMapper_SongDataChanger.Controller;

namespace ChroMapper_SongDataChanger.UserInterface
{
    public class MenuUI : MonoBehaviour
    {
        public GameObject _menu;
        public UIDropdown _dropdown;
        public UITextInput _offsetInput;
        public TextMeshProUGUI _batachLable;
        public TextMeshProUGUI _bpmLable;
        public TextMeshProUGUI _beatLable;
        public decimal _realOffset;
        public bool _isOffsetChangeActive;
        public decimal _offset
        {
            get
            {
                return this._realOffset;
            }
            set
            {
                this._realOffset = value;
                this._isOffsetChangeActive = true;
                this._offsetInput.InputField.text = value.ToString("F3");
                this._isOffsetChangeActive = false;
            }
        }
        public void Start()
        {
            BatchRunController.Instance.OnBatchFinished += OnBatchRunFinished;
            this._offset = (decimal)BeatSaberSongContainer.Instance.Song.SongTimeOffset;
        }
        public void OnDestory()
        {
            BatchRunController.Instance.OnBatchFinished -= OnBatchRunFinished;
            Options.Instance.SettingSave();
        }
        public void OnBatchRunFinished()
        {
            this.SongListReload();
        }
        public void SongListReload()
        {
            var nowSong = this._dropdown.Dropdown.options[this._dropdown.Dropdown.value].text;
            Plugin.songDataController.SongFilesUpdate();
            this._dropdown.SetOptions(Plugin.songDataController.songFiles);
            var index = Plugin.songDataController.songFiles.IndexOf(nowSong);
            if (index < 0)
                this._dropdown.Dropdown.SetValueWithoutNotify(Plugin.songDataController.defalutSongIndex);
            else
                this._dropdown.Dropdown.SetValueWithoutNotify(index);
        }
        public void AnchoredPosSave()
        {
            Options.Instance.menuUIAnchoredPosX = this._menu.GetComponent<RectTransform>().anchoredPosition.x;
            Options.Instance.menuUIAnchoredPosY = this._menu.GetComponent<RectTransform>().anchoredPosition.y;
        }
        public void AddMenu(MapEditorUI mapEditorUI)
        {
            var ui = Plugin.ui;
            // メニューパネル設定
            this._menu = ui.SetMenu(this.gameObject, mapEditorUI.MainUIGroup[5], AnchoredPosSave, 250, 190, Options.Instance.menuUIAnchoredPosX, Options.Instance.menuUIAnchoredPosY);
            // タイトル
            ui.AddLabel(this._menu.transform, "SongDataChanger", "Song Data Changer", 150, 24, 1, 1, -165, -19.3f, TextAlignmentOptions.Center, 16);
            // 曲データリスト更新ボタン
            ui.AddButton(this._menu.transform, "SongListReload", "Song List Reload", 10, 70, 20, 1, 1, -44.6f, -23.3f, () =>
            {
                this.SongListReload();
            });
            // 曲データ選択ドロップダウン
            this._dropdown = ui.AddDropdown(this._menu.transform, Plugin.songDataController.songFiles, Plugin.songDataController.defalutSongIndex, 230, 20, 1, 1, -124.3f, -46.7f, (change) => {});
            // オフセット値入力
            ui.AddLabel(this._menu.transform, "Offset", "Offset", 30, 24, 1, 1, -224.3f, -72.6f, TextAlignmentOptions.Right, 12);
            this._offsetInput = ui.AddTextInput(this._menu.transform, "Offset", "", TextAlignmentOptions.Right, 10, 55, 20, 1, 1, -179.8f, -72.1f, (value) =>
            {
                if (this._isOffsetChangeActive)
                    return;
                float offset;
                if (float.TryParse(this._offsetInput.InputField.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out offset))
                    this._realOffset = (decimal)offset;
            });
            ui.AddLabel(this._menu.transform, "sec", "sec", 30, 24, 1, 1, -134.5f, -72.1f, TextAlignmentOptions.Left, 12);
            // 変更ボタン
            ui.AddButton(this._menu.transform, "Change", "Change", 12, 50, 20, 1, 1, -35.3f, -72.1f, () =>
            {
                var dropdown = this._dropdown.Dropdown;
                StartCoroutine(Plugin.songDataController.LoadAudio(dropdown.options[dropdown.value].text, (float)this._offset));
            });
            // -10msボタン
            ui.AddButton(this._menu.transform, "Minus10ms", "-10ms", 10, 30, 20, 1, 1, -223.9f, -96.6f, () =>
            {
                this._offset -= 0.01m;
            });
            // -1msボタン
            ui.AddButton(this._menu.transform, "Minus1ms", "-1ms", 10, 30, 20, 1, 1, -192.5f, -96.6f, () =>
            {
                this._offset -= 0.001m;
            });
            // 0ボタン
            ui.AddButton(this._menu.transform, "Zero", "0", 10, 30, 20, 1, 1, -161.1f, -96.6f, () =>
            {
                this._offset = 0;
            });
            // +1msボタン
            ui.AddButton(this._menu.transform, "Plus1ms", "+1ms", 10, 30, 20, 1, 1, -129.5f, -96.6f, () =>
            {
                this._offset += 0.001m;
            });
            // +10msボタン
            ui.AddButton(this._menu.transform, "Plus10ms", "+10ms", 10, 32, 20, 1, 1, -96.5f, -96.6f, () =>
            {
                this._offset += 0.01m;
            });
            // -1beatボタン
            ui.AddButton(this._menu.transform, "Minus1beat", "-1 beat", 10, 35, 20, 1, 1, -221.1f, -119.1f, () =>
            {
                this._offset -= 60m / (decimal)BeatSaberSongContainer.Instance.Song.BeatsPerMinute;
            });
            // -0.5beatボタン
            ui.AddButton(this._menu.transform, "MinusHalfBeat", "-0.5 beat", 10, 42, 20, 1, 1, -181.2f, -119.1f, () =>
            {
                this._offset -= 30m / (decimal)BeatSaberSongContainer.Instance.Song.BeatsPerMinute;
            });
            // +0.5beatボタン
            ui.AddButton(this._menu.transform, "PlusHalfBeat", "+0.5 beat", 10, 42, 20, 1, 1, -137.4f, -119.1f, () =>
            {
                this._offset += 30m / (decimal)BeatSaberSongContainer.Instance.Song.BeatsPerMinute;
            });
            // +1beatボタン
            ui.AddButton(this._menu.transform, "Plus1beat", "+1 beat", 10, 35, 20, 1, 1, -97.6f, -119.1f, () =>
            {
                this._offset += 60m / (decimal)BeatSaberSongContainer.Instance.Song.BeatsPerMinute;
            });
            // リセットボタン
            ui.AddButton(this._menu.transform, "DefaultReset", "Default Reset", 12, 65, 20, 1, 1, -42.5f, -96.6f, () =>
            {
                this._offset = (decimal)BeatSaberSongContainer.Instance.Song.SongTimeOffset;
                this._dropdown.Dropdown.SetValueWithoutNotify(Plugin.songDataController.defalutSongIndex);
            });
            // BPM情報ラベル
            this._bpmLable = ui.AddLabel(this._menu.transform, "BPM", $"{BeatSaberSongContainer.Instance.Song.BeatsPerMinute} BPM", 70, 12, 1, 1, -43.6f, -112.88f, TextAlignmentOptions.Left, 10).Item2;
            this._beatLable = ui.AddLabel(this._menu.transform, "Beat", $"1 Beat={(60m / (decimal)BeatSaberSongContainer.Instance.Song.BeatsPerMinute).ToString("F5")}sec", 70, 12, 1, 1, -43.6f, -123.1f, TextAlignmentOptions.Left, 10).Item2;
            // 実行バッチラベル
            this._batachLable = ui.AddLabel(this._menu.transform, "BatchFile", Path.GetFileName(Options.Instance.batachFilePath), 230, 16, 1, 1, -125.3f, -148.2f, TextAlignmentOptions.Left, 12).Item2;
            // バッチ実行ボタン
            ui.AddButton(this._menu.transform, "BatchFileRUN", $"{Options.Instance.batchUITitle} File RUN", 10, 70, 20, 1, 1, -204.7f, -170f, () =>
            {
                if (!BatchRunController.Instance.IsRunning)
                    StartCoroutine(BatchRunController.Instance.BatchRun());
            });
            // バッチファイル選択ボタン
            ui.AddButton(this._menu.transform, "BatchFileSelect", $"{Options.Instance.batchUITitle} File Select", 10, 70, 20, 1, 1, -128.9f, -170f, () =>
            {
                var ext = new ExtensionFilter[] { new ExtensionFilter($"{Options.Instance.batchUITitle} File", new string[] {Options.Instance.batchExtension}), new ExtensionFilter("All File", new string[] { "*" }) };
                var directory = Environment.CurrentDirectory;
                if (File.Exists(Options.Instance.batachFilePath))
                    directory = Path.GetDirectoryName(Options.Instance.batachFilePath);
                if (!Directory.Exists(directory))
                    directory = Environment.CurrentDirectory;
                var paths = StandaloneFileBrowser.OpenFilePanel($"{Options.Instance.batchUITitle} File", directory, ext, false);
                if (paths.Length > 0 && File.Exists(paths[0]))
                {
                    this._batachLable.text = Path.GetFileName(paths[0]);
                    Options.Instance.batachFilePath = paths[0];
                    Options.Instance.SettingSave();
                }
            });
            // 閉じるボタン
            ui.AddButton(this._menu.transform, "Close", "Close", 12, 40, 20, 1, 1, -29.8f, -170f, () =>
            {
                this._menu.SetActive(false);
                Options.Instance.SettingSave();
            });

            this._menu.SetActive(false);
            ui._extensionBtn.Click = () =>
            {
                this._menu.SetActive(!this._menu.activeSelf);
                Options.Instance.SettingSave();
            };
        }
    }
}
