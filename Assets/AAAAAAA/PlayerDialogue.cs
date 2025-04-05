using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialpgue : MonoBehaviour
{
    public GameObject panel; // UI对话框
    public Text dialogueText; // 显示对话的 Text
    public float textSpeed = 0.05f; // 逐字显示速度

    private int currentLine = 0; // 当前对话索引
    private bool isDialogueActive = false; // 是否在对话中
    private bool isTyping = false; // 是否正在逐字显示
    private bool hasTriggered = false; // 防止重复触发

    private string[] dialogues = new string[]
    {
        "NPC：你好，你知道游戏规则吗？请按L键继续对话",
        "Player：你好，我知道一些。",
        "NPC：攻击是鼠标左键攻击。",
        "Player：听起来不错，那鼠标右键就是防御咯？",
        "NPC：当然！你可以空格切换武器和WASD移动，开始吧！"
    };

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false); // 初始隐藏对话框
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered) // 只有 Player 触碰到才触发
        {
            hasTriggered = true;
            ShowPanel();
        }
    }

    private void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0; // 暂停游戏
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        StartCoroutine(TypeDialogue(dialogues[currentLine]));
    }

    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // 清空文本

        foreach (char letter in line.ToCharArray()) // 逐字显示
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed); // 逐字显示
        }

        isTyping = false;
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.L) && !isTyping) // 按 L 继续对话
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        if (currentLine < dialogues.Length - 1)
        {
            currentLine++;
            StartCoroutine(TypeDialogue(dialogues[currentLine]));
        }
        else
        {
            StartCoroutine(HidePanel());
        }
    }

    private IEnumerator HidePanel()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (panel != null)
            panel.SetActive(false);
        Time.timeScale = 1; // 恢复游戏
    }
}
