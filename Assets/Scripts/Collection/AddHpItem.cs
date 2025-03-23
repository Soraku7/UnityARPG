using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddHpItem : MonoBehaviour
{
    [SerializeField]
    private Item _item;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("拾取");
        if (other.CompareTag("Player"))
        {
            InventoryMannage.Instance.AddItem(_item);
            Destroy(gameObject);
        }
    }
}
