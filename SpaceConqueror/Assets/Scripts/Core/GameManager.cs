using System;
using Enemies;
using Menu;
using Pickups;
using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityCommunity.UnitySingleton;

namespace Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private static GameObject GameObject => Instance.gameObject;
        public static Action OnSpawned;
        public static Action OnDied;
        
        [SerializeField] private Camera _camera;
        public static Camera Camera => Instance._camera == null ? Instance._camera = Camera.main : Instance._camera;

        [SerializeField] private CinemachineCamera _cineCam;
        public static CinemachineCamera CineCam
        {
            get => Instance._cineCam ?? (Instance._cineCam = FindAnyObjectByType<CinemachineCamera>());
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
            get => Instance._player ?? (Instance._player = FindAnyObjectByType<PlayerScript>());
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
            get => Instance._enemySpawner ?? (Instance._enemySpawner = FindAnyObjectByType<EnemySpawnerScript>());
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
