using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentCount : MonoBehaviour
{
    public static AgentCount instance;
    public Text agentCountText;
    public int agentCount, newAgentCount;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        agentCountText.text = "" + agentCount;
    }
}
