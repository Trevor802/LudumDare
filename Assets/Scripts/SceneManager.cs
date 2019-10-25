using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VII
{
    public enum SceneType { MainScene, GameScene, RestartScene, WinScene };

    [System.Serializable]
    public class MusicOfScene
    {
        public SceneType SceneType;
        public AudioClip MusicClip;
    }

    public class SceneManager : MonoBehaviour
    {
        #region Singleton
        public static SceneManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(transform.gameObject);
        }
        #endregion
        public List<MusicOfScene> music;
        private Dictionary<SceneType, AudioClip> musicOfScenes;

        private void Start()
        {
            musicOfScenes = new Dictionary<SceneType, AudioClip>()
            {
                {SceneType.MainScene, null },
                {SceneType.GameScene, null },
                {SceneType.RestartScene, null },
                {SceneType.WinScene, null }
            };
            foreach (var m in music)
            {
                musicOfScenes[m.SceneType] = m.MusicClip;
            }
        }

        public void LoadScene(SceneType scene)
        {
            switch (scene)
            {
                case SceneType.MainScene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
                    break;
                case SceneType.GameScene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
                    break;
                case SceneType.RestartScene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("RestartScene");
                    break;
                case SceneType.WinScene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayMusic(musicOfScenes[scene]);
        }
    }
}

