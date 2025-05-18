using System;
using System.Collections;
using System.Collections.Generic;

namespace RHFrame
{
    public class EventHelper
    {
        public static void Triggle(string eventName, params System.Object[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    //特殊数据装箱处理

                }
            }
            EventCenter.instance.trigger(eventName, args);
        }        
    }



}
