using UnityEngine;

public static class ItemDatabase
{
    public static Item[] Items { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
        => Items = Resources.LoadAll<Item>("Items/");
}