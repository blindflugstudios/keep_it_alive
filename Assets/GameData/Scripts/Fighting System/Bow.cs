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

        private void Start()
        {
            _mainCam = Camera.main;
        }

        public void UseWeapon(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started)
            {
                return;
            }
            Arrow arrow;

            var shootDirection = CalculateShootDirection();
        
            arrowPrefab.ShootArrow(transform.position, arrowSpeed, shootDirection);
            
//            throw new System.NotImplementedException();
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