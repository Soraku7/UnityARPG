using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : MonoBehaviour
{
    private Button closeBtn;
    private Button useBtn;

    private void Awake()
    {
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        useBtn = transform.Find("UseBtn").GetComponent<Button>();
        
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(CloseBtnOnClick);
        useBtn.onClick.AddListener(UseBtnOnClick);
    }
    
    private void CloseBtnOnClick()
    {
        gameObject.SetActive(false);
        Debug.Log("UseBtnOnClick");
    }
    
    private void UseBtnOnClick()
    {
        Debug.Log("UseBtnOnClick");
        InventoryMannage._instance.UseItem();
        
    }
}
