using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //if (GUILayout.Button("Update Positions"))
        //{
        //    Tile[] tiles = FindObjectsOfType<Tile>();

        //    int width = 8;
        //    int height = 8;

        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            int index = y * width + x;
        //            if (index < tiles.Length)
        //            {
        //                tiles[index].MyPosition = new Vector2(x, y);
        //                EditorUtility.SetDirty(tiles[index]);
        //            }
        //        }
        //    }
        //}
    }
}
#endif
