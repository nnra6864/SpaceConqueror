using Core;
using NnUtils.Scripts;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovementScript : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;
        
        [SerializeField] private float _speed = 25;
        [SerializeField] private float _chaseRange = 10;
        [SerializeField] private float _attackRange = 5;
        [SerializeField] private float _fleeRange = 2;

        private void Update()
        {
            
        }
    }
}