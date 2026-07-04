using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapAssetHealer : EditorWindow
{
    private Tilemap targetTilemap;

    [MenuItem("Tools/Tilemap Asset Healer")]
    public static void ShowWindow()
    {
        GetWindow<TilemapAssetHealer>("Tilemap Healer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Heal Broken Tilemap Links", EditorStyles.boldLabel);
        targetTilemap = (Tilemap)EditorGUILayout.ObjectField("Target Tilemap", targetTilemap, typeof(Tilemap), true);

        if (GUILayout.Button("Scan Project and Repair Red Tiles"))
        {
            ExecuteRepair();
        }
    }

    private void ExecuteRepair()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("Tilemap Healer: Please assign a target tilemap layer first.");
            return;
        }

        // 1. Scan your project folder for all your new valid tile assets
        string[] guids = AssetDatabase.FindAssets("t:TileBase");
        Dictionary<string, TileBase> cleanTileLookup = new Dictionary<string, TileBase>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null && !cleanTileLookup.ContainsKey(tile.name.ToLower().Trim()))
            {
                cleanTileLookup.Add(tile.name.ToLower().Trim(), tile);
            }
        }

        // 2. Read the boundaries of the scene tilemap coordinates
        BoundsInt bounds = targetTilemap.cellBounds;
        int healCount = 0;

        // 3. Sweep every active grid square inside your scene layer boundaries
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // We use GetUsedTiles to inspect the historical reference name hidden in the scene data
            TileBase brokenTile = targetTilemap.GetTile(pos);

            // If the cell data is active but drawing blank, let's try a backup name query
            string cellTileName = "";

            // Unity retains the original sprite asset name in its internal serializable block
            Sprite sprite = targetTilemap.GetSprite(pos);
            if (sprite != null) cellTileName = sprite.name;

            if (!string.IsNullOrEmpty(cellTileName))
            {
                string lookupKey = cellTileName.ToLower().Trim();

                // If we find an entry in our project folder matching that name, heal it!
                if (cleanTileLookup.TryGetValue(lookupKey, out TileBase matchingValidTile))
                {
                    targetTilemap.SetTile(pos, matchingValidTile);
                    healCount++;
                }
            }
        }

        EditorUtility.SetDirty(targetTilemap);
        AssetDatabase.SaveAssets();
        Debug.Log($"Tilemap Healer: Successfully restored {healCount} tile coordinate reference links inside the scene!");
    }
}
