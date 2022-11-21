using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Vector3 _firstPressPos;
    Vector3 _secondPressPos;
    Vector3 _currentSwipe;
    Vector3 _mOffset;
    private IClickable _previousHover;
    int _layer;
    void Start()
    {
        _layer = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        // If on Hover Scope(Not press anything)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log(_layer);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
            {
                IClickable clickable = hit.transform.GetComponent<IClickable>();
                if (clickable != _previousHover)
                {
                    if (clickable != null)
                    {
                        clickable.OnMouseEnterHover();
                    }
                    if (_previousHover != null)
                        _previousHover.OnMouseExistHover();
                    _previousHover = clickable;
                }
            }
        }

    }

}
