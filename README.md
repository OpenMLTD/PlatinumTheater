# Platium Theater

Tools for resource generation for [THE iDOLM@STER Million Live! Theater Days](https://millionlive.idolmaster.jp/theaterdays/) (MiriShita/ミリシタ).

## Details

This project demonstrates how to build custom music and beatmap playable in MLTD.

### Custom Music and Scores

- The score source is from custom beatmap [Death by Glamour](http://undertale.wikia.com/wiki/Death_by_Glamour) ([here](https://www.bilibili.com/video/av15612246/) is its preview) while the data is built into a beatmap for [Blue Symphony](https://www.project-imas.com/wiki/Blue_Symphony). You can check this by running the game. Scenario data (including mouth sync/morph, UI animations, etc.) is from [Shooting Stars](https://www.project-imas.com/wiki/Shooting_Stars), so you will see mismatches in various UI elements. I've tried a minimal attempt to modify the time of some of the animations based on input score. There's an alternative file from Shooting Stars. Replace the content in `blsymp_fumen_sobj.txt` with the content in `shtstr_fumen_sobj.txt` to work.
- Blue Symphony's ACB file contains audio data from Death by Glamour. An alternative test contains [Brand New Theater!](https://www.project-imas.com/wiki/Brand_New_Theater!).
- Blue Symphony's song title is changed to an image writing "Death by Glamour". The most important parameters are sprite names (`songname_1` and `songname_2`) and sizes (256x72 and 84x72).

### Dance Motion Viewer

1. Put these asset bundles in `StreamingAssets` folder (under Unity's `Assets` folder): model data (`cb_xxyyy_zzzwww.unity3d`) and dance data (`dan_xxxxxx_yy.unity3d). If there is no `StreamingAssets` folder, create one.
2. Set the fields of "MLTD Model Animator" attached on `GameObject` in the default scene. A template is given.
3. Run the game and press space key to view.

## Unity Versions

Current Unity project is for Unity 2017.4.0f1. I might not have time to track all the things, so here is a check list if you want to build assets for the real game:

- For MLTD before 1.3.000, you need Unity 5.4+, but no later than 5.6.x. Recommended version is 5.6.2f1, which is also the one that MLTD was built with.
- For MLTD version 1.3.000 (live with 13 people on the stage) and newer, you need Unity 2017.4.0f1. Assets built with old Unity versions may cause app crash.

Since MLTD has a forced updating mechanism, you can safely use Unity 2017.4, unless there are specific requirements.

## Known Issues

- The project uses Standalone File Browser (see references). Visual Studio cannot recognize the reference to `System.Windows.Forms.dll` but Unity can. As a result, when the "play" button in Unity is pressed, everything works. However, Visual Studio always prompts error and refuses to attach to Unity for debugging.
- The model in dance viewer is not renderered correctly. I'm not very familiar with Unity's mechanisms so please help if you can.

## Contributing

Contributions are [Welcome!!](https://www.project-imas.com/wiki/Welcome!!)

## License

BSD 3-Clause Clear License

External references:

- [YamlDotNet for Unity](https://assetstore.unity.com/packages/tools/integration/yamldotnet-for-unity-36292) is the work of Beetle Circus.
- [Standalone File Browser](https://github.com/gkngkc/UnityStandaloneFileBrowser) is the work of [Gökhan Gökçe](https://github.com/gkngkc) and contributors.
