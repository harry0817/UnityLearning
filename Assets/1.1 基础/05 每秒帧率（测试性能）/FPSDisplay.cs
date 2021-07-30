using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

    [System.Serializable]
    private struct FPSColor {
        public Color color;
        public int minimunFPS;
    }

    public Text highestFPSLabel;
    public Text averageFPSLabel;
    public Text lowestFPSLabel;
    [SerializeField]
    private FPSColor[] coloring;

    FPSCounter fpsCounter;

    private void Awake() {
        fpsCounter = GetComponent<FPSCounter>();
    }

    private void Update() {
        Display(averageFPSLabel, fpsCounter.AverageFPS);
        Display(highestFPSLabel, fpsCounter.HighestFPS);
        Display(lowestFPSLabel, fpsCounter.LowestFPS);
    }

    void Display(Text label, int fps) {
        label.text = Mathf.Clamp(fps, 0, 99).ToString();
        for (int i = 0; i < coloring.Length; i++) {
            if (fps >= coloring[i].minimunFPS) {
                label.color = coloring[i].color;
                break;
            }
        }
    }

}
