﻿using System;
using UnityEngine;

[Serializable]
public class LinearInterpolator : IInterpolator
{
    public float Speed;

    private float targetValue;
    private float currentValue;

    public LinearInterpolator(float speed)
    {
        Speed = speed;

        targetValue = 0.0f;
        currentValue = 0.0f;
    }

    public float Value
    {
        get => currentValue;
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