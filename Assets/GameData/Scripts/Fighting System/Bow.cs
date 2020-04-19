using UnityEngine;
using UnityEngine.InputSystem;

namespace GameData.Scripts.Fighting_System
{
    public class Bow : MonoBehaviour
    {
        public Transform _playerTransform;
        public PlayerInventory _Inventory;
        
        private Camera _mainCam;
        public Arrow arrowPrefab;
        public float arrowSpeed;

        [SerializeField]
        public float coolDownTime = 1;

        private float timeOfLastShot;

        private void Start()
        {
            _mainCam = Camera.main;
        }

        public void UseWeapon(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled && _Inventory.HasTorch == false && Time.time - timeOfLastShot > coolDownTime)
            {
                var shootDirection = CalculateShootDirection();
                timeOfLastShot = Time.time;
                arrowPrefab.ShootArrow(transform.position, arrowSpeed, shootDirection);
            }
        }

        private Vector2 CalculateShootDirection()
        {
            var mouseCurrentPosition = Mouse.current.position.ReadValue();
            Vector2 playerCurrentPosition = _mainCam.WorldToScreenPoint(_playerTransform.position);
            var shootDirection = (mouseCurrentPosition - playerCurrentPosition).normalized;
            
            return shootDirection;


        }
    }
}