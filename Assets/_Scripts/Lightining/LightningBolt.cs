﻿using UnityEngine;

public class LightningBolt
{
    public LineRenderer[] LineRenderer { get; set; }
    public LineRenderer LightRenderer { get; set; }

    public float SegmentLength { get; set; }
    public int Index { get; private set; }
    public bool IsActive { get; private set; }

    public LightningBolt(float segmentLength, int index)
    {
        SegmentLength = segmentLength;
        Index = index;
    }

    public void Init(int lineRendererCount, GameObject lineRenderPrefab, GameObject lightRendererPrefab)
    {
        LineRenderer = new LineRenderer[lineRendererCount];
        for (int i = 0; i < lineRendererCount; i++)
        {
            LineRenderer[i] = Object.Instantiate(lineRenderPrefab).GetComponent<LineRenderer>();
            LineRenderer[i].enabled = false;
        }
        //LightRenderer = Object.Instantiate(lightRendererPrefab).GetComponent<LineRenderer>();
        IsActive = false;
    }

    public void Activate()
    {
        for (int i = 0; i < LineRenderer.Length; i++)
        {
            LineRenderer[i].enabled = true;
        }

        //LightRenderer.enabled = true;
        IsActive = true;
    }

    public void DrawLightning(Vector2 source, Vector2 target)
    {
        float distance = Vector2.Distance(source, target);
        int segments;
        if (distance > SegmentLength)
            segments = Mathf.FloorToInt(distance / SegmentLength) + 2;
        else
            segments = 4;

        for (int i = 0; i < LineRenderer.Length; i++)
        {
            LineRenderer[i].positionCount = segments;
            LineRenderer[i].SetPosition(0, source);
            for (int j = 0; j < segments - 1; j++)
            {
                var t = (float)j / segments;
                Vector2 tmp = Vector2.Lerp(source, target, t);
                const float RANGE = .1f;
                Vector2 lastPosition = new(tmp.x + Random.Range(-RANGE, RANGE), tmp.y + Random.Range(-RANGE, RANGE));
                LineRenderer[i].SetPosition(j, lastPosition); ;
            }
            LineRenderer[i].SetPosition(segments - 1, target);
        }

        //LightRenderer.SetPosition(0, source);
        //LightRenderer.SetPosition(1, target);

        //Color lightColor = new(.5647f, .58823f, 1f, Random.Range(0.2f, 1f));
        //LightRenderer.startColor = lightColor;
        //LightRenderer.endColor = lightColor;
    }
}