using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private float _height = 0.5f;
   private void OnEnable()
    {
        PipeSpawner.OnPipeSpawned += SpawnMoney;
    }


    private void SpawnMoney(float pipeHeight)
    {
        bool moneySpawnHeight = Random.value > 0.5f;
        if (moneySpawnHeight)
        {
            Instantiate(_moneyPrefab, new Vector3(5.7f, _height, 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(_moneyPrefab, new Vector3(5.7f, -_height, 0f), Quaternion.identity);
        }
    }

    
    private void OnDisable()  {
        PipeSpawner.OnPipeSpawned -= SpawnMoney;
    }
}
