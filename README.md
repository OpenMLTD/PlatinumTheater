# Platium Theater

Tools for resource generation for [THE iDOLM@STER Million Live! Theater Days](https://millionlive.idolmaster.jp/theaterdays/) (MiriShita/ミリシタ).

## Details

This project demonstrates how to build custom music and beatmap playable in MLTD.

- The score source is from custom beatmap [Death by Glamour](http://undertale.wikia.com/wiki/Death_by_Glamour) ([here](https://www.bilibili.com/video/av15612246/) is its preview) while the data is built into a beatmap for [Blue Symphony](https://www.project-imas.com/wiki/Blue_Symphony). You can check this by running the game. Scenario data (including mouth sync/morph, UI animations, etc.) is from [Shooting Stars](https://www.project-imas.com/wiki/Shooting_Stars), so you will see mismatches in various UI elements. I've tried a minimal attempt to modify the time of some of the animations based on input score. There's an alternative file from Shooting Stars. Replace the content in `blsymp_fumen_sobj.txt` with the content in `shtstr_fumen_sobj.txt` to work.
- Blue Symphony's ACB file contains audio data from Death by Glamour. An alternative test contains [Brand New Theater!](https://www.project-imas.com/wiki/Brand_New_Theater!).
- Blue Symphony's song title is changed to an image writing "Death by Glamour". The most important parameters are sprite names (`songname_1` and `songname_2`) and sizes (256x72 and 84x72).

Current Unity project is for Unity 5.6.2f1. I might not have time to track all the things, so here is a check list if you want to build assets for the real game:

- For MLTD before 1.3.000, you need Unity 5.4+, but no later than 5.6.x. Recommended version is 5.6.2f1, which is also the one that MLTD was built with.
- For MLTD version 1.3.000 (live with 13 people on the stage) and newer, you need Unity 2017.4.0f1. Assets built with old Unity versions may cause app crash.

Since MLTD has a forced updating mechanism, you can safely use Unity 2017.3, unless there are specific requirements.

## Contributing

Contributions are [Welcome!!](https://www.project-imas.com/wiki/Welcome!!)

## License

BSD 3-Clause Clear License

External references:

- [YamlDotNet for Unity](https://assetstore.unity.com/packages/tools/integration/yamldotnet-for-unity-36292) is the work of Beetle Circus.
- [Standalone File Browser](https://github.com/gkngkc/UnityStandaloneFileBrowser) is the work of [Gökhan Gökçe](https://github.com/gkngkc) and contributors.
