using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryBag", menuName = "Inventory/New IventoryBag")]
public class InventoryBag : ScriptableObject
{
    public List<Item> bagList=new List<Item>();
}
