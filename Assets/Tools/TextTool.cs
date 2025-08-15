using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextTool : Editor
{
    [MenuItem("UI工具/默认Text属性")]

    public static void ModifyText() 
    {
        foreach (var selectGo in Selection.gameObjects) 
        {
            Debug.Log($"选中物体{selectGo.name}");
            Text[] txts = selectGo.GetComponentsInChildren<Text>(true);
            foreach (var txt in txts)
            {
                var rect = txt.GetComponent<RectTransform>();
                if (rect.localScale.x < 1f || rect.localScale.y < 1f) continue;
                rect.sizeDelta *= 2;
                rect.localScale = new Vector3(0.5f, 0.5f, 1f);
                if (txt.fontSize < 20) txt.fontSize = 20;
                txt.fontSize *= 2;
                txt.horizontalOverflow = HorizontalWrapMode.Overflow;
                txt.verticalOverflow = VerticalWrapMode.Overflow;
                txt.raycastTarget = false;
                Debug.Log($"修改组件{selectGo.name} # {txt.name}");

                var outlines = txt.gameObject.GetComponents<Outline>();
                if (outlines.Length > 0)
                    UpdateOutline(txt, 2);
            }
            Debug.Log($"修改完成{selectGo.name}");


        }
    
    }
    [MenuItem("UI工具/默认Text属性", true)]
    private static bool VlidateModifyText() 
    {
        return Selection.activeTransform != null;
    }

    private static void UpdateOutline(Text txt, float scale = 1f)
    {
        int Time = 2;

        var outlines = txt.gameObject.GetComponents<Outline>();
        var length = outlines.Length;
        var hasOutLine = length > 0;
        Outline firstOutLine = null;
        if (hasOutLine)
        {
            firstOutLine = outlines[0];
            for (var i = 0; i < length; i++)
            {
                var outline = outlines[i];
                outline.effectDistance = new Vector2(scale, -scale);
            }

        }
        var start = Time - length;
        if (start <= 0) return;
        for (int i = start; i < Time; i++)
        {
            var outLine = txt.gameObject.AddComponent<Outline>();
            outLine.effectDistance = new Vector2(scale, -scale);
            if (hasOutLine)
            {
                outLine.effectColor = firstOutLine.effectColor;
            }
            else
            {
                outLine.effectColor = Color.black;
            }
        }
    }
}
