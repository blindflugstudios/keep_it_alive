using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour _entity;

        public IEntity Entity => _entity.GetComponent<Player.Player>() is IEntity ? _entity.GetComponent<Player.Player>() as IEntity : null;

        private float _healthBarLength;

        void Start () 
        {
            if(Entity == null)
            {
                Debug.Log("Please attach IEntity");
                return;
            }
          
            _healthBarLength = Screen.width / 6;
        }

        private void OnGUI() 
        {
            GUI.Box(new Rect(700, 10, _healthBarLength, 20), Entity.Health.ToString());
        }
    }
}