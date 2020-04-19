using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace GameData.Scripts.Fighting_System
{
    public class Arrow : MonoBehaviour
    {

        [SerializeField]
        private Rigidbody2D rb2d;

        private float _aliveTime = 4;
        private bool stopArrow;

        public static Arrow CreateNewArrow()
        {
            var arrowGameObject = new GameObject("arrow");
            var arrow = arrowGameObject.AddComponent<Arrow>();
            arrow.rb2d = arrowGameObject.AddComponent<Rigidbody2D>();

            return arrow;
        }

        public void ShootArrow(Vector2 startingPosition, float speed, Vector2 direction)
        {
            Arrow arrow = Instantiate(transform, startingPosition, quaternion.identity).GetComponent<Arrow>();
            arrow.StartCoroutine(arrow.FlyCoroutine(speed, direction));
        }

        private IEnumerator FlyCoroutine(float speed, Vector2 direction)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,0,Mathf.Asin(direction.y) * Mathf.Rad2Deg * (direction.x < 0? -1: 1)));
            while (_aliveTime > 0)
            {
                if(!stopArrow)
                {
                    _aliveTime -= Time.deltaTime;
                    speed /= 1.01f;
                    rb2d.MovePosition((Vector2) transform.position + Time.deltaTime * speed * direction);
                }
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    Destroy(gameObject);
                    break;
                case "Obstacle":
                    stopArrow = true;
                    Destroy(GetComponent<BoxCollider2D>());
                    break;
                    
            }
//            throw new NotImplementedException();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                stopArrow = true;
                Destroy(GetComponent<BoxCollider2D>());
            }
        }
    }
}