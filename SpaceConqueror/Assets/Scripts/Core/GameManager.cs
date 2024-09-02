using System;
using Enemies;
using Menu;
using Pickups;
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
                if (_instance != null) return _instance;
                
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance != null)
                {
                    DontDestroyOnLoad(_instance);
                    return _instance;
                }
                
                var go = new GameObject("GameManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<GameManager>();
                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        private static GameObject GameObject => Instance.gameObject;
        public static Action OnSpawned;
        public static Action OnDied;
        
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

        [SerializeField] private EnemySpawnerScript _enemySpawner;
        public static EnemySpawnerScript EnemySpawner
        {
            get => Instance._enemySpawner ?? (Instance._enemySpawner = FindFirstObjectByType<EnemySpawnerScript>());
            set
            {
                if (Instance._enemySpawner == value) return;
                if (Instance._enemySpawner != null) Destroy(Instance._player);
                Instance._enemySpawner = value;
            }
        }

        [SerializeField] private PickupLootTableScript _pickupLT;
        public static PickupLootTableScript PickupLT =>
            Instance._pickupLT ?? (Instance._pickupLT = Instance.GetComponent<PickupLootTableScript>());

        [SerializeField] private MainMenu _menu;
        public static MainMenu Menu => Instance._menu;
    }
}