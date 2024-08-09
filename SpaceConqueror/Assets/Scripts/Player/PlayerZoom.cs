using System.Collections;
using Core;
using NnUtils.Scripts;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerZoom : NnBehaviour
    {
        private static CinemachineCamera CineCam => GameManager.CineCam;
        
        [SerializeField] private float _zoomStep = 20f;
        [SerializeField] private float _zoomTime = 1;
        [SerializeField] private Vector2 _zoomMinMax = new(5, 10);

        private float _zoom;
        public float Zoom
        {
            get => _zoom;
            private set
            {
                if (_zoom == value) return;
                _zoom = Mathf.Clamp(value, _zoomMinMax.x, _zoomMinMax.y); 
                RestartRoutine(ref _zoomRoutine, ZoomRoutine());
            }
        }

        private void Start() => _zoom = CineCam.Lens.OrthographicSize;
        private void Update() => Zoom -= Input.GetAxisRaw("Mouse ScrollWheel") * _zoomStep;

        private Coroutine _zoomRoutine;
        private IEnumerator ZoomRoutine()
        {
            var startZoom = CineCam.Lens.OrthographicSize;
            float lerpPos = 0;

            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, _zoomTime, false, Easings.Types.ExpoOut);
                CineCam.Lens.OrthographicSize = Mathf.LerpUnclamped(startZoom, Zoom, t);
                yield return null;
            }

            _zoomRoutine = null;
        }
    }
}