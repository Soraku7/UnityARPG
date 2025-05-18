using System;
using System.Collections.Generic;
using MyUnitTools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RHFrame 
{
    /// <summary>
    /// UI节点管理器 需在页面中挂载此脚本，节点下需有UIRoot节点，预制体和脚本需要同名
    /// </summary>
    public class UIManager : SingletonBase<UIManager>
    {
        #region 字段

        //显示的节点
        private Transform uiroot;
        //节点名称
        private const string uiRootName = "UIRoot";

        //面板预制体的路径 (Resouces目录下)
        private const string PanelObjPath = "Prefabs/Panel/";

        //记录当前已经显示的面板
        private Dictionary<string, PanelBase> panelDic = new Dictionary<string, PanelBase>();
        #endregion


        #region 属性
        public Transform UIRoot
        {
            get
            {
                if (uiroot == null)
                {
                    uiroot = transform.Find(uiRootName);
                }

                return uiroot;
            }
        }
        #endregion



        #region 外部方法

        /// <summary>
        /// 显示面板
        /// </summary>
        public T ShowPanel<T>( ) where T : PanelBase
        {
            string panelName = typeof(T).Name;
            if (panelDic.ContainsKey(panelName))
            {
                return panelDic[panelName] as T;
            }
            var str = PanelObjPath + panelName;
            GameObject obj = Resources.Load<GameObject>(PanelObjPath + panelName);
            GameObject panel = Instantiate(obj);

            //记录该面板要在哪个层中显示
            Debug.Log(UIRoot);
            Debug.Log(transform.Find("UIRoot"));
            Transform parent = UIRoot;
            //修正面板位置
            panel.transform.SetParent(transform, false);

            //在指定的层显示该面板
            panel.transform.SetParent(parent);

            T panelT = panel.GetComponent<T>();
            //记录到字典
            panelDic.Add(panelName, panelT);
            panelT.Show();
            return panelT;
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="task"></param>
        public void HidePanel<T>( Action task = null) where T : PanelBase
        {
            string panelName = typeof(T).Name;
            if (panelDic.ContainsKey(panelName))
            {
                T panelT = panelDic[panelName] as T;
                panelT.Hide(() =>
                {
                   // Destroy(panelT.gameObject);
                    panelDic.Remove(panelName);
                });
                task?.Invoke();
            }
            else
            {
                Debug.LogErrorFormat("{0}面板不存在!", panelName);
            }
        }

        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>() where T : PanelBase
        {
            string panelName = typeof(T).Name;
            if (panelDic.ContainsKey(panelName))
                return panelDic[panelName] as T;
            else
            {
                Debug.LogErrorFormat("{0}面板不存在!", panelName);
                return null;
            }
        }

        /// <summary>
        /// 判断面板是否显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsPanelShowed<T>() where T : PanelBase
        {
            string panelName = typeof(T).Name;
            if (panelDic.ContainsKey(panelName))
                return true;
            return false;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 创建Canvas
        /// </summary>
        //private void CreateOverlayCanvas()
        //{
        //    if (FindObjectOfType<Canvas>()) return;

        //    //修改Layer
        //    gameObject.layer = LayerMask.NameToLayer("UI");

        //    //添加并设置Canvas组件
        //    Canvas canvas = gameObject.AddComponent<Canvas>();
        //    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //    canvas.sortingOrder = 30000;

        //    //添加并设置CanvasScaler组件
        //    CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
        //    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //    canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        //    canvasScaler.matchWidthOrHeight = Screen.width > Screen.height ? 1 : 0;

        //    //添加并设置GraphicRayCaster组件
        //    GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

        //    //添加子物体 作为显示层的父物体
        //    //Rearmost层的父物体
        //    GameObject rearmost = new GameObject(rearmostName);
        //    rearmost.transform.SetParent(transform, false);

        //    //Rear层的父物体
        //    GameObject rear = new GameObject(rearName);
        //    rear.transform.SetParent(transform, false);

        //    //Middle层的父物体
        //    GameObject middle = new GameObject(middleName);
        //    middle.transform.SetParent(transform, false);

        //    //Front层的父物体
        //    GameObject front = new GameObject(frontName);
        //    front.transform.SetParent(transform, false);

        //    //Forefront层的父物体
        //    GameObject forefront = new GameObject(forefrontName);
        //    forefront.transform.SetParent(transform, false);
        //}

        /// <summary>
        /// 创建EventSystem
        /// </summary>
        private void CreateEventSystem()
        {
            if (FindObjectOfType<EventSystem>()) return;

            GameObject eventSystem = new GameObject("EventSystem");

            //切换场景不销毁
            DontDestroyOnLoad(eventSystem);

            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
 
        #endregion
        
    }



}
