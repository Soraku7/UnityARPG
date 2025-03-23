using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponAnimationEvent : MonoBehaviour
{
    [SerializeField] private Transform hipGS;
    [SerializeField] private Transform handGS;
    [SerializeField] private Transform handKatana;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HitHideGS();
    }

    public void ShowGS()
    {
        if (!handGS.gameObject.activeSelf)
        {

            handGS.gameObject.SetActive(true);

            hipGS.gameObject.SetActive(false);
            
            handKatana.gameObject.SetActive(false);
        }
    }

    public void HideGS()
    {

        if (handGS.gameObject.activeSelf)
        {

            handGS.gameObject.SetActive(false);
            
            hipGS.gameObject.SetActive(true);
            
            handKatana.gameObject.SetActive(true);
        }
    }

    private void HitHideGS()
    {
        if (animator.CheckAnimationTag("Hit") || animator.CheckAnimationTag("ParryHit"))
        {
            handKatana.gameObject.SetActive(true);

            hipGS.gameObject.SetActive(true);
            handGS.gameObject.SetActive(false);
        }
    }
}
