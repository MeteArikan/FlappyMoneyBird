using System;
using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public static event Action<float> OnPipeSpawned;
    [Header("References")]
    [SerializeField] private GameObject _pipesPrefab;

    [Header("Settings")]
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private float _height = 0.5f;

    private Coroutine _spawnCoroutine;


    private void Start()
    {
        //SpawnPipes();
        GameManager.OnGameStarted += SpawnPipes;
        GameManager.OnAfterGameOver += StopSpawningPipes;
    }

    private void SpawnPipes()
    {
        _spawnCoroutine = GameModeController.Instance.GetGameMode() == GameMode.MoneyMode ?
            StartCoroutine(MoneyModeSpawnPipesCoroutine()) : StartCoroutine(SpawnPipesCoroutine());
    }


    private IEnumerator SpawnPipesCoroutine()
    {
        while (true)
        {
            Instantiate(_pipesPrefab, new Vector3(3f, UnityEngine.Random.Range(-_height, _height), 0f), Quaternion.identity);
            //Instantiate(_pipesPrefab, new Vector3(3f,  -_height, 0f), Quaternion.identity);
            Debug.Log("Spawned Pipes");
            yield return new WaitForSeconds(_spawnInterval);
        }

    }

    private IEnumerator MoneyModeSpawnPipesCoroutine()
    {
        GameObject _pipeObject;
        while (true)
        {
            bool spawnPipeAtTop = UnityEngine.Random.value > 0.5f;
            if (spawnPipeAtTop)
            { 
                _pipeObject = Instantiate(_pipesPrefab, new Vector3(3f, UnityEngine.Random.Range(_height - 2.5f, _height), 0f), Quaternion.identity);
            }
            else
            {
                _pipeObject = Instantiate(_pipesPrefab, new Vector3(3f, UnityEngine.Random.Range(-_height, -_height + 2.5f), 0f), Quaternion.identity);
            }
            //_pipeObject = Instantiate(_pipesPrefab, new Vector3(3f, UnityEngine.Random.Range(-_height, _height), 0f), Quaternion.identity);
            //Instantiate(_pipesPrefab, new Vector3(3f,  -_height, 0f), Quaternion.identity);
            Debug.Log("Spawned Pipes in Money Mode");
            yield return new WaitForSeconds(_spawnInterval / 2f);
            CallMoneySpawner(_pipeObject.transform.position.y);
            //Invoke(nameof(CallMoneySpawner), _spawnInterval / 2f);
            yield return new WaitForSeconds(_spawnInterval / 2f);
        }
            
        
    }

    private void StopSpawningPipes()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }
    
    private void CallMoneySpawner(float height)
    {
        OnPipeSpawned?.Invoke(height);
    }


    private void OnDestroy()
    {
        GameManager.OnGameStarted -= SpawnPipes;
        GameManager.OnAfterGameOver -= StopSpawningPipes;
    }
}
