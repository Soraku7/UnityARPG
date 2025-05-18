using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RHFrame 
{
    /// <summary>
    /// UI面板基类
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class PanelBase : MonoBehaviour
    {
        #region 字段
        [SerializeField, Header("淡入淡出的速度")] protected float fadeSpeed = 2.0f;
        [SerializeField, Header("动画类型")] protected PanelAni aniType = PanelAni.None;

        private CanvasGroup _canvasGroup;
        private bool _isShow;//是否显示
        private Action _action;

        //面板组件缓存
        private Dictionary<string, Component> cacheCompents = new Dictionary<string, Component>();
        #endregion

        #region 生命周期

        protected virtual void Awake() 
        {
            _canvasGroup = GetComponent<CanvasGroup>();           
        }

        private void Start() 
        {
            Init();
        }

        protected virtual void Update() 
        {
        }

        protected virtual void OnDisable() 
        {
            cacheCompents.Clear();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化面板
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="ani"></param>
        public void Show()
        {
            _isShow = true;
            ShowOrHideAni();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="task"></param>
        public void Hide(Action task)
        {
            _isShow = false;
            if (task != null)
                _action = task;
            ShowOrHideAni();
        }

        private void ShowOrHideAni()
        {
            if (_isShow)
            { 
                switch (aniType)
                {
                    case PanelAni.None:
                        //无效果
                        break;
                    case PanelAni.Fade:
                        //淡入淡出
                        break;
                    case PanelAni.Pop:
                        //弹出
                        break;
                }
            }
            else
            {
                //隐藏动画结束后调用回调
                Destroy(this.gameObject);
                _action?.Invoke();
            }
        }

        /// <summary>
        /// 获取当前面板下的指定组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childObjectName"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected T GetController<T>(string childObjectName, Transform parent = null) where T : Component
        {
            parent = parent ?? this.transform;
            if (cacheCompents.ContainsKey(childObjectName))
                return cacheCompents[childObjectName] as T;

            Transform childTransform = parent.Find(childObjectName);

            if (childTransform != null)
            {
                T component = childTransform.GetComponent<T>();
                if (component != null)
                {
                    if (!cacheCompents.ContainsKey(childObjectName))
                    {
                        cacheCompents.Add(childObjectName, component);
                    }
                    return component;
                }
            }

            //第一层没有找到指定的子对象 继续递归扫描
            int childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                //检测当前父对象是不是有子对象，没有直接跳出
                if (parent.GetChild(i) == null) continue;

                T component = GetController<T>(childObjectName, parent.GetChild(i));
                if (component != null)
                {
                    if (!cacheCompents.ContainsKey(childObjectName))
                    {
                        cacheCompents.Add(childObjectName, component);
                    }
                    return component;
                }
            }

            //到这里没找到那就是没有
            return null;
        }

        #endregion
    }

    /// <summary>
    /// UI面板动画类型
    /// </summary>
    public enum PanelAni 
    {
        None,//无效果
        Fade,//淡入淡出
        Pop//弹出
    }
}

