using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    // Destroy anything that leaves the boundary
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
