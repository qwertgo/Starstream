using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathSection : MonoBehaviour
{
    [SerializeField] TransitionDirection transitionDirection;
 
    public TransitionDirection GetDirection() => transitionDirection;
    Transform objectsFolder;

    private void Start() {
        objectsFolder = transform.Find("Objects");
        //AddComponents();
    }
    void AddComponents()
    {
        for(int i = 0; i < objectsFolder.childCount; i++)
        {
            Rigidbody rb;
            MeshCollider meshCollider;
            GameObject child = objectsFolder.GetChild(i).gameObject;
            if (child.GetComponent<Rigidbody>() == false)
                child.AddComponent<Rigidbody>();
            rb = child.GetComponent<Rigidbody>();
            rb.useGravity = false;
                
            if (child.GetComponent<MeshCollider>() == false)
                child.AddComponent<MeshCollider>();
            meshCollider = child.GetComponent<MeshCollider>();
            meshCollider.convex = true;

            if(child.GetComponent<DestructableObject>() == false)
                child.AddComponent<DestructableObject>();
        }
    }
}
