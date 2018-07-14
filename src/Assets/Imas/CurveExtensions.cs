using System;
using JetBrains.Annotations;

namespace Imas {
    internal static class CurveExtensions {

        public static PropertyType GetPropertyType([NotNull] this Curve curve) {
            var s = curve.attribs[0].Substring(14); // - "property_type "

            switch (s) {
                case "AngleX":
                    return PropertyType.AngleX;
                case "AngleY":
                    return PropertyType.AngleY;
                case "AngleZ":
                    return PropertyType.AngleZ;
                case "PositionX":
                    return PropertyType.PositionX;
                case "PositionY":
                    return PropertyType.PositionY;
                case "PositionZ":
                    return PropertyType.PositionZ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }

        public static KeyType GetKeyType([NotNull] this Curve curve) {
            var s = curve.attribs[1].Substring(9); // - "key_type "

            switch (s) {
                case "Const":
                    return KeyType.Const;
                case "Discreate": // This typo exists in MLTD, not my fault.
                    return KeyType.Discrete;
                case "FullFrame":
                    return KeyType.FullFrame;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }

    }
}
