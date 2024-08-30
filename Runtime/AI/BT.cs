using UnityEngine;
using System.Collections.Generic;

#region 示例

//        private void OnEnable()
//        {
//            ai = BT.Root();
//            ai.OpenBranch(
//                BT.SetActive(child, true),
//                BT.Call(() => { this.transform.DOMoveX(1, 2); }),
//                BT.Wait(2),
//                BT.Call(() => { Debug.Log("开门： 动画执行完毕"); }),
//                BT.If(IsClose).OpenBranch(
//                     BT.SetActive(child, false),
//                     BT.Call(() => { this.transform.DOMoveX(0, 2); }),
//                     BT.Wait(2),
//                     BT.Call(() => { Debug.Log("关门：动画执行完毕"); })
//                ),
//                BT.Call(() => { Debug.Log("ai已完成"); }),
//                BT.Terminate()
//            );
//        }

//        private void Update()
//        {
//            ai.Tick();
//        }

//        bool IsClose()
//        {
//            return isClose;
//        }
//    }
//}

#endregion

namespace SimpleFramework
{
    /// <summary>
    /// ai进度类型
    /// </summary>
    public enum BTState
    {
        Failure,
        Success,
        Continue,
        Abort
    }

    /// <summary>
    /// ai 模式集合
    /// </summary>
    public static class BT
    {
        public static Root Root() { return new Root(); }

        public static SetActive SetActive(GameObject gameObject, bool active) { return new SetActive(gameObject, active); }

        public static Action Call(System.Action fn) { return new Action(fn); }

        public static Wait Wait(float seconds) { return new Wait(seconds); }

        public static ConditionalBranch If(System.Func<bool> fn) { return new ConditionalBranch(fn); }

        public static While While(System.Func<bool> fn) { return new While(fn); }

        public static Trigger Trigger(Animator animator, string name, bool set = true) { return new Trigger(animator, name, set); }

        public static SetBool SetBool(Animator animator, string name, bool value) { return new SetBool(animator, name, value); }

        public static WaitForAnimatorState WaitForAnimatorState(Animator animator, string name, int layer = 0) { return new WaitForAnimatorState(animator, name, layer); }

        public static Terminate Terminate() { return new Terminate(); }
    }

    /// <summary>
    /// 节点
    /// </summary>
    public abstract class BTNode
    {
        public abstract BTState Tick();
    }

    /// <summary>
    /// 分支
    /// </summary>
    public abstract class Branch : BTNode
    {
        protected int activeChild;
        protected List<BTNode> children = new List<BTNode>();

        public virtual Branch OpenBranch(params BTNode[] children)
        {
            for (int i = 0; i < children.Length; i++)
            {
                this.children.Add(children[i]);
            }
            return this;
        }

        public virtual void ResetChildren()
        {
            activeChild = 0;
            for (var i = 0; i < children.Count; i++)
            {
                Branch b = children[i] as Branch;
                if (b != null)
                {
                    b.ResetChildren();
                }
            }
        }
    }

