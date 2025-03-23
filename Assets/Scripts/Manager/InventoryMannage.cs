using System;
using System.Collections;
using System.Collections.Generic;
using MyUnitTools;
using UGG.Health;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMannage : SingletonBase<InventoryMannage>
{

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
        for (int i = 0; i < Instance.slotGrid.transform.childCount; i++)
        {
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
            Instance.slots.Clear();
        }

        //添加
        for (int i = 0; i <Instance.playerBag.bagList.Count; i++)
        {
            //CreatNewItem(instance.playerBag.bagList[i]);
            Instance.slots.Add(Instantiate(Instance.emptslot));//物品集合添加物品
            Instance.slots[i].transform.SetParent(Instance.slotGrid.transform);//让物品成为物品栏得到子集
            
            if(Instance.slots[i].GetComponent<Slot>() == null) Debug.Log("Slot找不到");
            
            Instance.slots[i].GetComponent<Slot>().slotId = i;//物品栏的物品序号初始化
            Instance.slots[i].GetComponent<Slot>().SetupSlot(Instance.playerBag.bagList[i]);//物品信息的初始化
        }
    }

    /// <summary>
    /// 进行物品描述信息的赋值
    /// </summary>
    /// <param name="info"></param>
    public static void UpItemInfomation(string info)
    {
        Instance.itemInfomation.text = info;
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
    public void AddItem(Item thisItem)
    {
        if (!playerBag.bagList.Contains(thisItem))
        {
            //thisBag.bagList.Add(thisItem);
            //遍历背包，如果为null的话就设置成该物品，
            for (int i = 0; i < playerBag.bagList.Count; i++)
            {
                if (playerBag.bagList[i] == null)
                {
                    playerBag.bagList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;//如果存在该物品，就将数量+1；
        }
        
        RestItem();//更新背包的数据
    }
    

    public void AddHp()
    {
        PlayerHealthSystem playerHealthSystem = Instance.player.GetComponent<PlayerHealthSystem>();
        playerHealthSystem.RecoverHealth(10);
    }

    public void AddMp()
    {
        PlayerManaSystem playerManaSystem = Instance.player.GetComponent<PlayerManaSystem>();
        playerManaSystem.RecoverMana(10);
    }
}

