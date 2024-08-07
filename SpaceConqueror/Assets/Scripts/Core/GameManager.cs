using NnUtils.Scripts;
using Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }
                _instance = value;
            }
        }
        private void Awake() => Instance = GetComponent<GameManager>();
    
        private static GameObject GO => Instance.gameObject;

        [SerializeField] private TimeManager _timeManager;
        public static TimeManager TimeManager =>
            Instance._timeManager ?? (Instance._timeManager = GO.GetOrAddComponent<TimeManager>());

        [SerializeField] private Camera _camera;
        public static Camera Camera => Instance._camera == null ? Instance._camera = Camera.main : Instance._camera;

        [SerializeField] private CinemachineCamera _cineCam;
        public static CinemachineCamera CineCam
        {
            get => Instance._cineCam ?? (Instance._cineCam = FindFirstObjectByType<CinemachineCamera>());
            set
            {
                if (Instance._cineCam == value) return;
                if (Instance._cineCam != null) Destroy(Instance._cineCam);
                Instance._cineCam = value;
            }
        }
    
        [SerializeField] private PlayerScript _player;
        public static PlayerScript Player
        {
            get => Instance._player ?? (Instance._player = FindFirstObjectByType<PlayerScript>());
            set
            {
                if (Instance._player == value) return;
                if (Instance._player != null) Destroy(Instance._player);
                Instance._player = value;
            }
        }
    }
}