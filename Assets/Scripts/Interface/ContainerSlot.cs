using System.Collections.Generic;
using UnityEngine;

public class ContainerSlot : MonoBehaviour
{
    [SerializeField] private string acceptedType = "Default";
    [SerializeField] private Transform slotTransform; // where filler snaps to; defaults to this transform
    [SerializeField] private float catchRadius = 1.5f;
    [SerializeField] private AudioClip soundEffect;

    public static readonly List<ContainerSlot> All = new List<ContainerSlot>();

    public string AcceptedType => acceptedType;
    public Transform SlotTransform => slotTransform != null ? slotTransform : transform;
    public float CatchRadius => catchRadius;
    public bool IsOccupied => CurrentFiller != null;
    public FillerObject CurrentFiller { get; private set; }

    private void OnEnable() => All.Add(this);
    private void OnDisable() => All.Remove(this);

    public bool TryAccept(FillerObject filler)
    {
        if (IsOccupied || filler.FillerType != acceptedType) return false;
        CurrentFiller = filler;
        AudioController.instance.PlaySound(soundEffect);
        return true;
    }

    public void RemoveFiller()
    {
        CurrentFiller = null;
    }
}