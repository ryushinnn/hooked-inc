using UnityEngine;

public abstract class MovableObject : MonoBehaviour {
    [SerializeField] protected float VelocityMin, VelocityMax;

    protected Vector3 Destination;

    public abstract void SetDestination(Vector3 dest, bool fixedVel);
    public abstract void OnCaught();
    protected abstract void OnDisappear();

    protected float GetVelocity(bool fixedVel) {
        return fixedVel ? (VelocityMin + VelocityMax) / 2 : Random.Range(VelocityMin, VelocityMax);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, Destination);
    }
#endif
}