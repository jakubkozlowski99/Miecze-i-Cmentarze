using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Ink.Parsed;
using System.Linq;

public class Enums
{
    public int EnumStringToHash(string value) => Animator.StringToHash(value);

    public List<T> EnumToList<T>() where T : Enum
    {
        var result = new List<T>();

        result = Enum.GetValues(typeof(T))
        .Cast<T>()
        .ToList();

        return result;
    }
    public List<T> EnumToList<T>(int maxIntValue) where T : Enum
    {
        var enumList = new List<T>();

        enumList = Enum.GetValues(typeof(T))
        .Cast<T>()
        .ToList();

        var result = new List<T>();

        foreach (var value in enumList) if ((int)(object)value <= maxIntValue) result.Add(value);

        return result;
    }
    public List<T> EnumToList<T>(int minIntValue, int maxIntValue) where T : Enum
    {
        var enumList = new List<T>();

        enumList = Enum.GetValues(typeof(T))
        .Cast<T>()
        .ToList();

        foreach (var value in enumList) if ((int)(object)value <= maxIntValue && (int)(object)value >= minIntValue) enumList.Remove(value);

        return enumList;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class StringValueAttribute : Attribute
{
    public StringValueAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; }
}

public static class EnumExtensions
{
    public static string StringValue<T>(this T value)
        where T : Enum
    {
        var fieldName = value.ToString();
        var field = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
        return field?.GetCustomAttribute<StringValueAttribute>()?.Value ?? fieldName;
    }
}

public enum KeyActions
{
    Up, Down, Left, Right, 
    Dodge, 
    Interaction, 
    Attack, 
    Toggle_Inventory
}

public enum AnimationTags
{
    // player combat animation tags
    [StringValue("PlayerSwing_1")]
    PlayerSwing_1 = 1,
    [StringValue("PlayerSwing_2")]
    PlayerSwing_2 = 2,
    [StringValue("PlayerSwing_3")]
    PlayerSwing_3 = 3,
    [StringValue("PlayerHurt")]
    PlayerHurt = 4,
    [StringValue("PlayerDodge")]
    PlayerDodge = 5

}

public enum Sounds
{
    
}
