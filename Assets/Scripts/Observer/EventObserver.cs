using System;
using System.Collections;
using System.Collections.Generic;


namespace RHFrame
{
    public class EventObserver
    {
        public bool Active = true;
        /// <summary>
        /// 事件监听列表 key为事件名，value为该key事件对应的委托集合 非对象的成员函数
        /// </summary>
        public Dictionary<string, ListEventCallBack> _DicEventCallback;

        public EventObserver()
        {
            _DicEventCallback = new Dictionary<string, ListEventCallBack>();
        }
        /// <summary>
        /// 判断该对象是否已经有对应的委托列表
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callbak"></param>
        public void listen(string eventName, EventNotifyCallback callbak)
        {
            if (null != eventName)
            {

                ListEventCallBack lCallBack = null;
                if (!_DicEventCallback.ContainsKey(eventName))
                {
                    lCallBack = new ListEventCallBack();
                    _DicEventCallback.Add(eventName, lCallBack);
                }
                else
                {
                    lCallBack = _DicEventCallback[eventName];
                }
                lCallBack.listen(callbak);
            }
        }

        public void listen(string eventName, Action callback)
        {
            if (null != eventName)
            {
                //判断该对象是否已经有对应的委托列表
                ListEventCallBack lCallBack = null;
                if (!_DicEventCallback.ContainsKey(eventName))
                {
                    lCallBack = new ListEventCallBack();
                    _DicEventCallback.Add(eventName, lCallBack);
                }
                else
                {
                    lCallBack = _DicEventCallback[eventName];
                }
                lCallBack.listen(callback);
            }
        }
        public void trigger(string eventName, EventListArgs eventArgs) 
        {
            EventObserver eventObserver = null;
            if (_DicEventCallback.ContainsKey(eventName)) 
            {
                _DicEventCallback[eventName].trigger(eventArgs);
            }
              
        }
    }

}

