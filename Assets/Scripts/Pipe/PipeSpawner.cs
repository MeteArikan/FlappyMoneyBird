using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _pipesPrefab;

    [Header("Settings")]
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private float _height = 0.5f;

    private void Start()
    {
        SpawnPipes();
    }

    private IEnumerator SpawnPipesCoroutine()
    {
        while (true)
        {
            Instantiate(_pipesPrefab, new Vector3(3f, Random.Range(-_height, _height), 0f), Quaternion.identity);
            yield return new WaitForSeconds(_spawnInterval);
        }

    }

    private void SpawnPipes()
    {
        StartCoroutine(SpawnPipesCoroutine());
    }
}
