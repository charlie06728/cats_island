using Terrain;
using UnityEngine;

namespace DefaultNamespace.Sound {
    public class FootStepManager : MonoBehaviour {
        /* Singleton */
        public static FootStepManager Instance { get; private set; }
        
        public string switchGroup = "sfx_footsteps"; // Wwise Switch Group Name
        public string footstepEvent = "Play_Footstep"; // Wwise Footstep Event Name

        public GameObject player;

        protected uint playID = 0;
        protected string terrainType = null;
        
        private UnityEngine.Terrain terrain;
        
        protected void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            terrain = TerrainManager.Instance.Terrain;
        }

        public void PlayFootstep()
        {
            string terrainTypeNow = GetTerrainTexture();
            if (terrainTypeNow == terrainType && playID != 0) return;

            terrainType = terrainTypeNow;
            SetFootstepSwitch(terrainType);

            // playID = footstepEvent.Post(gameObject);
        }
        
        public void StopFootstep()
        {
            if (playID != 0) {
                playID = 0;
                terrainType = null;
                AkSoundEngine.StopAll(gameObject);
            }
        }
        
        private void SetFootstepSwitch(string surfaceType)
        {
            // Set the correct Wwise switch
            AkSoundEngine.SetSwitch(switchGroup, surfaceType, gameObject);

            // Play the footstep event
            playID = AkSoundEngine.PostEvent(footstepEvent, gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnSoundEnd, null);
        }
        
        void OnSoundEnd(object in_cookie, AkCallbackType in_type, object in_info)
        {
            terrainType = GetTerrainTexture();
            SetFootstepSwitch(terrainType);
        }
        
        private string GetTerrainTexture() {
            // UnityEngine.Terrain terrain = TerrainManager.Instance.Terrain;
            TerrainData terrainData = TerrainManager.Instance.Terrain.terrainData;
            Vector3 playerPos = transform.position;
            float[,,] splatmapData = terrainData.GetAlphamaps(
                (int)((playerPos.x - terrain.transform.position.x) / terrainData.size.x * terrainData.alphamapWidth),
                (int)((playerPos.z - terrain.transform.position.z) / terrainData.size.z * terrainData.alphamapHeight),
                1, 1);

            float maxVal = 0;
            int maxIndex = 0;
            for (int i = 0; i < splatmapData.GetLength(2); i++)
            {
                if (splatmapData[0, 0, i] > maxVal)
                {
                    maxVal = splatmapData[0, 0, i];
                    maxIndex = i;
                }
            }

            // Define terrain textures manually (must match Unity terrain layers)
            string[] terrainTextures = { "Grass", "Sand", "Grass", "Sand" };
            return terrainTextures[maxIndex];
        }
    }
}