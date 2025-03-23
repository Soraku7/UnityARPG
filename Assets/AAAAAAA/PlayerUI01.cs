// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UGG.Combat;

// public class PlayerUI01 : MonoBehaviour
// {
//     public Image hpFill;
//     public Image staminaFill;

//     [SerializeField] private PlayerCombatSystem player; // 添加手动绑定

//     void Start()
//     {
//         if (player == null)
//         {
//             player = FindObjectOfType<PlayerCombatSystem>(); // 确保它存在
//         }
//     }

//     void Update()
//     {
//         if (player != null)
//         {
//             hpFill.fillAmount = player.GetHealthSystem().GetCurrentHealth() / player.GetHealthSystem().GetMaxHealth();
//             staminaFill.fillAmount = player.GetCurrentStamina() / player.GetMaxStamina(); // 确保这两个方法已存在
//         }
//     }

// }

