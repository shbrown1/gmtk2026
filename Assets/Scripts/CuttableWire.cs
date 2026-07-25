using UnityEngine;

public class CuttableWire : MonoBehaviour, IClickable
{
    public WireColor color;
    public bool isCut { get; private set;} = false;
    [SerializeField] private AudioClip snippingClip;
    private Dynamite dynamite;

    void Start()
    {
        dynamite = GetComponentInParent<Dynamite>();
        Debug.Log(dynamite);
    }
    private void CutWire()
    {
        if (isCut) return;
        AudioController.instance.PlaySound(snippingClip);

        isCut = true;
        UpdateModel();
        dynamite.OnWireCut(this);
    }

    public void OnClick()
    {
        CutWire();
    }

    private void UpdateModel()
    {
        //update to cut wire model
    }
}


public enum WireColor
{
    Red,
    Blue,
    Green,
    Purple,
}
