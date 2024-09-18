using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Code.Runtime.Core.Utils;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.AI.Sensor
{
    public class VisionSensor : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;
        [SerializeField] private float _height;
        [SerializeField] private float _scanDelay;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private LayerMask _obstacleLayer;

        [Header("Debug")] [SerializeField] private bool _showFov;
        [ShowIf(nameof(_showFov))] [SerializeField] private Color _meshColor;

        private Collider[] _colliders = new Collider[Constants.DefaultCollectionCapacity];
        private HashSet<GameObject> _previouslySeenObjects = new();
        private List<GameObject> _visibleTargets = new();

        private CancellationTokenSource _cts = new();
        private Mesh _mesh;
        private int _count;

        public event System.Action TargetSighted;
        public event System.Action TargetLost;
        
        public IReadOnlyList<GameObject> VisibleTargets => _visibleTargets;

        private void OnValidate()
        {
            _mesh = CreateWedgeMesh();
        }

        private void Start() => Scan().Forget();

        private async UniTask Scan()
        {
            while (!_cts.IsCancellationRequested)
            {
                _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _targetLayer,
                    QueryTriggerInteraction.Collide);

                var currentlySeenObjects = new HashSet<GameObject>();

                for (int i = 0; i < _count; i++)
                {
                    var obj = _colliders[i].gameObject;
                    if (IsInSight(obj))
                    {
                        currentlySeenObjects.Add(obj);
                        _visibleTargets.Add(obj);
                        if (!_previouslySeenObjects.Contains(obj))
                        {
                            TargetSighted?.Invoke();
                        }
                    }
                }

                foreach (var obj in _previouslySeenObjects)
                {
                    if (!currentlySeenObjects.Contains(obj))
                    {
                        _visibleTargets.Remove(obj);
                        TargetLost?.Invoke();
                    }
                }

                _previouslySeenObjects = currentlySeenObjects;
                await UniTask.Delay(TimeSpan.FromSeconds(_scanDelay));
            }
        }

        private bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;

            if (direction.y < 0 || direction.y > _height)
                return false;

            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);

            if (deltaAngle > _angle)
                return false;

            origin.y += _height / 2;
            dest.y = origin.y;

            if (Physics.Linecast(origin, dest, _obstacleLayer))
                return false;

            return true;
        }

        private void OnDestroy() => _cts.Cancel();

        private void OnDrawGizmos()
        {
            if(!_showFov)
                return;
            
            if (_mesh)
            {
                Gizmos.color = _meshColor;
                Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
            }

            for (int i = 0; i < _count; i++) 
                Gizmos.DrawSphere(_colliders[i].transform.position, .2f);

            Gizmos.color = Color.green;
            foreach (var obj in _visibleTargets) 
                Gizmos.DrawSphere(obj.transform.position, .2f);
        }

        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();

            int segments = 10;
            int numTriangles = (segments * 4) + 2 + 2;
            int numVertices = numTriangles * 3;

            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
            Vector3 bottomRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

            Vector3 topCenter = bottomCenter + Vector3.up * _height;
            Vector3 topRight = bottomRight + Vector3.up * _height;
            Vector3 topLeft = bottomLeft + Vector3.up * _height;

            int vert = 0;

            //left
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;
            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            //right
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;
            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -_angle;
            float deltaAngle = (_angle * 2) / segments;
            for (int i = 0; i < segments; i++)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;
                topRight = bottomRight + Vector3.up * _height;
                topLeft = bottomLeft + Vector3.up * _height;

                //far
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;
                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                //top
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                //bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;

                currentAngle += deltaAngle;
            }

            for (int i = 0; i < numVertices; ++i)
                triangles[i] = i;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}