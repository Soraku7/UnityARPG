using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// 为了实现点击物品跟随鼠标移动，所以它添加上IBeginDragHandler,IDragHandler,IEndDragHandler三个事件
/// 注意添加这三个接口时添加头文件：using UnityEngine.EventSystems;
/// </summary>
public class ItemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    /// <summary>
    /// 记录原来的父级位置，因为可能进行物品交换位置，所以记录原先的父级
    /// </summary>
    public Transform startParent;
    public InventoryBag PlayerBag;//玩家的背包
    private int startID;//当前初始的序号，点击物品的刚开始序号，因为交换挪位置，所以也要记录下来
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startID = startParent.GetComponent<Slot>().slotId;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;//物品跟随鼠标的位置
        GetComponent<CanvasGroup>().blocksRaycasts = false;//防止拖拽的物体挡住鼠标的射线检测
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")//判断下面物体的名字是：ItemImage 那么互换位置
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                var temp = PlayerBag.bagList[startID];
                PlayerBag.bagList[startID] =
                    PlayerBag.bagList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId];
                PlayerBag.bagList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = temp;
            
            
                //进行交换位置，改变父子级别
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = startParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(startParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                PlayerBag.bagList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = PlayerBag
                    .bagList[startID];
        
               
                if(eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotId!=startID)
                    PlayerBag.bagList[startID] = null;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
        
        
        //其他任何位置都设置成原来的位置
        transform.SetParent(startParent);
        transform.position = startParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

