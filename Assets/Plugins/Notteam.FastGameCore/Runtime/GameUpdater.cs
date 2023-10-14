using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Notteam.FastGameCore
{
    [DefaultExecutionOrder(0)]
    public class GameUpdater : MonoBehaviour
    {
        private bool _collectedAllSystems;
        private bool _createdGameSceneBridge;
        
        private GameSceneBridge _sceneBridge;
        
        private List<GameUpdaterSystem> _updateSystems = new();

        public GameSceneBridge SceneBridge => _sceneBridge;
        public static GameUpdater Instance { get; private set; }

        private void CreateGameSceneBridge()
        {
            _sceneBridge = new GameObject("GameSceneBridge", typeof(GameSceneBridge)).GetComponent<GameSceneBridge>();
            
            _createdGameSceneBridge = true;
            
            Debug.Log($"Created Scene Bridge : {_sceneBridge}");
        }

        private void LoadSceneInitialization()
        {
            CreateGameSceneBridge();
            
            // load logic...
        }
        
        private void UnloadSceneInitialization()
        {
            // unload logic...
        }

        private void CollectUpdateSystems()
        {
            _updateSystems = GetComponentsInChildren<GameUpdaterSystem>().ToList();

            _collectedAllSystems = true;
        }
        
        private void Initialize()
        {
            CollectUpdateSystems();
            
            SceneManager.sceneLoaded += (_, _) =>
            {
                LoadSceneInitialization();
            };

            SceneManager.sceneUnloaded += (_) =>
            {
                UnloadSceneInitialization();
            };
        }
        
        private void SetOrDestroyInstanceAndInitialize()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                
                Initialize();
                
                DontDestroyOnLoad(this);
            }
        }
        
        private void Awake()
        {
            SetOrDestroyInstanceAndInitialize();
        }

        private void FixedUpdate()
        {
            if (_collectedAllSystems & _createdGameSceneBridge)
            {
                foreach (var system in _updateSystems)
                    system.OnUpdateFixedInternal();
            }
        }
        
        private void Update()
        {
            if (_collectedAllSystems & _createdGameSceneBridge)
            {
                foreach (var system in _updateSystems)
                    system.OnUpdateInternal();
            }
        }

        private void LateUpdate()
        {
            if (_collectedAllSystems & _createdGameSceneBridge)
            {
                foreach (var system in _updateSystems)
                    system.OnUpdateLateInternal();
            }
        }

        internal T GetUpdateSystem<T>() where T : GameUpdaterSystem
        {
            var system = default(T);
            
            foreach (var updateSystem in _updateSystems)
            {
                if (updateSystem.GetType() == typeof(T))
                    system = updateSystem as T;
            }

            return system;
        }
        
        internal void AddToUpdateSystem(GameUpdaterComponent component)
        {
            for (int i = 0; i < _updateSystems.Count; i++)
            {
                var system = _updateSystems[i];

                if (component.GameUpdateSystemType == system.GetType())
                    _updateSystems[i].AddToReadyList(component);
            }
        }
        
        internal void RemoveFromUpdateSystem(GameUpdaterComponent component)
        {
            for (int i = 0; i < _updateSystems.Count; i++)
            {
                var system = _updateSystems[i];

                if (component.GameUpdateSystemType == system.GetType())
                    _updateSystems[i].RemoveFromReadyList(component);
            }
        }
    }
}
