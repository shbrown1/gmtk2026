using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private int objectAmount = 20;
    [SerializeField] private TMP_Text objectAmountText;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Vector3 spawnOrigin;
    [SerializeField] private Vector3 spawnOffset = Vector3.right;
    [SerializeField] private Vector3 spawnRotation;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject circuitBoard;
    [SerializeField] private AudioClip musicSound;
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private ConveyorBeltTimer timer;


    private List<GameObject> _pendingObjects = new List<GameObject>();
    private List<GameObject> _completedObjects = new List<GameObject>();
    private int _remainingCount;

    void Start()
    {
        _remainingCount = objectAmount;
        if (objectAmountText != null) objectAmountText.text = _remainingCount.ToString();

        for (int i = 0; i < objectAmount - 1; i++)
        {
            Vector3 pos = transform.TransformPoint(spawnOrigin + spawnOffset * i);
            GameObject obj = Instantiate(objectPrefab, pos, Quaternion.identity);
            obj.transform.SetParent(transform, worldPositionStays: true);
            obj.transform.localRotation = Quaternion.Euler(spawnRotation);

            foreach (var col in obj.GetComponentsInChildren<Collider>())
                Destroy(col);

            foreach (var mono in obj.GetComponentsInChildren<MonoBehaviour>())
                Destroy(mono);

            _pendingObjects.Add(obj);
        }
        //sorry this is lazy I can't be bothered to do this in a smarter way
        Vector3 dynamitePos = transform.TransformPoint(spawnOrigin + spawnOffset * objectAmount);
        GameObject dynamiteObj = Instantiate(dynamitePrefab, dynamitePos, Quaternion.Euler(spawnRotation));
        dynamiteObj.transform.SetParent(transform, worldPositionStays: true);
        dynamiteObj.transform.localRotation = Quaternion.Euler(spawnRotation);

        foreach (var col in dynamiteObj.GetComponentsInChildren<Collider>())
            Destroy(col);

        foreach (var mono in dynamiteObj.GetComponentsInChildren<MonoBehaviour>())
            Destroy(mono);

        _pendingObjects.Add(dynamiteObj);

        AudioController.instance.PlayBackgroundMusic(musicSound, 0.7f);
    }

    public void GizmoCompleted(Gzimo gizmo)
    {
        StartCoroutine(GizmoCompletedSequence(gizmo.transform));
    }

    private IEnumerator GizmoCompletedSequence(Transform t)
    {
        _remainingCount--;
        if (objectAmountText != null) objectAmountText.text = _remainingCount.ToString();

        Quaternion rotStart = t.rotation;
        Quaternion rotEnd = Quaternion.Euler(270f, 0f, 180f);
        float elapsed = 0f;
        float resetRotationDuration = 0.2f;
        while (elapsed < resetRotationDuration)
        {
            elapsed += Time.deltaTime;
            t.rotation = Quaternion.Slerp(rotStart, rotEnd, elapsed / resetRotationDuration);
            yield return null;
        }
        t.rotation = rotEnd;

        yield return new WaitForSeconds(.6f);

        t.SetParent(transform, worldPositionStays: true);

        Vector3 posStart = t.position;
        Vector3 posEnd = transform.TransformPoint(spawnOrigin - spawnOffset);
        Quaternion slideRotStart = t.rotation;
        Quaternion slideRotEnd = transform.rotation * Quaternion.Euler(spawnRotation);
        elapsed = 0f;
        float slideIntoDuration = 0.2f;
        while (elapsed < slideIntoDuration)
        {
            elapsed += Time.deltaTime;
            float pct = elapsed / slideIntoDuration;
            t.position = Vector3.Lerp(posStart, posEnd, pct);
            t.rotation = Quaternion.Slerp(slideRotStart, slideRotEnd, pct);
            yield return null;
        }
        t.position = posEnd;
        t.rotation = slideRotEnd;

        _completedObjects.Insert(0, t.gameObject);

        yield return new WaitForSeconds(0.3f);

        // Advance all belt objects together
        var allObjects = new List<GameObject>(_completedObjects);
        allObjects.AddRange(_pendingObjects);

        Vector3 worldOffset = transform.TransformVector(-spawnOffset);
        float advanceDuration = 0.5f;
        Vector3[] advanceStarts = new Vector3[allObjects.Count];
        for (int i = 0; i < allObjects.Count; i++)
            advanceStarts[i] = allObjects[i].transform.position;

        elapsed = 0f;
        while (elapsed < advanceDuration)
        {
            elapsed += Time.deltaTime;
            float pct = Mathf.SmoothStep(0f, 1f, elapsed / advanceDuration);
            for (int i = 0; i < allObjects.Count; i++)
                allObjects[i].transform.position = advanceStarts[i] + worldOffset * pct;
            yield return null;
        }
        for (int i = 0; i < allObjects.Count; i++)
            allObjects[i].transform.position = advanceStarts[i] + worldOffset;

        // Grab the next unworked object
        if (_pendingObjects.Count == 1)
        {
            
            objectPrefab = dynamitePrefab;
            //yield break;
        }
        GameObject next = _pendingObjects[0];
        _pendingObjects.RemoveAt(0);

        Transform rotatableObject = FindAnyObjectByType<RotatableObject>().transform;

        rotatableObject.rotation = Quaternion.identity;
        next.transform.SetParent(rotatableObject, worldPositionStays: true);

        Vector3 liftPosStart = next.transform.localPosition;
        Quaternion liftRotStart = next.transform.localRotation;
        Quaternion liftRotEnd = Quaternion.Euler(270f, 0f, 180f);
        elapsed = 0f;
        var liftDuration = 0.2f;
        while (elapsed < liftDuration)
        {
            elapsed += Time.deltaTime;
            float pct = Mathf.SmoothStep(0f, 1f, elapsed / liftDuration);
            next.transform.localPosition = Vector3.Lerp(liftPosStart, Vector3.zero, pct);
            next.transform.localRotation = Quaternion.Slerp(liftRotStart, liftRotEnd, pct);
            yield return null;
        }

        Destroy(next);
        GameObject fresh = Instantiate(objectPrefab, rotatableObject);

        if (_pendingObjects.Count == 0)
        {
            timer.StopTimer();
            yield break;
        } 

        Instantiate(battery);
        Instantiate(circuitBoard);
        fresh.transform.localEulerAngles = new Vector3(270f, 0f, 180f);
        fresh.transform.localPosition = Vector3.zero;
    }

    public int GetRemainingCount()
    {
        return _remainingCount;
    }
}
