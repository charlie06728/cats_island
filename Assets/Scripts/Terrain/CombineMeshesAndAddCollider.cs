using UnityEngine;

namespace Terrain {
    public class CombineMeshesAndAddCollider : MonoBehaviour {
        void Start()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<MeshFilter>() && !child.GetComponent<MeshCollider>())
                {
                    child.gameObject.AddComponent<MeshCollider>();
                }
            }
        }
    }
}