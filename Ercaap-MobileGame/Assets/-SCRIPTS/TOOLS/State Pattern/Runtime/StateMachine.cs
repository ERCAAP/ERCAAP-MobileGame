using Sirenix.OdinInspector;
using UnityEngine;

namespace LunaGames.Tools.States
{
    public class StateMachine : MonoBehaviour
    {
        public string StateName => currentState?.GetType().Name;
        private string CurrentStateName => currentState != null ? currentState?.GetType().Name : "Empty";
        private string PrevStateName => previousState != null ? previousState?.GetType().Name : "Empty";

        [BoxGroup("State Machine"), Title("Current State", "$CurrentStateName"), ShowInInspector, ReadOnly, HideLabel] public State currentState;
        [BoxGroup("State Machine"), Title("$PrevStateName"), ShowInInspector, ReadOnly, HideLabel, FoldoutGroup("State Machine/Previous State")] public State previousState;

        public void ChangeState(State newState)
        {
            if (currentState == newState) return;
            currentState?.OnStop();
            previousState = currentState;
            currentState = newState;
            currentState?.OnStart();
        }
        public void RevertState()
        {
            currentState?.OnStop();
            currentState = previousState;
            currentState?.OnStart();
        }
        private void Update() => currentState?.OnUpdate();
        private void FixedUpdate() => currentState?.OnFixedUpdate();
        private void LateUpdate() => currentState?.OnLateUpdate();

        #region Object Interactions
        private void OnTriggerEnter(Collider other) => currentState?.OnTriggerEnter(other);
        private void OnTriggerStay(Collider other) => currentState?.OnTriggerStay(other);
        private void OnTriggerExit(Collider other) => currentState?.OnTriggerExit(other);
        private void OnCollisionEnter(Collision collision) => currentState?.OnCollisionEnter(collision);
        private void OnCollisionStay(Collision collision) => currentState?.OnCollisionStay(collision);
        private void OnCollisionExit(Collision collision) => currentState?.OnCollisionExit(collision);
        #endregion

        #region Mouse Functions
        private void OnMouseDown() => currentState?.OnMouseDown();
        private void OnMouseDrag() => currentState?.OnMouseDrag();
        private void OnMouseEnter() => currentState?.OnMouseEnter();
        private void OnMouseExit() => currentState?.OnMouseExit();
        private void OnMouseOver() => currentState?.OnMouseOver();
        private void OnMouseUp() => currentState?.OnMouseUp();
        private void OnMouseUpAsButton() => currentState?.OnMouseUpAsButton();
        #endregion

        private void OnDisable() => currentState?.OnDisable();
        private void OnDestroy() => currentState?.OnDestroy();
    }
}

