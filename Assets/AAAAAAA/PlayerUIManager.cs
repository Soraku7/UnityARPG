// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UGG.Health; 

// namespace UGG.UI
// {
//     public class PlayerUIManager : MonoBehaviour
//     {
//         public Slider healthSlider;
//         public Slider stanceSlider;

//         public PlayerHealthSystem healthSystem;

//         private void Start()
//         {
//             healthSystem = FindObjectOfType<PlayerHealthSystem>();
//             healthSystem.OnHealthChanged += UpdateHealthUI;
//             healthSystem.OnStanceChanged += UpdateStanceUI;

//             // 确保 UI 正确更新
//             UpdateHealthUI(healthSystem.currentHealth, healthSystem.maxHealth);
//             UpdateStanceUI(healthSystem.currentStance, healthSystem.maxStance);
//         }

//         private void UpdateHealthUI(float current, float max)
//         {
//             healthSlider.value = current / max;
//         }

//         private void UpdateStanceUI(float current, float max)
//         {
//             stanceSlider.value = current / max;
//         }
//     }
// }
