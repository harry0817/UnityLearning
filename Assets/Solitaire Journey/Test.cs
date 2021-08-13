using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : Image {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    protected override void OnPopulateMesh(VertexHelper vh) {
        Debug.Log("vh.currentVertCount: " + vh.currentVertCount);
        if (vh.currentVertCount == 0)
            return;
        UIVertex vertex = new UIVertex();
        vh.PopulateUIVertex(ref vertex, 2);
        Debug.Log("color " + vertex.color + " position " + vertex.position + " uv0 " + vertex.uv0);
    }
}
