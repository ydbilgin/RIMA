using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RIMA.Balance;

public class BalanceTelemetry : MonoBehaviour
{
    public static BalanceTelemetry Instance { get; private set; }

    private readonly Dictionary<DamageSourceType, int> damageBySource = new();
    private int totalDamageDealt;
    private int totalDamageTaken;
    private int healingDone;
    private float roomStartTime;
    private string roomSeed;
    private string enemyPreset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void BeginRoom(string seed, string preset)
    {
        roomSeed = seed;
        enemyPreset = preset;
        roomStartTime = Time.time;
        totalDamageDealt = 0;
        totalDamageTaken = 0;
        healingDone = 0;
        damageBySource.Clear();
    }

    public void RecordDamageDealt(DamagePacket packet, DamageCalculationResult result)
    {
        totalDamageDealt += result.finalDamage;

        if (!damageBySource.ContainsKey(packet.sourceType))
            damageBySource[packet.sourceType] = 0;

        damageBySource[packet.sourceType] += result.finalDamage;
    }

    public void RecordDamageTaken(int amount)
    {
        totalDamageTaken += amount;
    }

    public void RecordHealing(int amount)
    {
        healingDone += amount;
    }

    public string BuildRoomSummaryJson(string classType, string statProfileName)
    {
        float clearTime = Time.time - roomStartTime;
        var sb = new StringBuilder();
        sb.Append("{\n");
        sb.Append($"  \"roomSeed\": \"{roomSeed}\",\n");
        sb.Append($"  \"enemyPreset\": \"{enemyPreset}\",\n");
        sb.Append($"  \"classType\": \"{classType}\",\n");
        sb.Append($"  \"statProfile\": \"{statProfileName}\",\n");
        sb.Append($"  \"clearTimeSec\": {clearTime:F2},\n");
        sb.Append($"  \"damageDealt\": {totalDamageDealt},\n");
        sb.Append($"  \"damageTaken\": {totalDamageTaken},\n");
        sb.Append($"  \"healingDone\": {healingDone}\n");
        sb.Append("}");
        return sb.ToString();
    }
}
