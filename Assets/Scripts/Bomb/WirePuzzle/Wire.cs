using Unity.VisualScripting;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public bool isWireToCut = false;
    public bool isWireCut = false;

    void OnMouseDown()
    {
        if (isWireCut == false)
        {
            isWireCut = true;

            if (isWireToCut)
            {
                Debug.Log("correct wire cut");
            }
            else
            {
                Debug.Log("something terrible has happened");
            };  
        }
    }
}
