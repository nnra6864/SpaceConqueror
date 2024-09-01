using System.Collections;
using System.Collections.Generic;
using Core;
using NnUtils.Scripts;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawnerScript : NnBehaviour
    {
        private static readonly WaitForSeconds UpdateInterval = new(0.1f); //Cached the update interval
        private static PlayerScript Player => GameManager.Player;

        [SerializeField] private BomberScript _enemyPrefab;
        [SerializeField] private Vector2Int _spawnAmount = new(3, 10);
        [SerializeField] private Vector2 _spawnDistance = new(3, 15);

        [HideInInspector] public List<BomberScript> Enemies = new();

        private void Start() => StartCoroutine(UpdateRoutine());

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                if (!Player ||
                    Enemies.Count > 3
                    ) { yield return UpdateInterval; continue; }
                
                yield return SpawnRoutine();
                yield return UpdateInterval;
            }
        }

        private IEnumerator SpawnRoutine()
        {
            var amount = Random.Range(_spawnAmount.x, _spawnAmount.y);

            for (int i = 0; i < amount; i++)
            {
                if (!Player) yield break;
                var dir = Random.insideUnitCircle.normalized;
                var distance = Random.Range(_spawnDistance.x, _spawnDistance.y);
                var spawnPos = (Vector2)Player.transform.position + dir * distance;
                Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0.25f, 1f));
            }
        }
    }
}