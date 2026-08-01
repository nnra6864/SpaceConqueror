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
        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
            _cineCam = FindAnyObjectByType<CinemachineCamera>(FindObjectsInactive.Include);
            _enemySpawner = FindAnyObjectByType<EnemySpawnerScript>(FindObjectsInactive.Include);
            _pickupLT = FindAnyObjectByType<PickupLootTableScript>(FindObjectsInactive.Include);
            _stats = FindAnyObjectByType<StatsScript>(FindObjectsInactive.Include);
            _menu = FindAnyObjectByType<MainMenu>(FindObjectsInactive.Include);
            _menu.Show();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Playing) Pause();
                else Resume();
            }
        }

        private static GameObject GameObject => Instance.gameObject;

        private Camera _camera;
        public static Camera Camera => Instance._camera;

        private CinemachineCamera _cineCam;
        public static CinemachineCamera CineCam => Instance._cineCam;

        private PlayerScript _player;
        public static PlayerScript Player => Instance._player;
        public static Action<PlayerScript> OnPlayerRegistered;
        public static Action OnPlayerUnregistered;

        [SerializeField] private PlayerScript _playerPrefab;

        private bool _playing;
        public static bool Playing => Instance._playing;

        private bool _paused = true;
        public static bool Paused => Instance._paused;

        public static void Play()
        {
            if (Playing) return;

            Menu.Hide();
            GameObject.Instantiate(Instance._playerPrefab);
            Stats.Show();

            Instance._playing = true;
            Instance._paused = false;
        }

        public static void Resume()
        {
            if (!Playing) return;
            if (!Paused) return;

            Menu.Hide();
            Stats.Show();

            Instance._paused = false;
        }

        public static void Pause()
        {
            if (!Playing) return;
            if (Paused) return;

            Stats.Hide();
            Menu.Show();

            Instance._paused = true;
        }

        public static void Stop()
        {
            if (!Playing) return;

            Stats.Hide();
            Menu.Show();

            Instance._playing = false;
            Instance._paused = true;
        }

        public static void RegisterPlayer(PlayerScript player)
        {
            Instance._player = player;
            CineCam.Follow = player.transform;
            OnPlayerRegistered?.Invoke(Player);
        }

        public static void UnregisterPlayer()
        {
            Instance._player = null;
            GameManager.CineCam.Follow = null;
            OnPlayerUnregistered?.Invoke();
        }

        private EnemySpawnerScript _enemySpawner;
        public static EnemySpawnerScript EnemySpawner => Instance._enemySpawner;

        private PickupLootTableScript _pickupLT;
        public static PickupLootTableScript PickupLT => Instance._pickupLT;

        private MainMenu _menu;
        public static MainMenu Menu => Instance._menu;

        private StatsScript _stats;
        public static StatsScript Stats => Instance._stats;
    }
}
