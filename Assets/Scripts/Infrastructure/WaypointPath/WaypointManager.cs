using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    public List<Transform> _pathPoints = new List<Transform>();

    [SerializeField]
    private List<Transform> _PathPoints => _pathPoints;

    [SerializeField]
    private Color _pathColor = Color.green;

    [SerializeField]
    private GameObject _pointPrefab;

    [SerializeField]
    [Range(0f, 1f)]
    public float _handleSize = 0.1f;

    [SerializeField]
    public int _gizmoSegments = 20;

    private void OnDrawGizmos()
    {
        if (_pathPoints.Count > 0)
        {
            for (int i = 0; i < _pathPoints.Count - 1; i++)
            {
                if (_pathPoints[i] != null && _pathPoints[i + 1] != null)
                {
                    DrawBezier(_pathPoints[i], _pathPoints[i + 1], i);
                }
            }
        }
    }

    private void DrawBezier(Transform _startPoint, Transform _endPoint, int _index)
    {
        Vector3 _startPosition = _startPoint.position;
        Vector3 _endPosition = _endPoint.position;

        Vector3 _startTangent = _startPosition + _startPoint.forward * Vector3.Distance(_startPosition, _endPosition) * _handleSize;
        Vector3 _endTangent = _endPosition - _endPoint.forward * Vector3.Distance(_startPosition, _endPosition) * _handleSize;

        Gizmos.color = Color.green;
        Vector3 _previousPosition = _startPosition;
        for (int i = 1; i <= _gizmoSegments; i++)
        {
            float t = i / (float)_gizmoSegments;
            Vector3 _currentPosition = CalculateBezierPoint(t, _startPosition, _endPosition, _startTangent, _endTangent);
            Gizmos.DrawLine(_previousPosition, _currentPosition);
            _previousPosition = _currentPosition;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_startPosition, _startTangent);
        Gizmos.DrawLine(_endPosition, _endTangent);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public void AddPoint()
    {
        GameObject _newPoint = Instantiate(_pointPrefab, this.transform);
        _newPoint.name = "Point " + _pathPoints.Count;

        Transform _pointTransform = _newPoint.GetComponent<Transform>();
        _pathPoints.Add(_pointTransform);

        if (_pathPoints.Count > 2)
            _pathPoints[_pathPoints.Count - 1].position = _pathPoints[_pathPoints.Count - 2].position;

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

        #if UNITY_EDITOR
        Selection.activeGameObject = _newPoint;
        #endif
    }

    public void ClearPoints()
    {
        foreach (Transform _point in _pathPoints)
        {
            if (_point != null)
            {
                DestroyImmediate(_point.gameObject);
            }
        }
        _pathPoints.Clear();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaypointManager _waypointManager = (WaypointManager)target;

        if (GUILayout.Button("Add Point"))
        {
            _waypointManager.AddPoint();
        }

        if (GUILayout.Button("Clear Points"))
        {
            _waypointManager.ClearPoints();
        }
    }
}
#endif