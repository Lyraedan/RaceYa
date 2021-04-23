using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        public CameraSettings set;
        private float x;
        private float y;

        bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }
        bool skipControl;

        // Start is called before the first frame update
        void Start()
        {
            if (Mathf.Abs(set.distance) <= 0f) set.distance = Vector3.Distance(transform.position, set.target.position);

            x = set.startAngleX;
            y = set.startAngleY;

            UpdateCamera();
        }

        void Update()
        {
            if (set.target == null) return;

            if (Input.GetKeyDown(KeyCode.Escape)) skipControl = !skipControl;
            if (skipControl) return;

            if (!set.allowPlayerControl)
            {
                UpdateCamera();
                return;
            }

            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
            {
                set.distance -= Input.mouseScrollDelta.y;
                set.distance = Mathf.Clamp(set.distance, set.allowDistance.x, set.allowDistance.y);
            }

            if (!Input.GetMouseButton(1))
            {
                UpdateCamera();
                return;
            }

            var cx = Input.GetAxis("Mouse X");
            var cy = Input.GetAxis("Mouse Y");

            UpdatePosition(cx, cy);
            UpdateCamera();
        }

        internal void UpdatePosition(float xOffset, float yOffset)
        {
            if (set.onlyUpdateInWindow && !IsMouseOverGameWindow) return;

            if (set.target == null) return;
            if (Mathf.Abs(xOffset) <= set.deadZone.x && Mathf.Abs(yOffset) <= set.deadZone.y) return;

            Vector2 offset = new Vector2(
                xOffset * set.moveSpeed.x * (set.inverseX ? -1f : 1f),
                yOffset * set.moveSpeed.y * (set.inverseY ? -1f : 1f));

            x += offset.x;
            y += offset.y;

            if (x < 0f) x += 360f;
            if (x > 360f) x -= 360f;
            if (set.restrictVerticalMovement && y < set.setAngles.x) y = set.setAngles.x;
            if (set.restrictVerticalMovement && y > set.setAngles.y) y = set.setAngles.y;
        }

        public void UpdateCamera()
        {
            if (set.target == null) return;

            var home = set.target.transform.position - (transform.forward * 0.5f) + Vector3.up;
            transform.rotation = Quaternion.Euler(y, x, 0);
            var targetPosition = transform.rotation * new Vector3(0f, 0.5f, -set.distance) + set.target.position + new Vector3(0, set.heightOffset, 0);
            var dir = targetPosition - home;
            dir.y = 0;
            var distance = Vector3.Distance(home, targetPosition);
            if (distance > set.distance) distance = set.distance;

            if (set.checkForObstruction)
            {
                if (Physics.Raycast(new Vector3(set.target.position.x, targetPosition.y, set.target.position.z), dir, out RaycastHit hit, distance))
                    targetPosition = hit.point - (dir * 0.05f);
            }

            if (targetPosition.y < -5f) 
                targetPosition.y = -5f;

            transform.position = targetPosition;
        }


        [System.Serializable]
        public class CameraSettings
        {
            public Transform target;
            public float distance = 12;
            public float heightOffset = 2;

            //Setup start view
            public float startAngleX = 0;
            public float startAngleY = 8;
            public bool restrictVerticalMovement = true;
            public Vector2Int setAngles = new Vector2Int(-45, 45);
            public Vector2Int allowDistance = new Vector2Int(5, 15);
            public Vector2 deadZone = new Vector2(0.05f, 0.05f);
            public Vector2 moveSpeed = new Vector2(2, 1);

            public bool allowPlayerControl = true;
            public bool onlyUpdateInWindow;
            public bool checkForObstruction = true;
            public bool stopOnObstruction = true;
            public bool inverseX;
            public bool inverseY = true;
        }
    }
