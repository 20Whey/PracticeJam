using System;
using System.Collections.Generic;

public static class EnumExtensions {
    public static T GetEnumWithHighestValue<T>(Dictionary<T, int> dictionary) where T : Enum {
        T maxEnum = default;
        int maxValue = int.MinValue;

        foreach (var kvp in dictionary) {
            if (kvp.Value > maxValue) {
                maxEnum = kvp.Key;
                maxValue = kvp.Value;
            }
        }

        return maxEnum;
    }

    public static T GetNextEnumValue<T>(T currentValue) where T : Enum {
        T[] values = (T[])Enum.GetValues(typeof(T));
        int currentIndex = Array.IndexOf(values, currentValue);
        int nextIndex = (currentIndex + 1) % values.Length;
        return values[nextIndex];
    }

    private static readonly System.Random random = new();

    public static T GetRandomEnumValue<T>() {
        if (!typeof(T).IsEnum) {
            throw new ArgumentException("Type T must be an enum type");
        }

        Array enumValues = Enum.GetValues(typeof(T));

        int randomIndex = random.Next(enumValues.Length);

        return (T)enumValues.GetValue(randomIndex);
    }

}