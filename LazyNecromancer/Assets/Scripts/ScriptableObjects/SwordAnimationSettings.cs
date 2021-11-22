using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Settings", menuName = "ScriptableObjects/Sword Settings")]
public class SwordAnimationSettings : ScriptableObject
{
    [SerializeField] float duration;

    [Space(10)]

    [SerializeField] float arcOffset;
    [SerializeField] float arc;
    [SerializeField] AnimationCurve arcOverTime;

    [Space(10)]

    [SerializeField] float radiusOffset;
    [SerializeField] float radius;
    [SerializeField] AnimationCurve radiusOverTime;

    [Space(10)]

    [SerializeField] float rotationOffset;
    [SerializeField] float rotation;
    [SerializeField] AnimationCurve rotationOverTime;

    public float EvaluateArc(float time)
    {
        return arcOffset + arc * arcOverTime.Evaluate(time);
    }

    public float EvaluateRadius(float time)
    {
        return radiusOffset + radius * radiusOverTime.Evaluate(time);
    }

    public float EvaluateRotation(float time)
    {
        return rotationOffset + rotation * rotationOverTime.Evaluate(time);
    }

    public float Duration => duration;

    public float ArcOffset => arcOffset;
    public float Arc => arc;
    public AnimationCurve ArcOverTime => arcOverTime;

    public float RadiusOffset => radiusOffset;
    public float Radius => radius;
    public AnimationCurve RadiusOverTime => radiusOverTime;

    public float RotationOffset => rotationOffset;
    public float Rotation => rotation;
    public AnimationCurve RotationOverTime => rotationOverTime;
}
