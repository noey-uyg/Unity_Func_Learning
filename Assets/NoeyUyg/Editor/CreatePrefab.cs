using UnityEngine;
using UnityEditor;

public class CreatePrefab
{
    public static void CreatePrefabs(string fileName)
    {
        string prefabPath = $"Assets/My/Prefabs/{fileName}.prefab";

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab != null)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if(Selection.activeTransform != null)
            {
                instance.transform.SetParent(Selection.activeTransform, false);
            }

            EditorGUIUtility.PingObject(instance);

            Undo.RegisterCreatedObjectUndo(instance, "Create" + instance.name);
        }
        else
        {
            Debug.LogError("No Prefab : " + prefabPath);
        }
    }

    //Create
   #region Fruits_Create
   [MenuItem("GameObject/Prefabs/Fruits/1/Apple",false, 1)]
   public static void CreateApple() => CreatePrefabs("Apple");
   [MenuItem("GameObject/Prefabs/Fruits/1/Banana",false, 1)]
   public static void CreateBanana() => CreatePrefabs("Banana");
   [MenuItem("GameObject/Prefabs/Fruits/1/Blueberry",false, 1)]
   public static void CreateBlueberry() => CreatePrefabs("Blueberry");
   [MenuItem("GameObject/Prefabs/Fruits/2/Cherry",false, 1)]
   public static void CreateCherry() => CreatePrefabs("Cherry");
   [MenuItem("GameObject/Prefabs/Fruits/2/Coconut",false, 1)]
   public static void CreateCoconut() => CreatePrefabs("Coconut");
   [MenuItem("GameObject/Prefabs/Fruits/2/Lime",false, 1)]
   public static void CreateLime() => CreatePrefabs("Lime");
   [MenuItem("GameObject/Prefabs/Fruits/3/Melon",false, 1)]
   public static void CreateMelon() => CreatePrefabs("Melon");
   [MenuItem("GameObject/Prefabs/Fruits/3/Orange",false, 1)]
   public static void CreateOrange() => CreatePrefabs("Orange");
   [MenuItem("GameObject/Prefabs/Fruits/3/Peach",false, 1)]
   public static void CreatePeach() => CreatePrefabs("Peach");
   [MenuItem("GameObject/Prefabs/Fruits/3/Pineapple",false, 1)]
   public static void CreatePineapple() => CreatePrefabs("Pineapple");
   [MenuItem("GameObject/Prefabs/Fruits/3/Watermelon",false, 1)]
   public static void CreateWatermelon() => CreatePrefabs("Watermelon");
   #endregion

   #region Panel_Create
   [MenuItem("GameObject/Prefabs/Panel/GameoverPanel",false, 1)]
   public static void CreateGameoverPanel() => CreatePrefabs("GameoverPanel");
   [MenuItem("GameObject/Prefabs/Panel/Play_BG",false, 1)]
   public static void CreatePlay_BG() => CreatePrefabs("Play_BG");
   [MenuItem("GameObject/Prefabs/Panel/Title_BG",false, 1)]
   public static void CreateTitle_BG() => CreatePrefabs("Title_BG");
   #endregion


}