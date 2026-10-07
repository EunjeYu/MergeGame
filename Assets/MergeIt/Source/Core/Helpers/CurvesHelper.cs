// Copyright (c) 2024, Awessets

using UnityEngine;

namespace MergeIt.Core.Helpers
{
    public static class CurvesHelper
    {
        public static AnimationCurve CopyCurveFrom(AnimationCurve curve, float originValue)
        {
            var newCurve = new AnimationCurve();
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe kf = curve.keys[i];
                kf.value += originValue;
                newCurve.AddKey(kf);
            }

            return newCurve;
        }
        
        public static AnimationCurve CopyCurveFrom(AnimationCurve curve, float originValue, float lastValue)
        {
            var newCurve = new AnimationCurve();
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe kf = curve.keys[i];
                if (i == curve.keys.Length - 1)
                {
                    kf.value += lastValue;
                }
                else
                {
                    kf.value += originValue;
                }
                
                newCurve.AddKey(kf);
            }

            return newCurve;
        }
    }
}