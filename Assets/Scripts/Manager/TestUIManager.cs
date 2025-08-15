using System;
using System.Collections.Generic;
using UnityEngine;

namespace RHFrame
{
    public class TestUIManager : MonoBehaviour
    {
        private void Start()
        {
            // UIManager.Instance.ShowPanel<TestPanel>();
            BinaryDataMgr.Instance.LoadTable<CharacterContainer , Character>();
            
            var character = BinaryDataMgr.Instance.GetTable<CharacterContainer>().dataDic[101];
            Debug.Log(character.Name);
        }
    }
}