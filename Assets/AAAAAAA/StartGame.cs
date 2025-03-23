using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject panel; // 在 Inspector 中拖入你的 Panel

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (panel != null)
            {
                panel.SetActive(false); // 隐藏 Panel
            }
          //  SceneManager.LoadScene("SampleScene"); // 替换成你的游戏场景名称
        }
    }
}
