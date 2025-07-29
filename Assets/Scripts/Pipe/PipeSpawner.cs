using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
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
        _spawnCoroutine = StartCoroutine(SpawnPipesCoroutine());
    }


    private IEnumerator SpawnPipesCoroutine()
    {
        while (true)
        {
            Instantiate(_pipesPrefab, new Vector3(3f, Random.Range(-_height, _height), 0f), Quaternion.identity);
            Debug.Log("Spawned Pipes");
            yield return new WaitForSeconds(_spawnInterval);
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


    private void OnDestroy() {
        GameManager.OnGameStarted -= SpawnPipes;
        GameManager.OnAfterGameOver-= StopSpawningPipes;
    }
}
