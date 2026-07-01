using UnityEngine;

namespace LunaGames.Tools.States {
    public abstract class State
    {
        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnStop();
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnTriggerEnter(Collider other) { }
        public virtual void OnTriggerStay(Collider other) { }
        public virtual void OnTriggerExit(Collider other) { }
        public virtual void OnCollisionEnter(Collision collision) { }
        public virtual void OnCollisionStay(Collision collision) { }
        public virtual void OnCollisionExit(Collision collision) { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }
        public virtual void OnMouseDown() { }
        public virtual void OnMouseDrag() { }
        public virtual void OnMouseEnter() { }
        public virtual void OnMouseExit() { }
        public virtual void OnMouseOver() { }
        public virtual void OnMouseUp() { }
        public virtual void OnMouseUpAsButton() { }
    }
}
