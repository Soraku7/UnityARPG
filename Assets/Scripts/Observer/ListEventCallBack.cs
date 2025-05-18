using System;
using System.Collections.Generic;
namespace RHFrame 
{
    /// <summary>
    /// 事件委托集合
    /// </summary>
    public class ListEventCallBack
    {
        //委托列表
        public List<EventNotifyCallback> _ListCallback;
        public List<Action> _ListActionCallback;
        //无参构造
        public ListEventCallBack() 
        {
            _ListCallback = new List<EventNotifyCallback>();
            _ListActionCallback = new List<Action>();
               
        }
        /// <summary>
        /// 事件监听
        /// </summary>
        /// <param name="callback"></param>
        public void listen(EventNotifyCallback callback) 
        { 
            _ListCallback.Add(callback);      
        }

        public void listen(Action callback)
        {
            _ListActionCallback.Add(callback);
        }
        /// <summary>
        /// 事件分发
        /// </summary>
        /// <param name="args"></param>
        public void trigger(EventListArgs eventArgs) 
        {
            if (_ListCallback.Count > 0) 
            {
                foreach (EventNotifyCallback callback in _ListCallback) 
                {
                    callback(eventArgs);
                }           
            }

            if (_ListActionCallback.Count > 0)
            {
                foreach (Action callback in _ListActionCallback)
                {
                    callback();
                }
            }
        
        }


    }




}