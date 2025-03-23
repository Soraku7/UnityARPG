using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaSystem : MonoBehaviour
{
    public float maxMana = 100f;      // 最大蓝量
    public float currentMana = 100f;  // 当前蓝量
    public float manaRegenRate = 5f;  // 每秒恢复量
    public float manaCost = 2f;       // 每次消耗的蓝量
    public float runningManaCost = 2f; // 按住 Shift + W 每秒消耗蓝量

    private CharacterInputSystem input;

    void Start()
    {
        input = GetComponent<CharacterInputSystem>();
    }

    void Update()
    {
       
        // if (Input.GetMouseButtonDown(0)) 
        // {
        //     UseMana(manaCost);
        // }
        if (Input.GetMouseButtonDown(1)) 
        {
            UseMana(manaCost);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) 
        {
            UseManaOverTime(runningManaCost);
        }
        else
        {
        
            RecoverMana();
        }
    }

    // 扣除蓝量
    private void UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
           
        }
        else
        {
            Debug.Log("蓝量不足！");
        }
    }

    // 持续消耗蓝量
    private void UseManaOverTime(float amountPerSecond)
    {
        if (currentMana > 0)
        {
            currentMana -= amountPerSecond * Time.deltaTime;
            currentMana = Mathf.Max(currentMana, 0); // 确保不低于 0
            
        }
        else
        {
            Debug.Log("蓝量耗尽，无法继续消耗！");
        }
    }

    /// <summary>
    /// 随时间恢复蓝量
    /// </summary>
    private void RecoverMana()
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(maxMana, currentMana + manaRegenRate * Time.deltaTime);
        }
    }

    public void RecoverMana(int mana)
    {
        currentMana += mana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }
}
