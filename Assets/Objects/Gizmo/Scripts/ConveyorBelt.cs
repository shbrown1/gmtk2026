using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private int objectAmount = 20;
    [SerializeField] private TMP_Text objectAmountText;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Vector3 spawnOrigin;
    [SerializeField] private Vector3 spawnOffset = Vector3.right;
    [SerializeField] private Vector3 spawnRotation;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < objectAmount; i++)
        {
            Vector3 pos = transform.TransformPoint(spawnOrigin + spawnOffset * i);
            GameObject obj = Instantiate(objectPrefab, pos, Quaternion.identity);
            obj.transform.SetParent(transform, worldPositionStays: true);
            obj.transform.localRotation = Quaternion.Euler(spawnRotation);

            foreach (var col in obj.GetComponentsInChildren<Collider>())
                Destroy(col);

            foreach (var mono in obj.GetComponentsInChildren<MonoBehaviour>())
                Destroy(mono);

            _spawnedObjects.Add(obj);
        }
    }
}
