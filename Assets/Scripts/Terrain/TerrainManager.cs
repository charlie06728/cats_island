using UnityEngine;

namespace Terrain {
    public class TerrainManager : MonoBehaviour {
        /* Singleton pattern */
        public static TerrainManager Instance { get; private set; }
        public UnityEngine.Terrain Terrain;

        protected void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}