// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class SlidingWindow {
//     private int _cap;
//     private int _size;
//     private int _head;
//     private int _tail;
//     private Vector3[] _data;

//     public SlidingWindow(int cap) {
//         _cap = cap;
//         _size = 0;
//         _head = 0;
//         _tail = 0;
//         _data = new Vector3[cap];
//     }
//     public void Push(Vector3 val) {
//         if (_size == _cap) {
//             Pop();
//         }
//         _data[_tail] = val;
//         _tail = (_tail + 1) % _cap;
//         ++ _size;
//     }
//     public Vector3 Pop() {
//         if (_size == 0) {
//             throw new System.Exception("Fuck you");
//         }
//         Vector3 val = _data[_head];
//         _head = (_head + 1) % _cap;
//         -- _size;
//         return val;
//     }

//     // used for negative index
//     private mod(int x, int m) {
//         return (x % m + m) % m;
//     }
//     public Vector3 this[int index]
//     {
//         get { 
//             if (index >= 0) {
//                 return _data[(_head + index) % _cap];
//             } else {
//                 return _data[mod(_tail + index, _cap)];
//             }
//         }
//     }

//     public int Length {
//         get => _cap;
//     }
// }

// public class InfiniteTrack : MonoBehaviour
// {
//     [Tooltip("player transform")]
//     [SerializeField] private Transform player;

//     [Tooltip("size of the window in which the track is generated")]
//     [SerializeField] private float windowSize = 100f;

//     [Tooltip("rough distance between two points")]
//     [SerializeField] private float minDistance = 10f;

//     [Tooltip("next point random radius")]
//     [SerializeField] private float radius = 2.5f;

//     SlidingWindow controlPoints;
    
//     int LogicalLength { get => controlPoints.Length - 1; }

//     Vector3 GetPos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
//         return 0.5f * (
//             (2f * p1) +
//             (-p0 + p2) * t +
//             (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
//             (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
//         );
//     }

//     // get position of the point between logical index and logical index + 1
//     Vector3 GetPos(float t, int logical) {
//         return GetPos(t,
//             controlPoints[logical],
//             controlPoints[logical + 1],
//             controlPoints[logical + 2],
//             controlPoints[logical + 3]
//         );
//     }

//     Vector3 GetTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
//         return 0.5f * (
//             (-p0 + p2) +
//             (4f * p0 - 10f * p1 + 8f * p2 - 2f * p3) * t +
//             (-3f * p0 + 9f * p1 - 9f * p2 + 3f * p3) * t * t
//         );
//     }

//     Vector3 GetTangent(float t, int logical) {
//         return GetTangent(t,
//             controlPoints[logical],
//             controlPoints[logical + 1],
//             controlPoints[logical + 2],
//             controlPoints[logical + 3]
//         );
//     }

//     Vector3 GetControlPoint(int logical) {
//         return controlPoints[logical + 1];
//     }

//     Vector3 GenNextPoint() {
//         var sp = Random.insideUnitCircle * radius;
//         var tan = GetTangent(1f, LogicalLength - 1);
//         var pos = GetControlPoint(LogicalLength - 1) + new Vector3(sp.x, 0f, sp.y) + tan.normalized * minDistance;
//         return pos;
//     }

//     void Awake() {
//         controlPoints = new SlidingWindow(Mathf.CeilToInt(windowSize / minDistance) * 2 + 4);
//         // generate initial points
//         controlPoints.Push(player.position - player.forward * minDistance);
//         controlPoints.Push(player.position);
//         controlPoints.Push(player.position + player.forward * minDistance);
//         controlPoints.Push(player.position + player.forward * minDistance * 2);
//     }
// }
