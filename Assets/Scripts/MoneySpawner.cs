using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private float _height = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void OnEnable()
    {
        PipeSpawner.OnPipeSpawned += SpawnMoney;
    }


    private void SpawnMoney(float pipeHeight)
    {
        // if (pipeHeight < -1.5)
        // {
        //     Instantiate(_moneyPrefab, new Vector3(5.7f, UnityEngine.Random.Range(_height - 0.5f, _height + 0.5f), 0f), Quaternion.identity);
        // }
        // else if (pipeHeight > 1.5)
        // {
        //     Instantiate(_moneyPrefab, new Vector3(5.7f, UnityEngine.Random.Range(-_height, -_height + 1f), 0f), Quaternion.identity);
        // }
        // else
        // {
        //     Instantiate(_moneyPrefab, new Vector3(5.7f, UnityEngine.Random.Range(-_height, _height), 0f), Quaternion.identity);
        // }

        bool moneySpawnHeight = Random.value > 0.5f;
        if (moneySpawnHeight)
        {
            Instantiate(_moneyPrefab, new Vector3(5.7f, _height, 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(_moneyPrefab, new Vector3(5.7f, -_height, 0f), Quaternion.identity);
        }



        Debug.Log("Money Spawner Called");
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     _moneyPrefab.transform.position += 2.5f * Time.deltaTime * Vector3.left;
    // }
    
    private void OnDisable()  {
        PipeSpawner.OnPipeSpawned -= SpawnMoney;
    }
}
