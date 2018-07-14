using System;
using System.Collections.Generic;
using System.Linq;
using Imas;
using JetBrains.Annotations;
using UnityEngine;

namespace PlatiumTheater.Runtime.Dancing {
    internal static class AnimationCreator {

        public static AnimationClip CreateFrom([NotNull] CharacterImasMotionAsset danceData) {
            var frameCount = danceData.curves.Max(curve => curve.values.Length);
            var frameDict = new Dictionary<string, List<KeyFrame>>();

            for (var i = 0; i < danceData.curves.Length; ++i) {
                var curve = danceData.curves[i];

                var path = curve.path;

                if (path.Contains("BODY_SCALE/")) {
                    path = path.Replace("BODY_SCALE/", string.Empty);
                }

                var keyType = curve.GetKeyType();
                var propertyType = curve.GetPropertyType();
                List<KeyFrame> frameList;

                if (frameDict.ContainsKey(path)) {
                    frameList = frameDict[path];
                } else {
                    frameList = new List<KeyFrame>();
                    frameDict.Add(path, frameList);
                }

                Func<int, KeyFrame> GetOrAddFrame = index => {
                    KeyFrame frame;

                    if (frameList.Count > index) {
                        frame = frameList[index];
                    } else {
                        frame = new KeyFrame(index, path);
                        frameList.Add(frame);
                    }

                    return frame;
                };

                switch (keyType) {
                    case KeyType.Const: {
                            switch (propertyType) {
                                case PropertyType.AngleX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).AngleX = curve.values[0];
                                    }

                                    break;
                                case PropertyType.AngleY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).AngleY = curve.values[0];
                                    }

                                    break;
                                case PropertyType.AngleZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).AngleZ = curve.values[0];
                                    }

