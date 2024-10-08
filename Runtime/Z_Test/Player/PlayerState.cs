using UnityEngine;

namespace SimpleFramework.Test 
{
    public class PlayerState
    {
        protected Player player;
        protected PlayerStateMachine stateMachine;

        private string animBoolName;
        public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        {
            this.player = _player;
            this.stateMachine = _stateMachine;
            this.animBoolName = _animBoolName;
        }

        public virtual void Enter()
        {
            Debug.Log("i enter " + animBoolName);
        }

        public virtual void Update()
        {
            Debug.Log("i'm in " + animBoolName);
        }

        public virtual void Exit()
        {
            Debug.Log("i exit " + animBoolName);
        }
    }
}