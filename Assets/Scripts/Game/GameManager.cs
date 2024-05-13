using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum EnemyCountPerWave
    {
        WaveOne = 1,
        WaveTwo = 2,
        WaveThree = 2
    }

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy_basic;
    [SerializeField] private GameObject enemy_medium;
    [SerializeField] private GameObject enemy_hard;
    [SerializeField] private int totalWaves = 3;
    private int _waveIndex = 0;
    private int _currentEnemyShips = 0;

    private float _waveDelay = 5.0f;

    [SerializeField] private GameObject pauseMenu = null;
    private bool isPaused = false;
    private Animator _pauseAnimator;


    void Start()
    {
        StartCoroutine(DelayNextWave());
        PlayerShip.Instance.OnPlayerDestroyed += HandlePlayerDestroyed;
        _pauseAnimator = pauseMenu.GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P)) && !isPaused)
        {
            _pauseAnimator.SetBool("isPaused", true);
            SetActivePauseMenu(true);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P) && isPaused)
        {
            _pauseAnimator.SetBool("isPaused", false);
            SetActivePauseMenu(false);
        }
    }

    public void SetActivePauseMenu(bool state)
    {
        Time.timeScale = state ? 0 : 1;
        isPaused = state;
    }

    void StartNextWave()
    {
        int enemyCount = (int)EnemyCountPerWave.GetValues(typeof(EnemyCountPerWave)).GetValue(_waveIndex);
        _currentEnemyShips = enemyCount;

        PlayerShip.Instance.health = (int)PlayerShip.Instance.DefaultHealth;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject[] enemyTypes = new GameObject[] { enemy_basic, enemy_medium, enemy_hard };
        GameObject enemyClone = null;

        if (_waveIndex >= 0 && _waveIndex < enemyTypes.Length)
        {
            enemyClone = Instantiate(enemyTypes[_waveIndex]);
        }
        else
        {
            enemyClone = Instantiate(enemyTypes[2]);
        }

        if (enemyClone != null)
        {
            enemyClone.GetComponent<EnemyShip>().OnEnemyDestroyed += CheckWaveCompletion;
        }
    }

    void CheckWaveCompletion()
    {
        _currentEnemyShips--;
        bool isWaveComplete = _currentEnemyShips == 0;

        if (isWaveComplete)
        {
            _waveIndex++;

            if (_waveIndex >= totalWaves)
            {
                PlayerPrefs.SetInt("GameResult", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("EndScene");
            }
            else
            {
                StartCoroutine(DelayNextWave());
            }
        }
    }

    IEnumerator DelayNextWave()
    {
        yield return new WaitForSeconds(_waveDelay);
        StartNextWave();
    }

    void HandlePlayerDestroyed()
    {
        PlayerPrefs.SetInt("GameResult", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndScene");
    }

    public void ResumeButton()
    {
        SetActivePauseMenu(false);
    }

    public static void RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public static void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public static void QuitButton()
    {
        Application.Quit();
    }
}
