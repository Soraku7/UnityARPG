using System;
using System.Collections;
using System.Collections.Generic;
using UGG.Health;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMannage : MonoBehaviour
{
    public static InventoryMannage _instance;//设置成单例模式

    public InventoryBag playerBag;//玩家背包
    public GameObject slotGrid;//装备栏
    //public Slot slotPrefab;
    public GameObject emptslot;//物品预制体
    public Text itemInfomation;//物品描述的text
    
    public Item curItem;//当前选中的物品

    public GameObject player;
    
    public List<GameObject> slots=new List<GameObject>();//因为直接在装备栏生成18个装备，用一个集合来存储，并标记序号
    private void Awake()
    {
        if(_instance!=null)
            Destroy(this);
        _instance = this;
        itemInfomation.text = "";
    }

    private void OnEnable()
    {
        RestItem();
        curItem = null;
        player = GameObject.FindWithTag("Player");
    }
    
    
    public static void RestItem()
    {
        //删除所有的
        for (int i = 0; i < _instance.slotGrid.transform.childCount; i++)
        {
            Destroy(_instance.slotGrid.transform.GetChild(i).gameObject);
            _instance.slots.Clear();
        }

        //添加
        for (int i = 0; i <_instance.playerBag.bagList.Count; i++)
        {
            //CreatNewItem(instance.playerBag.bagList[i]);
            _instance.slots.Add(Instantiate(_instance.emptslot));//物品集合添加物品
            _instance.slots[i].transform.SetParent(_instance.slotGrid.transform);//让物品成为物品栏得到子集
            
            if(_instance.slots[i].GetComponent<Slot>() == null) Debug.Log("Slot找不到");
            
            _instance.slots[i].GetComponent<Slot>().slotId = i;//物品栏的物品序号初始化
            _instance.slots[i].GetComponent<Slot>().SetupSlot(_instance.playerBag.bagList[i]);//物品信息的初始化
        }
    }

    /// <summary>
    /// 进行物品描述信息的赋值
    /// </summary>
    /// <param name="info"></param>
    public static void UpItemInfomation(string info)
    {
        _instance.itemInfomation.text = info;
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItem()
    {
        //当前物品数量-1
        //如果数量为0则删除
        if (curItem == null) return;
        playerBag.bagList.Find(item => item == curItem).itemHeld--;
        if (playerBag.bagList.Find(item => item == curItem).itemHeld <= 0)
        {
            //将ScriptObject的数量重置为1
            playerBag.bagList.Find(item => item == curItem).itemHeld = 1;
            playerBag.bagList.Remove(curItem);
            
            //保证格子数量
            playerBag.bagList.Add(null);
        }
        
        //使用道具效果
        switch (curItem.itemName)
        {
            case "AddHp":
                AddHp();
                break;
            case "AddMp":
                AddMp();
                break;
        }
        
        //重置背包
        RestItem();
    }
    
    /// <summary>
    /// 获取物品
    /// </summary>
    /// <param name="thisItem">当前物品</param>
    /// <param name="thisBag">当前添加物品的背包</param>
    void AddItem(Item thisItem ,InventoryBag thisBag)
    {
        if (!thisBag.bagList.Contains(thisItem))
        {
            //thisBag.bagList.Add(thisItem);
            //遍历背包，如果为null的话就设置成该物品，因为我们直接给装备栏添加了18个物品，只是初始化都是null
            for (int i = 0; i < thisBag.bagList.Count; i++)
            {
                if (thisBag.bagList[i] == null)
                {
                    thisBag.bagList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;//如果存在该物品，就将数量+1；
        }
        InventoryMannage.RestItem();//更新背包的数据
    }
    

    public void AddHp()
    {
        PlayerHealthSystem playerHealthSystem = _instance.player.GetComponent<PlayerHealthSystem>();
        playerHealthSystem.RecoverHealth(10);
    }

    public void AddMp()
    {
        PlayerManaSystem playerManaSystem = _instance.player.GetComponent<PlayerManaSystem>();
        playerManaSystem.RecoverMana(10);
    }
}

