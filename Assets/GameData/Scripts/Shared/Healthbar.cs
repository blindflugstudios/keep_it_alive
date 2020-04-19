using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour _entity;

        private IEntity Entity => _entity == null ? null :
         _entity.GetComponent<Player.Player>() is IEntity ? _entity.GetComponent<Player.Player>() as IEntity : null;

        private float _initialHealth;

        private float _healthBarLength;

        void Start () 
        {
            if(_entity == null && Entity == null)
            {
                Debug.Log("Please attach IEntity");
                return;
            }
          
            _initialHealth = Entity.Health;
            _healthBarLength = Screen.width / 6;
        }

        private void OnGUI() 
        {
            GUI.contentColor = Color.black;

            if(Entity != null && Entity.Health >= 0)
            {
                GUI.Box(new Rect(700, 10, _healthBarLength, 20), $"{_initialHealth.ToString()} / {Entity.Health.ToString()} ");
            }
        }
    }
}