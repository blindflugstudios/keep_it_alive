using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameData.Scripts.Fighting_System
{
    public class Bow : MonoBehaviour
    {

        public Transform _player;
        
        private Camera _mainCam;
        public Arrow arrowPrefab;
        public float arrowSpeed;
        private bool _equipped = true;

        [SerializeField]
        public float coolDownTime = 1;

        private float timeOfLastShot;

        public bool Equipped
        {
            get => _equipped;
            set
            {
                _equipped = value;
                if (_equipped)
                {
                    EquipWeapon();
                }
                else
                {
                    UnEquipWeapon();
                }
            }
        }

        private void Start()
        {
            _mainCam = Camera.main;
        }

        public void UseWeapon(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled && _equipped && Time.time - timeOfLastShot > coolDownTime)
            {
                Arrow arrow;

                var shootDirection = CalculateShootDirection();
                timeOfLastShot = Time.time;
                arrowPrefab.ShootArrow(transform.position, arrowSpeed, shootDirection);
            }
        }

        public void EquipWeapon()
        {
            throw new System.NotImplementedException();
        }

        public void UnEquipWeapon()
        {
            throw new System.NotImplementedException();
        }

        private Vector2 CalculateShootDirection()
        {
            
            var mouseCurrentPosition = Mouse.current.position.ReadValue();
            Vector2 playerCurrentPosition = _mainCam.WorldToScreenPoint(_player.position);
            var shootDirection = (mouseCurrentPosition - playerCurrentPosition).normalized;
            
            return shootDirection;


        }
    }
}