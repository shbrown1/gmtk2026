using UnityEngine;

public class CuttableWire : MonoBehaviour
{
    public WireColor color;
    public bool isCut { get; private set;} = false;

    public void CutWire(bool isCorrectWire)
    {
        if (isCut) return;

        isCut = true;
        UpdateModel();
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
