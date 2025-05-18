using System;
using System.Collections;
using System.Collections.Generic;

namespace RHFrame 
{
    public delegate void EventNotifyCallback(EventListArgs eventArgs);
    /// <summary>
    /// 事件中心
    /// </summary>
    public class EventCenter 
    {
        private static EventCenter _Instance = null;

        public static EventCenter instance 
        {
            get 
            {
                if (_Instance == null)
                    _Instance = new EventCenter();
                return _Instance;
            }      
        }
        /// <summary>
        /// 对象作为key,value为对象中的委托
        /// </summary>
        Dictionary<System.Object, EventObserver> _DicEventObserver;

        public EventCenter() 
        {
            _DicEventObserver = new Dictionary<System.Object, EventObserver>();
        }

        /// <summary>
        /// 添加监听 LuaBehaviourProxy需要调用自身listen 以便自动注销
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="callback">委托</param>
        /// <param name="target">委托所属于的对象</param>
        public void listen(string eventName, EventNotifyCallback callback, System.Object target)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                //Debug.LogError("EventCenter Listen 参数 eventName 为空!");
                return;
            }
            if (null == callback)
            {
                //Debug.LogError("EventCenter Listen 参数 callback为null!");
                return;
            }

            if (null == target)
            {
                //Debug.LogError("EventCenter Listen 参数 target为null!");
                return;
            }

            EventObserver eventObserver = null;
            //已经包含此函数的key 不能添加
            if (_DicEventObserver.ContainsKey(target))
            {
                eventObserver = _DicEventObserver[target];
            }
            else
            {
                eventObserver = new EventObserver();
                _DicEventObserver.Add(target, eventObserver);
            }

            eventObserver.listen(eventName, callback);
        }

        /// <summary>
        ///  LuaBehaviourProxy需要调用自身listen 以便自动注销
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        /// <param name="target"></param>
        public void listen(string eventName, Action callback, System.Object target)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                //Debug.LogError("EventCenter Listen 参数 eventName 为空!");
                return;
            }
            if (null == callback)
            {
                //Debug.LogError("EventCenter Listen 参数 callback为null!");
                return;
            }

            if (null == target)
            {
                //Debug.LogError("EventCenter Listen 参数 target为null!");
                return;
            }

            EventObserver eventObserver = null;
            //已经包含此函数的key 不能添加
            if (_DicEventObserver.ContainsKey(target))
            {
                eventObserver = _DicEventObserver[target];
            }
            else
            {
                eventObserver = new EventObserver();
                _DicEventObserver.Add(target, eventObserver);
            }

            eventObserver.listen(eventName, callback);
        }

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        public void trigger(string eventName, params System.Object[] args)
        {
            if (_DicEventObserver.Count <= 0)
                return;
            List<EventObserver> tempList1 = new List<EventObserver>();
            Dictionary<System.Object, EventObserver>.Enumerator enumerator = _DicEventObserver.GetEnumerator();
            while (enumerator.MoveNext())
            {
                tempList1.Add(enumerator.Current.Value);
            }
            for (int i = 0, listCount = tempList1.Count; i < listCount; ++i)
            {
                if (tempList1[i].Active)
                {
                    try
                    {
                        tempList1[i].trigger(eventName, EventListArgs.NewInstance(args));
                    }
                    catch (Exception e)
                    {
                        //Log.Exception(e);
                    }
                }
            }
            tempList1.Clear();
        }

        /// <summary>
		/// 是否存在指定侦听器
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool ExistObserver(System.Object obj)
        {
            if (null == obj)
            {
                return false;
            }
            EventObserver eventObserver = null;
            if (_DicEventObserver.TryGetValue(obj, out eventObserver))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除指定对象中的所有监听事件
        /// </summary>
        /// <param name="obj">指定的对象</param>
        public void RemoveObserver(System.Object obj)
        {
            if (null == obj)
            {
                //Log("IgnoreScope 对象为空,请检查传入参数");
                return;
            }

            EventObserver eventObserver = null;
            if (_DicEventObserver.TryGetValue(obj, out eventObserver))
            {
                eventObserver.Active = false;
                _DicEventObserver.Remove(obj);
            }
        }

        /// <summary>
        /// 清理所有消息监听器
        /// </summary>
        public void ClearAllObserver() 
        {
            if (_DicEventObserver != null)
            {
                _DicEventObserver.Clear();
            }
        }
    
    
    }

}