using Game.ECS.Components.Lifter;
using Game.ECS.Components.Movement;
using Svelto.DataStructures;
using UnityEngine;

namespace GameUnitTest.SystemTest.Lifter
{
    public class SystemTestLifterImplementor : MonoBehaviour, ILifter, IMovement2D, IPosition2D
    {
        public FasterList< int> CarriedThings { get; } = new FasterList< int>();

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
            // for the gameplay. As excercis you could try to create this engine and the
            // necessay components

            time += Time.deltaTime;
            while (time > Mathf.PI * 2)
                time -= Mathf.PI * 2;

            destPos = new Vector2( 0, Mathf.Sin( time));
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