                                    break;
                                case PropertyType.PositionX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).PositionX = curve.values[0];
                                    }

                                    break;
                                case PropertyType.PositionY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).PositionY = curve.values[0];
                                    }

                                    break;
                                case PropertyType.PositionZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        GetOrAddFrame(frameIndex).PositionZ = curve.values[0];
                                    }

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, $"Unknown property type: \"{propertyType}\".");
                            }
                        }
                        break;
                    case KeyType.FullFrame: {
                            var valueCount = curve.values.Length;

                            switch (propertyType) {
                                case PropertyType.AngleX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).AngleX = curve.values[index];
                                    }

                                    break;
                                case PropertyType.AngleY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).AngleY = curve.values[index];
                                    }

                                    break;
                                case PropertyType.AngleZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).AngleZ = curve.values[index];
                                    }

                                    break;
                                case PropertyType.PositionX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).PositionX = curve.values[index];
                                    }

                                    break;
                                case PropertyType.PositionY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).PositionY = curve.values[index];
                                    }

                                    break;
                                case PropertyType.PositionZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var index = frameIndex < valueCount ? frameIndex : valueCount - 1;
                                        GetOrAddFrame(frameIndex).PositionZ = curve.values[index];
                                    }

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, $"Unknown property type: \"{propertyType}\".");
                            }
                        }

                        break;
                    case KeyType.Discrete: {
                            if ((curve.values.Length % 2) != 0) {
                                throw new ApplicationException($"Length of curve values {curve.values.Length} is not a muliple of 2.");
                            }

                            var curveValueCount = curve.values.Length / 2;
                            var curTime = curve.values[0];
                            var curValue = curve.values[1];
                            var nextTime = curve.values[2];
                            var nextValue = curve.values[3];
                            var curIndex = 0;

                            Func<KeyFrame, float> InterpolateValue = frame => {
                                if (curIndex >= curveValueCount - 1) {
                                    return curValue;
                                }

                                var frameTime = frame.Time;

                                if (frameTime >= nextTime) {
                                    curTime = nextTime;
                                    curValue = nextValue;
                                    ++curIndex;

                                    if (curIndex < curveValueCount - 1) {
                                        nextTime = curve.values[(curIndex + 1) * 2];
                                        nextValue = curve.values[(curIndex + 1) * 2 + 1];
                                    }
                                }

                                if (curIndex >= curveValueCount - 1) {
                                    return curValue;
                                }

                                var duration = nextTime - curTime;
                                var delta = frameTime - curTime;
                                var p = delta / duration;

                                return curValue * (1 - p) + nextValue * p;
                            };

                            switch (propertyType) {
                                case PropertyType.AngleX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.AngleX = value;
                                    }

                                    break;
                                case PropertyType.AngleY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.AngleY = value;
                                    }

                                    break;
                                case PropertyType.AngleZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.AngleZ = value;
                                    }

                                    break;
                                case PropertyType.PositionX:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.PositionX = value;
                                    }

                                    break;
                                case PropertyType.PositionY:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.PositionY = value;
                                    }

                                    break;
                                case PropertyType.PositionZ:
                                    for (var frameIndex = 0; frameIndex < frameCount; ++frameIndex) {
                                        var frame = GetOrAddFrame(frameIndex);
                                        var value = InterpolateValue(frame);
                                        frame.PositionZ = value;
                                    }

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, $"Unknown property type: \"{propertyType}\".");
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(keyType), keyType, $"Unknown key type: \"{keyType}\".");
                }
            }

            var clip = new AnimationClip();

            clip.name = "Clip1";

            foreach (var kv in frameDict) {
                var path = kv.Key;
                var frameList = kv.Value;
                var frameListCount = frameList.Count;

                if (frameListCount > 0 && frameList[0].HasPositions) {
                    var posX = new Keyframe[frameListCount];
                    var posY = new Keyframe[frameListCount];
                    var posZ = new Keyframe[frameListCount];

                    for (var i = 0; i < frameListCount; ++i) {
                        var frame = frameList[i];
                        posX[i] = new Keyframe(frame.Time, frame.PositionX.Value);
                        posY[i] = new Keyframe(frame.Time, frame.PositionY.Value);
                        posZ[i] = new Keyframe(frame.Time, frame.PositionZ.Value);
                    }

                    var curveX = new AnimationCurve(posX);
                    var curveY = new AnimationCurve(posY);
                    var curveZ = new AnimationCurve(posZ);

                    clip.SetCurve(path, typeof(Transform), "localPosition.x", curveX);
                    clip.SetCurve(path, typeof(Transform), "localPosition.y", curveY);
                    clip.SetCurve(path, typeof(Transform), "localPosition.z", curveZ);
                }

                if (frameListCount > 0 && frameList[0].HasRotations) {
                    var rotX = new Keyframe[frameListCount];
                    var rotY = new Keyframe[frameListCount];
                    var rotZ = new Keyframe[frameListCount];
                    var rotW = new Keyframe[frameListCount];

                    for (var i = 0; i < frameListCount; ++i) {
                        var frame = frameList[i];
                        float rx = frame.AngleX.Value, ry = frame.AngleY.Value, rz = frame.AngleZ.Value;
                        var q = Quaternion.Euler(rx, ry, rz);
                        rotX[i] = new Keyframe(frame.Time, q.x);
                        rotY[i] = new Keyframe(frame.Time, q.y);
                        rotZ[i] = new Keyframe(frame.Time, q.z);
                        rotW[i] = new Keyframe(frame.Time, q.w);
                    }

                    var curveX = new AnimationCurve(rotX);
                    var curveY = new AnimationCurve(rotY);
                    var curveZ = new AnimationCurve(rotZ);
                    var curveW = new AnimationCurve(rotW);

                    clip.SetCurve(path, typeof(Transform), "localRotation.x", curveX);
                    clip.SetCurve(path, typeof(Transform), "localRotation.y", curveY);
                    clip.SetCurve(path, typeof(Transform), "localRotation.z", curveZ);
                    clip.SetCurve(path, typeof(Transform), "localRotation.w", curveW);
                }

                //for (var i = 0; i < frameListCount - 1; i++) {
                //    var startFrame = frameList[i];
                //    var endFrame = frameList[i + 1];

                //    if (startFrame.HasPositions) {
                //        clip.SetCurve(path, typeof(Transform), "localPosition.x", AnimationCurve.Linear(startFrame.Time, startFrame.PositionX.Value, endFrame.Time, endFrame.PositionX.Value));
                //        clip.SetCurve(path, typeof(Transform), "localPosition.y", AnimationCurve.Linear(startFrame.Time, startFrame.PositionY.Value, endFrame.Time, endFrame.PositionY.Value));
                //        clip.SetCurve(path, typeof(Transform), "localPosition.z", AnimationCurve.Linear(startFrame.Time, startFrame.PositionZ.Value, endFrame.Time, endFrame.PositionZ.Value));
                //    }

                //    if (startFrame.HasRotations) {
                //        float rx1 = startFrame.AngleX.Value, ry1 = startFrame.AngleY.Value, rz1 = startFrame.AngleZ.Value;
                //        float rx2 = endFrame.AngleX.Value, ry2 = endFrame.AngleY.Value, rz2 = endFrame.AngleZ.Value;

                //        var q1 = Quaternion.Euler(rx1, ry1, rz1);
                //        var q2 = Quaternion.Euler(rx2, ry2, rz2);

                //        clip.SetCurve(path, typeof(Transform), "localRotation.x", AnimationCurve.Linear(startFrame.Time, q1.x, endFrame.Time, q2.x));
                //        clip.SetCurve(path, typeof(Transform), "localRotation.y", AnimationCurve.Linear(startFrame.Time, q1.y, endFrame.Time, q2.y));
                //        clip.SetCurve(path, typeof(Transform), "localRotation.z", AnimationCurve.Linear(startFrame.Time, q1.z, endFrame.Time, q2.z));
                //        clip.SetCurve(path, typeof(Transform), "localRotation.w", AnimationCurve.Linear(startFrame.Time, q1.w, endFrame.Time, q2.w));
                //    }
                //}
            }

            clip.legacy = true;

            return clip;
        }

    }
}
