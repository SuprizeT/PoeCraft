using UnityEngine;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureSystemsExist()
    {
        if (EconomyManager.Instance != null) return; 

        var prefab = Resources.Load<GameObject>("GameSystems");
        if (prefab == null)
        {
            Debug.LogError("GameSystems prefab missing from a Resources folder.");
            return;
        }
        Object.Instantiate(prefab);
    }
}