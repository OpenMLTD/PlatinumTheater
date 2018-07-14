using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Imas;
using JetBrains.Annotations;
using UnityEngine;

namespace PlatiumTheater.Runtime.Dancing {
    public sealed class MLTDModelAnimator : MonoBehaviour {

        public string songResourceName {
            get { return _songResourceName; }
            set { _songResourceName = value; }
        }

        public int motionNumber {
            get { return _motionNumber; }
            set { _motionNumber = value; }
        }

        public string idolSerialName {
            get { return _idolSerialName; }
            set { _idolSerialName = value; }
        }

        public string costumeName {
            get { return _costumeName; }
            set { _costumeName = value; }
        }

        // Use this for initialization
        private IEnumerator Start() {
            if (string.IsNullOrWhiteSpace(songResourceName)) {
                LogError("Song resource name is empty.");
                yield break;
            }

            if (string.IsNullOrWhiteSpace(idolSerialName)) {
                LogError("Idol serial name is empty.");
                yield break;
            }

            if (string.IsNullOrWhiteSpace(costumeName)) {
                LogError("Costume name is empty.");
                yield break;
            }

            if (motionNumber < 1 || motionNumber > 5) {
                LogError($"Invalid motion number: {motionNumber}, should be 1 to 5.");
                yield break;
            }

            var danceAssetName = $"dan_{songResourceName}_{motionNumber:00}";
            var modelAssetName = $"cb_{costumeName}_{idolSerialName}";

            if (!DanceAssetNameRegex.IsMatch(danceAssetName)) {
                LogError($"\"{danceAssetName}\" is not a valid dance asset name.");
                yield break;
            }

            if (!ModelAssetNameRegex.IsMatch(modelAssetName)) {
                LogError($"\"{modelAssetName}\" is not a valid model asset name.");
                yield break;
            }

            var danceBundlePath = Path.Combine(Application.streamingAssetsPath, $"{danceAssetName}.unity3d");
            var modelBundlePath = Path.Combine(Application.streamingAssetsPath, $"{modelAssetName}.unity3d");

            AssetBundle danceBundle, modelBundle;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                var www = new WWW(danceBundlePath);
                yield return www;

                danceBundle = www.assetBundle;

                www.Dispose();

                www = new WWW(modelBundlePath);
                yield return www;

                modelBundle = www.assetBundle;

                www.Dispose();
            } else {
                if (!File.Exists(danceBundlePath)) {
                    LogError($"Dance bundle is not found at '{danceBundlePath}'.");
                    yield break;
                }

                if (!File.Exists(modelBundlePath)) {
                    LogError($"Model bundle is not found at '{modelBundlePath}'.");
                    yield break;
                }

                danceBundle = AssetBundle.LoadFromFile(danceBundlePath);
                modelBundle = AssetBundle.LoadFromFile(modelBundlePath);
            }

            if (danceBundle == null) {
                LogError("Dance bundle is null.");
                yield break;
            }

            if (modelBundle == null) {
                LogError("Model bundle is null.");
                yield break;
            }

            var danceAssetPath = $"assets/imas/resources/exclude/imo/dance/{songResourceName}/{danceAssetName}_dan.imo.asset";
            var danceData = danceBundle.LoadAsset<CharacterImasMotionAsset>(danceAssetPath);
            var modelAssetPath = $"assets/imas/resources/chara/body/{modelAssetName}/{modelAssetName}.fbx";
            var fbx = modelBundle.LoadAsset<GameObject>(modelAssetPath);

            var instance = Instantiate(fbx);

            var ren = instance.GetComponentInChildren<SkinnedMeshRenderer>();
            _renderer = ren;

            //var comp = instance.GetComponentsInChildren<Component>();

            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;

            var animator = instance.GetComponent<Animator>();

            Debug.Assert(animator != null);

            // OK, create the animation
            var clip = AnimationCreator.CreateFrom(danceData);

            var anim = instance.AddComponent<Animation>();

            anim.AddClip(clip, clip.name);
            anim.clip = clip;

            _animation = anim;
        }

        // Update is called once per frame
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (_playbackStarted) {
                    if (_animation != null) {
                        _animation.Stop();
                        _playbackStarted = false;
                    }
                } else {
                    if (_animation != null) {
                        _animation.Play();
                        _playbackStarted = true;
                    }
                }
            }
        }

        private void LogError(string log) {
            Debug.LogError(log);
        }

        // e.g.: dan_shtstr
        private static readonly Regex DanceAssetNameRegex = new Regex(@"^dan_[a-z0-9]{6}_[0-9]{2}$");
        // e.g.: cb_ss001_015siz
        private static readonly Regex ModelAssetNameRegex = new Regex("^cb_[a-z]{2}[0-9]{3}_[0-9]{3}[a-z]{3}$");

        private SkinnedMeshRenderer _renderer;

        // e.g.: shtstr
        [SerializeField]
        private string _songResourceName = "shtstr";

        // 1..5
        [SerializeField]
        private int _motionNumber = 1;

        // e.g.: 015siz
        [SerializeField]
        private string _idolSerialName = "001har";

        // e.g.: ss001
        [SerializeField]
        private string _costumeName = "ss001";

        private bool _playbackStarted;
        [CanBeNull]
        private Animation _animation;

    }
}
