using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RHFrame
{
    public class TestObserver : MonoBehaviour
    {
        public Object[] targetObject;
        public int i = 1;
        private void Start()
        {
            EventCenter.instance.listen(EventID.TEST , ObserverLog , targetObject);
            EventHelper.Triggle(EventID.TEST , targetObject);
        }

        private void ObserverLog(EventListArgs args)
        {
            Debug.Log("测试观察者模式");
            Debug.Log(args[1].ToString());
            Debug.Log(i++);
        }
    }
}