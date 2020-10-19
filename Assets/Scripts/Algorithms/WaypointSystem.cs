using UnityEditor;
using UnityEngine;

namespace Algorithms {
    public class WaypointSystem : MonoBehaviour {
        public Transform[] nodes;

        [Header("Gizmo Settings")] public Color waypointGizmoColor = Color.red;

        [Range(0.1f, 2f)] public float waypointGizmoRadius = 1;

        public bool drawWaypointPositions = true;

        public Color waypointNodeColor = Color.blue;
        public bool drawWaypointNodes = true;

        public bool drawGizmosOnlyWhenSelected;

        private void OnDrawGizmos(){
            if (!drawGizmosOnlyWhenSelected)
                DrawWaypointGizmos();
        }

        private void OnDrawGizmosSelected(){
            if (drawGizmosOnlyWhenSelected)
                DrawWaypointGizmos();
        }

        private void DrawWaypointGizmos(){
            for (int i = 0; i < nodes.Length; i++) {
                if (nodes[i] && drawWaypointPositions) {
                    Gizmos.color = waypointGizmoColor;
                    Gizmos.DrawSphere(nodes[i].position, waypointGizmoRadius);
                }

                if (i < nodes.Length - 1 && nodes[i] && nodes[i + 1] && drawWaypointNodes) {
                    Gizmos.color = waypointNodeColor;
                    Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
                }
            }
        }

        public void GetNearestPoint(Vector3 position, out Transform node, out int index){
            node = null;
            index = -1;

            if (nodes.Length == 0)
                return;

            float distance = Mathf.Infinity;

            for (int i = 0; i < nodes.Length; i++) {
                float nodeDist = Vector3.Distance(position, nodes[i].position);
                if (nodeDist < distance) {
                    distance = nodeDist;
                    index = i;
                    node = nodes[i];
                }
            }
        }

        public static void GenerateNodes(WaypointSystem waypointSystem){
            waypointSystem.nodes = new Transform[waypointSystem.transform.childCount];
            for (int i = 0; i < waypointSystem.nodes.Length; i++) {
                waypointSystem.nodes[i] = waypointSystem.transform.GetChild(i);
                waypointSystem.nodes[i].name = "Node " + (i + 1);
            }
        }

#if DEBUG


        [CustomEditor(typeof(WaypointSystem))]
        [CanEditMultipleObjects]
        public class WaypointSystemEditor : Editor {
            public override void OnInspectorGUI(){
                base.OnInspectorGUI();
                Rect r = EditorGUILayout.BeginHorizontal("Button");
                if (GUI.Button(r, GUIContent.none))
                    GenerateNodes((WaypointSystem) serializedObject.targetObject);
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("Generate Nodes", GUI.skin.label);
                EditorGUILayout.EndHorizontal();
            }
        }
#endif
    }
}