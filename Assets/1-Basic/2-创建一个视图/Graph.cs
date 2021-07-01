using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;

    Transform[] points;

    private void Awake() {
        points = new Transform[resolution];
        float step = 4f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            position.x = ((i + 1f) * step - 2f);
            //position.y = position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update() {
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            //position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            position.y = Mathf.Sin(position.x);
            point.localPosition = position;
        }
    }

}
