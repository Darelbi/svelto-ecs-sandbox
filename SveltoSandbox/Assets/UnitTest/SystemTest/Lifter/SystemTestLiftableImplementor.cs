using Game.ECS.Components;
using Game.ECS.Components.Liftable;
using Game.ECS.Components.Movement;
using UnityEngine;

namespace GameUnitTest.SystemTest.Lifter
{
    public class SystemTestLiftableImplementor : MonoBehaviour, ILiftable, IMovement2D, IPosition2D, IIdentifier
    {
        public int Carrier { get; set; } = -1;
        public int Id { get; set; } = -1;

        private Vector2 movement = Vector2.zero;
        private Vector2 destPos = Vector2.zero;
        private Vector2 oldDestPos = Vector2.zero;
        private float time = 0;

        void Update()
        {
            // NOTE: Don't do that during gameplay!!! it is conceptually wrong
            // I'm doing that here just because this is a SystemTest
            // All logic should be inside engines, actually creating a engine like
            // this is very simple. I don't do that because it would be a useless engine 
            // for the gameplay.As excercis you could try to create this engine and the
            // necessay components

            time += Time.deltaTime;
            while (time > Mathf.PI * 2)
                time -= Mathf.PI * 2;

            destPos = new Vector2( Mathf.Sin( time), 0);
            movement = destPos - oldDestPos;
            oldDestPos = destPos;
        }


        public Vector2 Movement {
            get
            {
                return movement;
            }

            set
            {
                movement = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }
    }
}
