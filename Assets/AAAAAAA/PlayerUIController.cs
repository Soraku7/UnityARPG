// using UnityEngine;
// using UnityEngine.UI;
// using UGG.Health;

// public class PlayerUIController : MonoBehaviour
// {
//     private UGG.Health.CharacterHealthSystemBase playerHealth;
//     [SerializeField] private CharacterHealthSystemBase playerHealth; // 角色的生命系统
//     [SerializeField] private Image hpBar; // 血量条
//     [SerializeField] private Image stanceBar; // 架势条

//     private void Update()
//     {
//         if (playerHealth == null) return;

//         // 更新血量条
//         hpBar.fillAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;

//         // 更新架势条（蓝量）
//         stanceBar.fillAmount = playerHealth.CurrentStance / playerHealth.MaxStance;
//     }
// }
