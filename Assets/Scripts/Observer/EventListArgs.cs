using System.Collections.Generic;
using System.Threading.Tasks;
namespace RHFrame
{
    public class EventListArgs
    {
        //参数列表
        List<object> listArgs = new List<object>();

        public static EventListArgs NewInstance(params object[] args)
        {
            EventListArgs eventArgs = new EventListArgs();
            if (null != args)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    eventArgs.Push(args[i]);

                }
            }
            return eventArgs;
        }

        public int Length
        {
            get
            {
                return listArgs.Count;
            }
        }

        public object this[int index]
        {
            get
            {
                return GetValueByIndex(index);
            }
        }
        public void Push(object arg)
        {
            if (null == arg)
                return;
            listArgs.Add(arg);
        }

        private object GetValueByIndex(int index)
        {
            object arg = null;
            if (index < listArgs.Count)
            {
                arg = listArgs[index];
            }
            return arg;
        }




    }
}