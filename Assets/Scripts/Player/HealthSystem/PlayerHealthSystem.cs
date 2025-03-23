using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UGG.Health
{
    public class PlayerHealthSystem : CharacterHealthSystemBase
    {
        private bool canExecute = false;
        private CharacterHealthSystemBase _characterHealthSystemBase;
        

        protected override void Update()
        {
            base.Update();
            OnHitLockTarget();

            if (currentAttacker != null)
            {
                if(currentAttacker.GetComponent<CharacterHealthSystemBase>().currentHealth <= 0)
                {
                    canExecute = true;
                }
            }
            
        }

        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);

            if (CanParry())
            {
                Parry(hitAnimationName);
            }
            else
            {
                _animator.Play(hitAnimationName, 0, 0f);
                GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
                currentHealth = Mathf.Max(0, currentHealth - damageFromSword);

                if (currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        // //新增碰撞检测：当玩家碰到敌人的剑时，减少血量
        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("EnemyWeapon")) // 确保敌人武器有 "EnemyWeapon" 标签
        //     {
        //         currentHealth = Mathf.Max(0, currentHealth - damageFromSword);
        //         Debug.Log($"玩家受到剑攻击，当前血量：{currentHealth}");
        //
        //         if (currentHealth <= 0)
        //         {
        //             Die();
        //         }
        //     }
        // }

        private void Die()
        {
            Debug.Log("玩家死亡！");
            Destroy(gameObject);
            UIManager.Instance.GameOver();
        }

        #region Parry

        private bool CanParry()
        {
            if (_animator.CheckAnimationTag("Parry")) return true;
            if (_animator.CheckAnimationTag("ParryHit")) return true;
            return false;
        }

        private void Parry(string hitName)
        {
            if (!CanParry()) return;

            switch (hitName)
            {
                default:
                    _animator.Play(hitName, 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
                    break;
                case "Hit_D_Up":
                    //弹刀处理
                    if (currentAttacker.TryGetComponent(out CharacterHealthSystemBase health))
                    {
                        health.FlickWeapon("Flick_0");
                        GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    }

                    canExecute = true;

                    Time.timeScale = 0.25f;
                    GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(0.25f, () =>
                    {
                        canExecute = false;
                        if (Time.timeScale < 1f)
                        {
                            Time.timeScale = 1f;
                        }
                    }, false);
                    break;
                case "Hit_H_Right":
                    _animator.Play("ParryL", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
            }
        }

        #endregion

        #region Hit

        private bool CanHitLockAttacker()
        {
            return true;
        }

        private void OnHitLockTarget()
        {
            if (_animator.CheckAnimationTag("Hit") || _animator.CheckAnimationTag("ParryHit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }

        #endregion

        public bool GetCanExecute() => canExecute;

        /// <summary>
        /// 增加血量
        /// </summary>
        public void RecoverHealth(int addHealth)
        {
            currentHealth += addHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
}