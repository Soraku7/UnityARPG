
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// 2进制数据管理器
/// </summary>
public class BinaryDataMgr
{
    private static BinaryDataMgr instance = new BinaryDataMgr();
    public static BinaryDataMgr Instance => instance;
    private BinaryDataMgr() { }

    /// <summary>
    /// 用于存储所有Excel表数据的容器
    /// </summary>
    private Dictionary<string, object> tableDic = new Dictionary<string, object>();

    /// <summary>
    /// 数据存储路径
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";
    /// <summary>
    /// 二进制数据存储路径
    /// </summary>
    public static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Bianry/";

    public void InitData()
    {
        //加载自己的表到内存
        //LoadTable<TowerInfoContainer, TowerInfo>();
        //LoadTable<PlayerInfo1Container, PlayerInfo1>();
        //LoadTable<TestInfoContainer, TestInfo>();
    }

    /// <summary>
    /// 加载Excel表的2进制数据到内存中
    /// </summary>
    /// <typeparam name="T">容器类名</typeparam>
    /// <typeparam name="K">数据结构体类名</typeparam>
    public void LoadTable<T, K>()
    {
        //读取Excel表对应的2进制文件  来进行解析
        using (FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name + ".zhou", FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            //用于记录当前读取了多少字节
            int index = 0;

            //读取多少行数据
            int count = BitConverter.ToInt32(bytes, index);
            index += 4;

            //读取主键的名字
            int keyNameLength = BitConverter.ToInt32(bytes, index);
            index += 4;
            string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
            index += keyNameLength;

            //创建容器类对象
            Type contaninerType = typeof(T);
            object contaninerObj = Activator.CreateInstance(contaninerType);
            //得到数据结构类Type
            Type classType = typeof(K);
            //通过反射得到数据结构类 所有字段的信息
            FieldInfo[] infos = classType.GetFields();
            //读取每一行的信息
            for (int i = 0; i < count; i++)
            {
                //实例化一个结构对象类 对象
                object dataObj = Activator.CreateInstance(classType);
                foreach (FieldInfo info in infos)
                {
                    if (info.FieldType == typeof(int))
                    {
                        //相当于将2进制数据转为int 然后赋值给对应字段
                        info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(float))
                    {
                        info.SetValue(dataObj, BitConverter.ToSingle(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(bool))
                    {
                        info.SetValue(dataObj, BitConverter.ToBoolean(bytes, index));
                        index += 1;
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        //读取字符串字节数组长度
                        int length = BitConverter.ToInt32(bytes, index);
                        index += 4;
                        info.SetValue(dataObj, Encoding.UTF8.GetString(bytes, index, length));
                        index += length;
                    }
                }

                //读取完一行的数据后 将这个数据存到容器对象中
                object dicObj = contaninerType.GetField("dataDic").GetValue(contaninerObj);

                //得到容器字段对象
                MethodInfo mInfo = dicObj.GetType().GetMethod("Add");
                //得到数据结构类对象中 指定主键字段的值
                object keyValue = classType.GetField(keyName).GetValue(dataObj);
                mInfo.Invoke(dicObj, new object[] { keyValue, dataObj });
            }

            //把读取完的表记录下来
            tableDic.Add(typeof(T).Name, contaninerObj);
            fs.Close();

        }
    }

    /// <summary>
    /// 得到一张表的信息
    /// </summary>
    /// <typeparam name="T">容器类名</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (tableDic.ContainsKey(tableName))
        {
            return tableDic[tableName] as T;
        }
        return null;
    }

    /// <summary>
    /// 存储类对象数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    public void Save(object data, string fileName)
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        using (FileStream fs = new FileStream(SAVE_PATH + fileName + ".zhou", FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, data);

            fs.Flush();
            fs.Close();
        }
    }

    /// <summary>
    /// 读取2进制数据转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>(string fileName) where T : class
    {
        if (!File.Exists(SAVE_PATH + fileName + ".zhou"))
        {
            return default(T);
        }

        T data;
        using (FileStream fs = new FileStream(SAVE_PATH + fileName + ".zhou", FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            data = bf.Deserialize(fs) as T;
            fs.Close();
        }
        return data;
    }

}