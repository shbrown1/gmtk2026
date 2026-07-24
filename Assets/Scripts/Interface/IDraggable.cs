using UnityEngine;

public interface IDraggable
{
    void BeginDrag();
    void UpdateDrag(Vector2 mousePosition);
    void EndDrag();
}
