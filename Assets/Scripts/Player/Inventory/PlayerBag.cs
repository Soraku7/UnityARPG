using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBag : MonoBehaviour
{
    
    public GameObject bag;//玩家的背包
    private bool isOpen = false;//背包是否打开

    private void Update()
    {
        BagSet();
    }

    void BagSet()
    {
        //打开背包显示UI同时暂停游戏
        //鼠标同时显示
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !bag.activeSelf;
            bag.SetActive(isOpen);
            
            Time.timeScale = isOpen ? 0 : 1;
            Cursor.visible = isOpen;
        }
    }
}