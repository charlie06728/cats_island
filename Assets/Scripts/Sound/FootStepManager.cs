using System;
using Terrain;
using UnityEngine;

namespace DefaultNamespace.Sound {
    public class FootStepManager : MonoBehaviour {
        /* Singleton */
        public static FootStepManager Instance { get; private set; }
        
        [NonSerialized] public string switchGroup = "sfx_footsteps"; // Wwise Switch Group Name
        [NonSerialized] public string footstepEvent = "Play_sfx_Footsteps"; // Wwise Footstep Event Name
        public AK.Wwise.Event fe;
        public AK.Wwise.Switch fs_wood;
        public AK.Wwise.Switch fs_sand;
        public AK.Wwise.Switch fs_grass;

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
            SetFootstepSwitch();

            // playID = footstepEvent.Post(gameObject);
        }
        
        public void StopFootstep()
        {
            if (playID != 0) {
                playID = 0;
                terrainType = null;
                // AkUnitySoundEngine.StopAll(gameObject);
                fe.Stop(gameObject);
            }
        }
        
        private void SetFootstepSwitch()
        {
            // Set the correct Wwise switch
            switch (terrainType) {
                case "wood":
                    fs_wood.SetValue(gameObject);
                    break;
                case "sand":
                    fs_sand.SetValue(gameObject);
                    break;
                case "grass":
                    fs_grass.SetValue(gameObject);
                    break;
                default:
                    fs_wood.SetValue(gameObject);
                    break;
            }
            // AkUnitySoundEngine.SetSwitch(switchGroup, surfaceType, gameObject);

            // Play the footstep event
            // playID = AkUnitySoundEngine.PostEvent(footstepEvent, gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnSoundEnd, null);
            playID = fe.Post(gameObject, (uint)AkCallbackType.AK_EndOfEvent, OnSoundEnd, null);
        }
        
        void OnSoundEnd(object in_cookie, AkCallbackType in_type, object in_info)
        {
            if (playID == 0 || terrainType == null) return;
            terrainType = GetTerrainTexture();
            SetFootstepSwitch();
        }
        
        private string GetTerrainTexture() {
            // UnityEngine.Terrain terrain = TerrainManager.Instance.Terrain;
            TerrainData terrainData = TerrainManager.Instance.Terrain.terrainData;
            Vector3 playerPos = Server.Server.Instance.player.transform.position;
            
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
            string[] terrainTextures = { "grass", "sand", "grass", "wood" };
            Debug.Log(terrainTextures[maxIndex]);
            return terrainTextures[maxIndex];
        }
    }
}