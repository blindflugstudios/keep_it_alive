using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField]
        [RequireInterface(typeof(IEntity))]
        private Object _entity;

        public IEntity Entity => _entity as IEntity;

        private float _health;

        private float _healthBarLength;

        void Start () 
        {
            _health = Entity.Health;
            _healthBarLength = Screen.width / 6;
        }

        private void OnGUI() 
        {
            GUI.Box(new Rect(700, 10, _healthBarLength, 20), _health.ToString());
        }
    }
}