using Domain.Manipulations;
using Util;
using Object = UnityEngine.Object;

namespace Components.Manipulations;

public class ManipulationCapturer : MonoBehaviour
{
    public List<Object> Observers = [];
    public float DeadRadius = 0.1f;

    private Camera _mainCamera;
    private Vector2 _lastPosition;
    private Manipulation _ongoingManipulation;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        var position = GetPosition();

        _lastPosition = position;
        _ongoingManipulation = new Manipulation(position);

        foreach (var observer in Observers)
        {
            if (observer == null)
                continue;

            ((IManipulationObserver)observer).OnManipulationBegun(_ongoingManipulation.State);
        }
    }

    private void OnMouseDrag()
    {
        if (_ongoingManipulation == null)
            return;

        var position = GetPosition();
        if ((position - _lastPosition).sqrMagnitude <= DeadRadius * DeadRadius)
            return;

        var manipulationDelta = _ongoingManipulation.Advance(position);
        _lastPosition = position;

        foreach (var observer in Observers)
        {
            if (observer == null)
                continue;

            ((IManipulationObserver)observer).OnManipulationAdvanced(manipulationDelta);
        }
    }

    private void OnMouseUp()
    {
        if (_ongoingManipulation == null)
            return;

        foreach (var observer in Observers)
        {
            if (observer == null)
                continue;

            ((IManipulationObserver)observer).OnManipulationFinished(_ongoingManipulation.State);
        }

        _ongoingManipulation = null;
    }

    private Vector2 GetPosition()
    {
        var mousePosition = Input.mousePosition;
        var worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        var localPosition = transform.InverseTransformPoint(worldPosition);

        return localPosition;
    }
}