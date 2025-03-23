using UnityEngine;
using UnityEngine.UI;

public class ShowCanvasOnTrigger : MonoBehaviour
{
public GameObject panel; // 拖入 Panel
    public Transform playerB; // 拖入 人物 B
    public float triggerDistance = 3f; // 触发的距离
    public float destroyDelay = 2f; // 2 秒后销毁

    private bool panelShown = false; // 记录 Panel 是否已显示

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false); // 开局隐藏 Panel
    }

    private void Update()
    {
        if (playerB != null && !panelShown)
        {
            float distance = Vector3.Distance(transform.position, playerB.position);
            if (distance <= triggerDistance)
            {
                ShowAndDestroyPanel(); // 显示并销毁 Panel
            }
        }
    }

    private void ShowAndDestroyPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true); // 显示 Panel
            panelShown = true; // 标记已显示
            Destroy(panel, destroyDelay); // 2 秒后销毁 Panel
        }
    }
}
