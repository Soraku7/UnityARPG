using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddHpItem : MonoBehaviour
{
    [SerializeField]
    private Item _item;
    
    [SerializeField]
    private AudioClip sound;
    
    private AudioSource audio;

    private void Start()
    {
        audio = transform.parent.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("拾取");
        if (other.CompareTag("Player"))
        {
            InventoryMannage.Instance.AddItem(_item);
            audio.PlayOneShot(sound);
            Destroy(gameObject);
        }
    }
}
