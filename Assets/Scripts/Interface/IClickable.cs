using UnityEngine;

public interface IClickable
{
    void OnClick()
    {
        Debug.Log($"Clicked on");
    }
}
