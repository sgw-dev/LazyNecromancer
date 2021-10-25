using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StatResource", order = 1)]
public class StatResource : ScriptableObject
{
    private int value = 0;
    [SerializeField]
    public UnityEvent OnValueChange;
    [SerializeField]
    public int minValue = 0;
    [SerializeField]
    public UnityEvent OnMinHit;
    [SerializeField]
    public int maxValue = 1;
    [SerializeField]
    public UnityEvent OnMaxHit;

    public StatResource() {
        value = maxValue;

        if(OnValueChange == null) {
            OnValueChange = new UnityEvent();
        }
        if(OnMaxHit == null) {
            OnMaxHit = new UnityEvent();
        }
        if(OnMinHit == null) {
            OnMinHit = new UnityEvent();
        }
    }

    public int GetValue() {
        return value;
    }

    public void SetValue(int update_value) {
        int initialValue = value;
        value = Mathf.Clamp(update_value, minValue, maxValue);
        if(value != initialValue) {
            OnValueChange.Invoke();

            if(value == maxValue) {
                OnMaxHit.Invoke();
            }

            if(value == minValue) {
                OnMinHit.Invoke();
            }
        }
    }

    public void AddValue(int addition_value) {
        this.SetValue(value + addition_value);
    }
    public void SubValue(int subtract_value) {
        this.SetValue(value - subtract_value);
    }


    //===================
    // Operator Overides 
    //===================
    public static StatResource operator + (StatResource stat, int value) {
        stat.AddValue(value);
        return stat;
    }
    public static StatResource operator - (StatResource stat, int value) {
        stat.SubValue(value);
        return stat;
    }
    public static bool operator == (StatResource stat, int value) {
        return stat.value == value;
    }
    public static bool operator != (StatResource stat, int value) {
        return stat.value != value;
    }
    public static bool operator > (StatResource stat, int value) {
        return stat.value > value;
    }
    public static bool operator >= (StatResource stat, int value) {
        return stat.value >= value;
    }
    public static bool operator < (StatResource stat, int value) {
        return stat.value < value;
    }
    public static bool operator <= (StatResource stat, int value) {
        return stat.value <= value;
    }

    //====================
    // Function Overrides
    //====================
    public override string ToString() {
        return this.value.ToString();
    }
    public override int GetHashCode()
    {
        return this.value;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as int?);
    }
    public bool Equals(int compare_value) {
        return this.value == compare_value;
    }
}