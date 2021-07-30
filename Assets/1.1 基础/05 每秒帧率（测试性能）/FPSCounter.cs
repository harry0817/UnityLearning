using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

    public int frameRange = 60;

    int[] fpsBuffer;
    int fpsBufferIndex;

    public int AverageFPS {
        get; private set;
    }
    public int HighestFPS {
        get; private set;
    }
    public int LowestFPS {
        get; private set;
    }

    void InitializeBuffer() {
        if (frameRange <= 0) {
            frameRange = 1;
        }
        fpsBuffer = new int[frameRange];
        fpsBufferIndex = 0;
    }

    void UpdateBuffer() {
        fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (fpsBufferIndex >= frameRange) {
            fpsBufferIndex = 0;
        }
    }

    void CalculateFPS() {
        int sum = 0;
        int highest = 0;
        int lowest = int.MaxValue;
        for (int i = 0; i < fpsBuffer.Length; i++) {
            sum += fpsBuffer[i];
            if (fpsBuffer[i] > highest) {
                highest = fpsBuffer[i];
            }
            if (fpsBuffer[i] < lowest) {
                lowest = fpsBuffer[i];
            }
        }
        AverageFPS = sum / frameRange;
        HighestFPS = highest;
        LowestFPS = lowest;
    }

    private void Update() {
        if (fpsBuffer == null || fpsBuffer.Length != frameRange) {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
    }
}
