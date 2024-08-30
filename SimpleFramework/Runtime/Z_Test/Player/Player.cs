using UnityEngine;

namespace SimpleFramework.Test 
{
    public class Player : MonoBehaviour
    {
        public Animator animator
        {
            get;
            private set;
        }

        public PlayerStateMachine StateMachine
        {
            get;
            private set;
        }

        public PlayerIdleState IdleState
        {
            get;
            private set;
        }

        public PlayerMoveState MoveState
        {
            get;
            private set;
        }

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");

            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.currentState.Update();
        }
    }
}