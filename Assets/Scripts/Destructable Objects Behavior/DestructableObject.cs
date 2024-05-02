using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class DestructableObject : MonoBehaviour
{
    public enum CollisionType {Static, Moveable}
    [SerializeField]CollisionType collisionType = CollisionType.Moveable;
    [SerializeField]UnityEvent onCrashEvent;
    [SerializeField]VisualEffect crashVFX;
    Collider collider;
    Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    public void OnCrash(Vector3 collisionPoint)
    {
        Debug.Log($"Crashed into {gameObject.name}");

        if (collider is MeshCollider)
        {
            MeshCollider meshCollider = (MeshCollider)collider;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = true;
        }
        onCrashEvent?.Invoke();
        VisualEffect vfx = Instantiate(crashVFX, collisionPoint, transform.rotation);
        switch (collisionType)
        {
            case CollisionType.Static:
                vfx.transform.localScale *=250;
                break;
            case CollisionType.Moveable:
                vfx.transform.localScale *=10;
                break;
        }
    }
}