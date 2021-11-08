using UnityEngine;

namespace WizardParty.Core
{
    public class SimpleMove : MonoBehaviour
    {
        [SerializeField]
        private float speed = 3;
        public void MoveUp()
            => MoveToawrds(Vector3.up);
        public void MoveToawrds(Vector2 direction)
            => Move(direction * speed);
        public void Move(Vector2 speedVector)
            => Move(speedVector, Time.deltaTime);
        public void Move(Vector2 speedVector, float deltaTime)
        {
            transform.position += (Vector3)speedVector * deltaTime;
        }
    }
}
