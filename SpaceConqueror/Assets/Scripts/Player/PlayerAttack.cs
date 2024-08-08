using NnUtils.Scripts;
using Projectile;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : NnBehaviour
    {
        [SerializeField] private ProjectileScript _projectilePrefab;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
            var projectile = Instantiate(_projectilePrefab, transform);
            projectile.transform.SetParent(null);
        }
    }
}