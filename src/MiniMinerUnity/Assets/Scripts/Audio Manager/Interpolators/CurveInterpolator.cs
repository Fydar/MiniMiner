﻿using System;
using UnityEngine;

[Serializable]
public struct CurveInterpolator : IInterpolator
{
    public float Speed;
    public AnimationCurve Curve;

    private float targetValue;
    private float currentValue;

    public CurveInterpolator(float speed, AnimationCurve curve)
    {
        Speed = speed;
        Curve = curve;

        targetValue = 0.0f;
        currentValue = 0.0f;
    }

    public float Value
    {
        get => Curve.Evaluate(currentValue);
        set => currentValue = value;
    }

    public float TargetValue
    {
        set => targetValue = value;
    }

    public bool Sleeping => currentValue == targetValue;

    public void Update(float deltaTime)
    {
        if (Sleeping)
        {
            return;
        }

        float movementAmount = Speed * deltaTime;

        if (currentValue < targetValue)
        {
            currentValue = Mathf.Min(currentValue + movementAmount, targetValue);
        }
        else
        {
            currentValue = Mathf.Max(currentValue - movementAmount, targetValue);
        }
    }
}