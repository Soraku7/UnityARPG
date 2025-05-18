using System;
using UnityEngine;

namespace RHFrame
{
    public class TestUIManager : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.ShowPanel<TestPanel>();
        }
    }
}