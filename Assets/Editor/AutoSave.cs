using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave
{
    // Constructor tĩnh được gọi ngay khi Unity load
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state)
    {
        // Kiểm tra nếu đang chuẩn bị bấm Play
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto-saving before entering Play Mode...");

            // Lưu Scene hiện tại
            EditorApplication.ExecuteMenuItem("File/Save");

            // Lưu cả các Asset/Prefab đã chỉnh sửa
            AssetDatabase.SaveAssets();
        }
    }
}