    /// <summary>
    /// 块
    /// </summary>
    public abstract class Block : Branch
    {
        public override BTState Tick()
        {
            switch (children[activeChild].Tick())
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    return BTState.Continue;
            }
        }
    }

    /// <summary>
    /// ai 根目录
    /// </summary>
    public class Root : Block
    {
        public bool isTerminated = false;

        public override BTState Tick()
        {
            if (isTerminated)
                return BTState.Abort;

            while (true)
            {
                switch (children[activeChild].Tick())
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    case BTState.Abort:
                        isTerminated = true;
                        return BTState.Abort;
                    default:
                        activeChild++;
                        if (activeChild == children.Count)
                        {
                            activeChild = 0;
                            return BTState.Success;
                        }
                        continue;
                }
            }
        }
    }

    /// <summary>
    /// 设置物体显示隐藏节点
    /// </summary>
    public class SetActive : BTNode
    {
        GameObject gameObject;
        bool active;

        public SetActive(GameObject gameObject, bool active)
        {
            this.gameObject = gameObject;
            this.active = active;
        }

        public override BTState Tick()
        {
            gameObject.SetActive(this.active);
            return BTState.Success;
        }
    }

    /// <summary>
    /// 事件调用
    /// </summary>
    public class Action : BTNode
    {
        System.Action fn;

        public Action(System.Action fn)
        {
            this.fn = fn;
        }

        public override BTState Tick()
        {
            fn?.Invoke();
            return BTState.Success;
        }
    }

    /// <summary>
    /// 暂停等待
    /// </summary>
    public class Wait : BTNode
    {
        public float seconds = 0;
        float future = -1;

        public Wait(float seconds)
        {
            this.seconds = seconds;
        }

        public override BTState Tick()
        {
            if (future < 0)
                future = Time.time + seconds;

            if (Time.time >= future)
            {
                future = -1;
                return BTState.Success;
            }
            else
            {
                return BTState.Continue;
            }
        }
    }

    /// <summary>
    /// 块： if条件判断
    /// </summary>
    public class ConditionalBranch : Block 
    {
        public System.Func<bool> fn;
        bool tested = false;

        public ConditionalBranch(System.Func<bool> fn) 
        {
            this.fn = fn;
        }

        public override BTState Tick()
        {
            if (!tested)
            {
                tested = fn();
            }
            if (tested)
            { 
                var result = base.Tick();
                if (result == BTState.Continue)
                {
                    return BTState.Continue;
                }
                else
                {
                    tested = false;
                    return result;
                }
            }
            else
            {
                return BTState.Failure;
            }
        }
    }

    /// <summary>
    /// 块： 当条件为true时，循环执行全部的子物体
    /// </summary>
    public class While : Block
    {
        public System.Func<bool> fn;

        public While(System.Func<bool> fn)
        {
            this.fn = fn;
        }

        public override BTState Tick()
        {
            if (fn())
            {
                base.Tick();
            }
            else
            {
                ResetChildren();
                return BTState.Failure;
            }

            return BTState.Continue;
        }
    }

    /// <summary>
    /// 激活一个动画的触发器
    /// </summary>
    public class Trigger : BTNode
    {
        Animator animator;
        int id;
        string triggerName;
        bool set = true;

        public Trigger(Animator animator, string name, bool set = true)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.triggerName = name;
            this.set = set;
        }

        public override BTState Tick()
        {
            if (set)
            {
                animator.SetTrigger(id);
            }
            else
            {
                animator.ResetTrigger(id);
            }

            return BTState.Success;
        }
    }

    /// <summary>
    /// 为动画设置bool值
    /// </summary>
    public class SetBool : BTNode
    {
        Animator animator;
        int id;
        bool value;
        string triggerName;

        public SetBool(Animator animator, string name, bool value)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.value = value;
            this.triggerName = name;
        }

        public override BTState Tick()
        {
            animator.SetBool(id, value);
            return BTState.Success;
        }
    }

    /// <summary>
    /// 等待动画到达某个状态
    /// </summary>
    public class WaitForAnimatorState : BTNode
    {
        Animator animator;
        int id;
        int layer;
        string stateName;

        public WaitForAnimatorState(Animator animator, string name, int layer = 0)
        {
            this.id = Animator.StringToHash(name);
            if (!animator.HasState(layer, this.id))
            {
                Debug.LogError("The animator does not have state: " + name);
            }
            this.animator = animator;
            this.layer = layer;
            this.stateName = name;
        }

        public override BTState Tick()
        {
            var state = animator.GetCurrentAnimatorStateInfo(layer);
            if (state.fullPathHash == this.id || state.shortNameHash == this.id)
                return BTState.Success;
            return BTState.Continue;
        }
    }

    /// <summary>
    /// 终止节点
    /// </summary>
    public class Terminate : BTNode
    {
        public override BTState Tick()
        {
            return BTState.Abort;
        }
    }
}