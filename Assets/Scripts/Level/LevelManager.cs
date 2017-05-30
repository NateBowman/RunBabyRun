//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="LevelManager.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Level
{
    using System.Collections;

    using Events;

    using NateTools.Attributes;
    using NateTools.Utils;

    using Terrain;

    using UnityEngine;

    using Utility;

    /// <summary>
    ///     Manages the generated level
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public int ChunkCacheSize;

        public GameObject ChunkPrefab;

        public WrappingQueue<TerrainArea> Chunks;

        public GameObject PlayerPrefab;

        public GameObject StartChunkPrefab;

        private int chunkCounter = 0;

        private TerrainArea currentChunk;

        private bool hasInit = false;

        private GameStats stats = new GameStats();

        public int CurrentSectionIndex { get { return Chunks.IndexOf(currentChunk); } }

        private Vector3 NextChunkPosition { get { return Chunks[0].EndLocation.EndPosition; } }

        [Button]
        public void StartGame()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            stats = new GameStats();
            Chunks = new WrappingQueue<TerrainArea>(ChunkCacheSize);

            // Spawn start chunk
            var chunk = (Instantiate(StartChunkPrefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<TerrainArea>();
            chunk.PositionStartAt(Vector3.zero);
            Chunks.Add(chunk);
            currentChunk = chunk;

            GameObject spawn = null;

            yield return new WaitWhile(() => (spawn = GameObject.Find("InitialSpawnPoint")) == null);

            // Spawn player
            if (spawn != null)
            {
                var player = Instantiate(PlayerPrefab, spawn.transform.position, Quaternion.identity) as GameObject;
                var cam = FindObjectOfType<CameraController>();

                if (cam != null)
                {
                    cam.MainTarget = player;
                }
            }

            // Spawn chunks up to the ChunkCacheSize
            for (var i = 1; i < ChunkCacheSize; i++)
            {
                SpawnAChunk();
            }

            hasInit = true;
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ChunkEntered>(SetCurrentChunk);
            EventManager.RemoveListener<ScoredPoints>(UpdateScore);
            EventManager.RemoveListener<PlayerJumped>(UpdateJumps);
            EventManager.RemoveListener<PlayerDied>(SetFinalChunk);
        }

        private void OnEnable()
        {
            EventManager.AddListener<ChunkEntered>(SetCurrentChunk);
            EventManager.AddListener<ScoredPoints>(UpdateScore);
            EventManager.AddListener<PlayerJumped>(UpdateJumps);
            EventManager.AddListener<PlayerDied>(SetFinalChunk);
        }

        private void SetCurrentChunk(GameEvent go)
        {
            var chunk = go.Sender.GetComponent<Chunk>();
            if (chunk)
            {
                currentChunk = chunk;
            }
        }

        private void SetFinalChunk(GameEvent arg0)
        {
            int id;
            if (int.TryParse(currentChunk.name.Split(' ')[0], out id))
            {
                stats.FinalChunk = id;
            }

            Debug.Log("Ending Game");

            EventManager.Raise(new EndGame(gameObject, stats));
        }

        private void SpawnAChunk()
        {
            var c = (Instantiate(ChunkPrefab, Vector2.up * 500f, Quaternion.identity) as GameObject).SetName(string.Format("id : {0}", chunkCounter)).GetComponent<Chunk>();
            c.CreateTerrain(Chunk.GenerateProductionString(3));
            c.PositionStartAt(NextChunkPosition);
            chunkCounter++;
            Chunks.Add(c);
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!hasInit)
            {
                return;
            }

            // if the current section index > ChunkCacheSize / 2f -> Destroy the last chunk, spawn a new one
            if (CurrentSectionIndex < (ChunkCacheSize / 2f))
            {
                var first = Chunks[Chunks.Count - 1];
                Destroy(first.gameObject);
                SpawnAChunk();
            }
        }

        private void UpdateJumps(GameEvent arg0)
        {
            stats.Jumps++;
        }

        private void UpdateScore(GameEvent gameEvent)
        {
            var e = gameEvent as ScoredPoints;
            if (e != null)
            {
                stats.Score += e.Value;
                EventManager.Raise(new ScoreChanged(gameObject, stats.Score));
            }
        }
    }
}