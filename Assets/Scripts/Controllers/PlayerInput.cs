using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask hoverClickMask = ~0;
    private IHoverable currentHover;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Handle Hover Check via Raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hoverClickMask))
        {
            IHoverable hoverObj = hit.collider.GetComponent<IHoverable>();

            if (hoverObj != currentHover)
            {
                if (currentHover != null) currentHover.OnHoverExit();
                currentHover = hoverObj;
                if (currentHover != null) currentHover.OnHoverEnter();
            }

            // Handle Click Check
            if (Input.GetMouseButtonDown(0))
            {
                IClickable clickObj = hit.collider.GetComponent<IClickable>();
                clickObj?.OnClick();
            }
        }
        else if (currentHover != null)
        {
            currentHover.OnHoverExit();
            currentHover = null;
        }
    }
}
