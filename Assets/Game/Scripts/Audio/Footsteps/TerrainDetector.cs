using UnityEngine;

namespace Game.Audio
{
    public class TerrainDetector
    {
        private readonly float[,,] _splatmapData;
        private readonly int _numTextures;

        public TerrainDetector()
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            int alphamapWidth = terrainData.alphamapWidth;
            int alphamapHeight = terrainData.alphamapHeight;

            _splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            _numTextures = _splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        private static Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
        {
            var terrain = Terrain.activeTerrain;
            var splatPosition = new Vector3();
            Vector3 terrainPosition = terrain.transform.position;

            TerrainData terrainData = terrain.terrainData;
            splatPosition.x = (worldPosition.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth;
            splatPosition.z = (worldPosition.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight;

            return splatPosition;
        }

        public int GetActiveTerrainTextureIndex(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
            int activeTerrainIndex = 0;
            float largestOpacity = 0f;

            for (int i = 0; i < _numTextures; i++)
            {
                if (largestOpacity < _splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
                {
                    activeTerrainIndex = i;
                    largestOpacity = _splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
                }
            }

            return activeTerrainIndex;
        }
    }
}