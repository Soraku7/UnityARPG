using System;
using UnityEngine;

namespace UI
{
    public class MinimapCamera : MonoBehaviour
    {
        private Camera _minimapCamera;
        public RenderTexture MinimapTexture => _minimapCamera.targetTexture;
        
        private GameObject _player;
        private void Awake()
        {
            _minimapCamera = GetComponent<Camera>();
            _player = GameObject.FindWithTag("Player");
        }
        
        private void LateUpdate()
        {
            if (_player == null) return;
            
            //更新小地图位置
            var pos = transform.position;
            pos.x = _player.transform.position.x;
            pos.y = 10;
            pos.z = _player.transform.position.z;
            transform.position = pos;
        }
    }
}
