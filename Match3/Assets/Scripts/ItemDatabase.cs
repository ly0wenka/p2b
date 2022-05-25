using UnityEngine;

public static class ItemDatabase
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
        => Items = Resources.LoadAll<Item>("Items/");

    // => Items = Resources.LoadAll<Item>($"{nameof(Items)}{Path.PathSeparator}");
}