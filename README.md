# ChroMapper-SongDataChanger
BeatSaberの作譜ツールの[ChroMapper](https://github.com/Caeden117/ChroMapper)で、曲データを一時的に変更するプラグインです。

元の曲データと同じフォルダにある、*.ogg(egg) と *.wavファイルを一覧から選択して切り替えが可能です。また、読み込む際に、オフセット調整をして読み込む事が可能です。

オフセット調整は±1, ±0.5ビート毎の調整も可能です。[ArrowVortex](https://arrowvortex.ddrnl.com/)でビート毎に何度もボタンを押すと小数点誤差から起きるズレの問題も、本プラグインでは発生しないようになっています。

オフセット設定は一時的なものなので、オフセット値決定後は、[Audacity](https://www.audacityteam.org/)などで元の曲データを直接編集してオフセット調整をしてください。

また、選択したバッチ実行ファイルに元の曲データを渡して実行する機能もあります。
この機能を使用して、[Demucs](https://github.com/facebookresearch/demucs)を起動して曲データをドラム・ベース・ボーカルなどに分離する方法も紹介します。

Demucsを細かい調整をして使いたい場合は、以下のGUIツールなどを参考にして下さい。
- [Demucs Gui](https://github.com/CarlGao4/Demucs-Gui)
- [Ultimate Vocal Remover GUI](https://github.com/Anjok07/ultimatevocalremovergui)

----
 This is a plugin that temporarily modifies the music data using the BeatSaber mapping tool [ChroMapper](https://github.com/Caeden117/ChroMapper).

It is possible to switch between *.ogg (egg) and *.wav files in the same folder as the original song data by selecting them from the list. It is also possible to adjust the offset when loading.

Offset adjustment is also possible in increments of ±1 or ±0.5 beats. This plugin also prevents the problem of drift caused by decimal point errors that occurs when you press the button multiple times for each beat in [ArrowVortex](https://arrowvortex.ddrnl.com/).

The offset setting is temporary, so after you have determined the offset value, please adjust the offset by directly editing the original song data in [Audacity](https://www.audacityteam.org/) or other software.

There is also a function that passes the original song data to the selected batch execution file and executes it.
Using this function, we will also introduce how to start [Demucs](https://github.com/facebookresearch/demucs) and separate the song data into drums, bass, vocals, etc.

If you want to make fine adjustments to Demucs, please refer to the following GUI tools, etc.
- [Demucs Gui](https://github.com/CarlGao4/Demucs-Gui)
- [Ultimate Vocal Remover GUI](https://github.com/Anjok07/ultimatevocalremovergui)

![image](https://github.com/user-attachments/assets/c4ace612-294d-4121-945a-ecbc1e437d67)

# 説明動画 (Explanatory Video)

https://github.com/user-attachments/assets/253e412e-ee38-4f2e-95eb-c081c8302c04

[Neko Hacker](https://nekohacker.com/)さんの[Chocolate Adventure feat. ななひら](https://nekohacker.com/neko-hacker/)を説明用に使用させて頂きました。

[BeatSaberの譜面作成などに利用許可をして頂いています](https://docs.google.com/document/d/1RHaeGxbfUlw5LacqokJSjOxuJk8_eqD9jaqPXlYfTy0/edit)

We used [Neko Hacker](https://nekohacker.com/)'s [Chocolate Adventure feat. Nanahira](https://nekohacker.com/neko-hacker/) as an explanation tool.

[We have permission to use it for BeatSaber map creation, etc.](https://docs.google.com/document/d/1RHaeGxbfUlw5LacqokJSjOxuJk8_eqD9jaqPXlYfTy0/edit)

# インストール方法 (How to Install)

1. [リリースページ](https://github.com/rynan4818/ChroMapper-SongDataChanger/releases)から、最新のプラグインのzipファイルをダウンロードして下さい。

    Download the latest plug-in zip file from the [release page](https://github.com/rynan4818/ChroMapper-SongDataChanger/releases).

2. ダウンロードしたzipファイルを解凍してChroMapperのインストールフォルダにある`Plugins`フォルダに`ChroMapper-SongDataChanger.dll`をコピーします。

    Unzip the downloaded zip file and copy `ChroMapper-SongDataChanger.dll` to the `Plugins` folder in the ChroMapper installation folder.

3. Demucsを使用して曲データを分離する場合は、別途Demucsのインストールが必要です。インストール方法は、こちらの[Googleドライブにインストール用zipとマニュアルあります](https://drive.google.com/drive/u/1/folders/1bm_OMCmlPIrX1vOgWgVwGNfBB4HrXUnA)ので参考にして下さい。

    If you want to use Demucs to separate the song data, you will need to install Demucs separately. For installation instructions, please refer to [There is a zip file and manual for installation on Google Drive](https://drive.google.com/drive/u/1/folders/1bm_OMCmlPIrX1vOgWgVwGNfBB4HrXUnA)

# 使用方法 (How to use)

1. 譜面のエディタ画面でTabキーを押すと右側にアイコンパネルが出ますので、紫色の♫アイコンを押すと下の画像の様な設定パネルが表示されます。

    Press the Tab key on the map editor screen to display the icon panel on the right, and then press the purple ♫ icon to display the settings panel shown in the image below.

2. Demucsで曲データを分離する場合は、`Batch File Select`ボタンでインストールしたDemucsの実行用バッチファイルを選択します。通常は`htdemucs_ft_split.bat`を推奨します。ピアノ・ギターも分離したい場合は`htdemucs_6s_split.bat`を選択します。（ただし、6s_splitはDemucsの調整が不十分です)

    To separate song data with Demucs, select the batch file for executing Demucs installed with the `Batch File Select` button. Normally, `htdemucs_ft_split.bat` is recommended. If you want to separate piano guitar as well, select `htdemucs_6s_split.bat`. (However, 6s_split is not well adjusted for Demucs)

![image](https://github.com/user-attachments/assets/dc4eccf9-0227-43cc-ad64-2e80ef2d4b13)

3. Demucsの実行用バッチファイルを選択したら、`Batch File RUN`ボタンで実行します。分離が終わるまで少しの間待ちます。

    After selecting the Demucs executable batch file, click the `Batch File RUN` button to execute it. Wait a moment until the separation is complete.

4. 分離が終わったら、曲データのドロップダウンリストが更新されるので、好きなデータを選択します。

    When the separation is complete, the drop-down list of music data will be updated, so select the data you want.

5. オフセット調整が必要な場合は、オフセット値を直接設定またはボタンで調整して下さい。オフセット未調整のデータの場合は、ArrowVortexのADJUST SYNCでオフセット値の初期値を調査して入力し、必要な分をボタンで調整すると言った使い方ができます。

    If you need to adjust the offset, you can either set the offset value directly or adjust it using the buttons. If you have data that has not been offset, you can use the ADJUST SYNC function in ArrowVortex to find the initial offset value and enter it, then use the buttons to adjust it as necessary.

6. 最後に`Change`ボタンで曲データを変更します。

    Finally, use the `Change` button to change the song data.

# 設定パネル説明 (Explanation of the settings panel)
![image](https://github.com/user-attachments/assets/cc66f054-814b-41d1-a8ba-d41722dad6aa)

- `Song List Reload` Button
    - 曲データリストの再読込
    
        Reloading the list of music data
- `ドロップダウンリスト(Drop-down list)`
    - 曲データの選択
    
        Selecting music data
- `Change` Button
    - 選択した曲データへの変更
        
        Changes to the selected song data
- `Offset`
    - 読み込む曲データのオフセット設定
        
        Offset setting for loaded music data
- `±*ms` `±* beat` Button
    - 固定値のオフセット加算と減算
        
        Add or subtract an offset from a fixed value
- `0` Button
    - オフセット値を0に設定
        
        Set the offset value to 0
- `Default Reset` Button
    - 曲データとオフセット値を譜面の設定値にリセット
        
        Reset the music data and offset values to the settings in the sheet music.
- `htdemucs_ft_split.bat` Label
    - 設定されたバッチファイルを表示
        
        Display the set batch file
- `Batch File RUN` Button
    - 設定したバッチファイルを実行
        
        Execute the batch file you have set up.
- `Batch File Select` Button
    - バッチファイルを選択して設定
        
        Select a batch file and configure it.
- `Close` Button
    - 設定パネルを閉じる
        
        Close the settings panel.

※バッチファイル実行後に曲リストは自動的に更新されます

※The song list is automatically updated after the batch file is executed.

# 設定ファイルについて (About the configuration file)
設定ファイルはChroMapperの設定ファイルと同じフォルダ`ユーザ設定フォルダ(Users)\ユーザ名\AppData\LocalLow\BinaryElement\ChroMapper`の`SongDataChanger.json`に保存されます。

The configuration file is saved in `SongDataChanger.json` in the same folder as ChroMapper's configuration file `User Settings Folder(Users)\User Name\AppData\LocalLow\BinaryElement\ChroMapper`.

| 設定項目 (Setting Item) | デフォルト値 (Default Value) | 説明 (Description) |
|:---|:---|:---|
| menuUIAnchoredPosX | -50 | 設定パネルのX位置<br>X position in the configuration panel |
| menuUIAnchoredPosY | -75 | 設定パネルのY位置<br>Y position in the configuration panel |
| dragEnableBinding | ＜Keyboard＞/shift | 設定パネル移動のキーバインド<br>Key bindings for setting panel movement |
| batachFilePath | - | バッチファイルのパス設定<br>Setting the path for the batch file |
| batchStartTimeout | 10 | バッチ実行開始までのタイムアウト時間<br>Timeout period until batch execution starts |
| batchRunTimeout | 600 | バッチ実行中のタイムアウト時間<br>Timeout duration during batch execution |
| batchUITitle | Batch | UIの`Batch`の文字<br>The word `Batch` in the UI |
| batchExtension | bat | バッチファイル選択での拡張子<br>Extension when selecting a batch file |

キーバインドはUnityのInputSystem形式で設定してください。<Br>
Key bindings should be set in Unity's InputSystem format.

# 開発者情報 (Developers)
このプロジェクトをビルドするには、ChroMapperのインストールパスを指定する`ChroMapper-SongDataChanger\ChroMapper-SongDataChanger.csproj.user`ファイルを作成する必要があります。

To build this project, you must create a `ChroMapper-SongDataChanger\ChroMapper-SongDataChanger.csproj.user` file that specifies the ChroMapper installation path.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <ChroMapperDir>C:\TOOL\ChroMapper\chromapper</ChroMapperDir>
  </PropertyGroup>
</Project>
```
