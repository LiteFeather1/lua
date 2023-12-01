using UnityEngine;
using System.Collections.Generic;

public class ChainLightning : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private LineRenderer lineRendererPrefab;

    [Header("Config")]
    [SerializeField] private int chainLength;
    [SerializeField] private int lightnings;

    private float nextRefresh;
    private readonly float segmentLength = 0.2f;

    private List<LightningBolt> LightningBolts { get; set; }
    [field: SerializeField] private List<Vector2> Targets { get; set; }

    void Awake()
    {
        LightningBolts = new();
        Targets = new();

        for (int i = 0; i < chainLength; i++)
        {
            var tmpLightningBolt = new LightningBolt(segmentLength, i);
            tmpLightningBolt.Init(lightnings, lineRendererPrefab);
            LightningBolts.Add(tmpLightningBolt);
        }
        BuildChain();
    }

    public void BuildChain()
    {
        //Build a chain, in a real project this might be enemies ;)
        Targets.Clear();
        for (int i = 0; i < chainLength; i++)
        {
            const float RANGE = 1.28f;
            Targets.Add(new Vector2(Random.Range(-RANGE, RANGE), Random.Range(-RANGE, RANGE)));
            LightningBolts[i].Activate();
        }
    }

    void Update()
    {
        //Refresh the LightningBolts
        if (Time.time > nextRefresh)
        {
            //BuildChain();
            for (int i = 0; i < Targets.Count; i++)
            {
                if (i == 0)
                {
                    LightningBolts[i].DrawLightning(Vector2.zero, Targets[i]);
                }
                else
                {
                    LightningBolts[i].DrawLightning(Targets[i - 1], Targets[i]);
                }
            }
            nextRefresh = Time.time + 0.1f;
        }
    }
}
