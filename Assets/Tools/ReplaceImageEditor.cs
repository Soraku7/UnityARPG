using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageReplacer : EditorWindow
{
    private Texture2D sourceImage;
    private Texture2D targetImage;
    private string sourceImagePath;
    private string targetImagePath;

    [MenuItem("Tools/图片替换工具")]
    public static void ShowWindow()
    {
        GetWindow<ImageReplacer>("图片替换工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("选择要替换的图片", EditorStyles.boldLabel);
        sourceImage = (Texture2D)EditorGUILayout.ObjectField("原图片", sourceImage, typeof(Texture2D), false);

        if (sourceImage != null)
        {
            GUILayout.Label("原图片预览", EditorStyles.boldLabel);
            GUILayout.Box(sourceImage, GUILayout.Width(128), GUILayout.Height(128));
        }

        GUILayout.Label("选择用来替换的新图片", EditorStyles.boldLabel);
        if (GUILayout.Button("从目标文件夹选择新图片"))
        {
            targetImagePath = EditorUtility.OpenFilePanel("选择新图片", "", "png,jpg,jpeg");
            if (!string.IsNullOrEmpty(targetImagePath))
            {
                byte[] fileData = File.ReadAllBytes(targetImagePath);
                targetImage = new Texture2D(2, 2);
                if (!targetImage.LoadImage(fileData))
                {
                    Debug.LogError("无法加载新图片");
                    targetImage = null;
                }
            }
        }

        if (targetImage != null)
        {
            GUILayout.Label("新图片预览", EditorStyles.boldLabel);
            GUILayout.Box(targetImage, GUILayout.Width(128), GUILayout.Height(128));
        }

        if (GUILayout.Button("替换图片"))
        {
            if (sourceImage == null || targetImage == null)
            {
                Debug.LogError("请先选择原图片和新图片");
                return;
            }
            ReplaceImage();
        }
    }

    private void ReplaceImage()
    {
        // 获取原图片的路径
        sourceImagePath = AssetDatabase.GetAssetPath(sourceImage);

        // 读取新图片的字节数组
        byte[] targetImageBytes = File.ReadAllBytes(targetImagePath);

        // 将新图片的字节数组写入原图片的路径，覆盖原图片
        File.WriteAllBytes(sourceImagePath, targetImageBytes);

        // 刷新AssetDatabase以确保Unity正确识别更改
        AssetDatabase.Refresh();

        // 更新所有依赖于原图片的资源
        string[] allGuids = AssetDatabase.FindAssets("t:Sprite");
        foreach (string guid in allGuids)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid));
            if (sprite.texture == sourceImage)
            {
                // 重新导入精灵以应用更改
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(sprite), ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log("图片替换完成！");
    }
}
