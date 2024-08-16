using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using HarmonyLib;

namespace ChroMapper_SongDataChanger.Component
{
    public class SongDataController : MonoBehaviour
    {
        public List<string> songFiles { get; set; }
        public int defalutSongIndex { get; set; }
        public AudioTimeSyncController atsc { get; set; }
        public AudioManager audioManager { get; set; }
        public bool IsAudioLoading { get; set; } = false;
        public void Awake()
        {
            SongFilesUpdate();
        }
        public void Start()
        {
            this.atsc = UnityEngine.Object.FindObjectOfType<AudioTimeSyncController>();
            this.audioManager = UnityEngine.Object.FindObjectOfType<AudioManager>();
        }
        public void SongFilesUpdate()
        {
            this.songFiles = Directory.EnumerateFiles(BeatSaberSongContainer.Instance.Song.Directory, "*.*")
               .Where(e => e.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || e.EndsWith(".egg", StringComparison.OrdinalIgnoreCase) || e.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
               .ToList()
               .ConvertAll(s => Path.GetFileName(s));
            this.defalutSongIndex = this.songFiles.IndexOf(BeatSaberSongContainer.Instance.Song.SongFilename);
        }
        public IEnumerator LoadAudio(string songFile, float offset = 0)
        {
            if (this.IsAudioLoading) yield break;
            var song = BeatSaberSongContainer.Instance.Song;
            if (!Directory.Exists(song.Directory)) yield break;
            this.IsAudioLoading = true;
            var playing = this.atsc.IsPlaying;
            if (playing) this.atsc.TogglePlaying();
            var fullPath = Path.Combine(song.Directory, songFile);
            if (File.Exists(fullPath))
            {
                yield return song.LoadAudio((clip) =>
                {
                    BeatSaberSongContainer.Instance.LoadedSong = clip;
                    BeatSaberSongContainer.Instance.LoadedSongSamples = clip.samples;
                    BeatSaberSongContainer.Instance.LoadedSongFrequency = clip.frequency;
                    BeatSaberSongContainer.Instance.LoadedSongLength = clip.length;
                    Traverse.Create(this.atsc).Field("clip").SetValue(clip);
                    var waveformSource = Traverse.Create(this.atsc).Field("waveformSource").GetValue<AudioSource>();
                    waveformSource.clip = clip;
                    this.atsc.SongAudioSource.clip = clip;
                    SampleBufferManager.GenerateSamplesBuffer(clip);
                    this.audioManager.GenerateFFT(clip, Settings.Instance.SpectrogramSampleSize, Settings.Instance.SpectrogramEditorQuality);
                    if (playing) this.atsc.TogglePlaying();
                    this.IsAudioLoading = false;
                }, -offset, songFile);
                Debug.Log($"{songFile} Load");
            }
        }
    }
}
