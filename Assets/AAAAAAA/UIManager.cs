using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UGG.Health;


public class UIManager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    
    private PlayerHealthSystem playerHealth;
    private PlayerManaSystem playerMana;

    void Start()
    {

        var player = GameObject.FindWithTag("Player");
          Debug.Log(player); // 看看是否成功找到 Player
        playerHealth = player.GetComponent<PlayerHealthSystem>();
        playerMana = player.GetComponent<PlayerManaSystem>();
        Debug.Log($"最大血量: {playerHealth?.maxHealth}, 当前血量: {playerHealth?.currentHealth}");
    Debug.Log($"最大蓝量: {playerMana?.maxMana}, 当前蓝量: {playerMana?.currentMana}");

        healthSlider.maxValue = playerHealth.maxHealth;
        manaSlider.maxValue = playerMana.maxMana;
    }

    void Update()
    {
        healthSlider.value = playerHealth.currentHealth;
        manaSlider.value = playerMana.currentMana;
    }
}