using System;
using UnityEngine;

namespace Imas {
    [Serializable]
    public sealed class CharacterImasMotionAsset : ScriptableObject {

        public string kind;

        public object[] atrribs;

        public float time_length;

        public string date;

        public Curve[] curves;

    }
}
