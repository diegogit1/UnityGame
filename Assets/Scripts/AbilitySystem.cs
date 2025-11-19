using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema sencillo de abilities aleatorias. 
/// - Define las abilities en el inspector (icono/nombre/descripción/valores).
/// - Llamar ApplyRandomAbility() para aplicar una mejora aleatoria al jugador.
/// - Si la ability ya está, sube su nivel hasta maxLevel.
/// - No hay UI aquí; solo lógica. Ideal para prototipar.
/// </summary>
public class AbilitySystem : MonoBehaviour
{
    public static AbilitySystem Instance { get; private set; }

    [Serializable]
    public class AbilityEntry
    {
        public string id;                 // id único, ej. "damage"
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public int intValue = 1;          // valor base entero (ej. +1 daño)
        public float floatValue = 0.2f;   // valor base float (ej. -0.1s cadencia)
        public int maxLevel = 5;
    }

    [Header("Lista de abilities (edítalas aquí)")]
    public List<AbilityEntry> abilities = new List<AbilityEntry>();

    [Header("Debug / pruebas")]
    public bool enableDebugKey = true;
    public KeyCode debugKey = KeyCode.R; // presiona R para aplicar una random en Play

    // Estado: niveles por id
    private Dictionary<string, int> levels = new Dictionary<string, int>();

    // Evento (opcional) que otros sistemas pueden suscribirse:
    // parameters: ability id, new level
    public event Action<string, int> OnAbilityApplied;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // (opcional) validaciones mínimas
        if (abilities == null || abilities.Count == 0)
            Debug.LogWarning("[AbilitySystemRandom] No hay abilities definidas en el inspector.");
    }

    void Update()
    {
        if (enableDebugKey && Input.GetKeyDown(debugKey))
            ApplyRandomAbility();
    }

    /// <summary>
    /// Escoge una ability aleatoria de la lista y la aplica al player.
    /// </summary>
    public void ApplyRandomAbility()
    {
        if (abilities == null || abilities.Count == 0)
        {
            Debug.LogWarning("[AbilitySystemRandom] No hay abilities definidas.");
            return;
        }

        // elegir una al azar
        var entry = abilities[UnityEngine.Random.Range(0, abilities.Count)];
        if (entry == null)
        {
            Debug.LogWarning("[AbilitySystemRandom] Entrada nula en abilities.");
            return;
        }

        ApplyAbility(entry.id);
    }

    /// <summary>
    /// Aplica (o sube el nivel) de la ability indicada por id.
    /// </summary>
    public void ApplyAbility(string id)
    {
        var entry = abilities.Find(a => a != null && a.id == id);
        if (entry == null)
        {
            Debug.LogWarning($"[AbilitySystemRandom] Ability id '{id}' no encontrada.");
            return;
        }

        levels.TryGetValue(id, out int currentLevel);
        if (currentLevel >= entry.maxLevel)
        {
            Debug.Log($"[AbilitySystemRandom] {entry.displayName} ya está en nivel máximo ({entry.maxLevel}).");
            return;
        }

        int newLevel = currentLevel + 1;
        levels[id] = newLevel;

        // aplicar efecto concreto (casos simples — edita según tus scripts)
        ApplyEffect(entry, newLevel);

        Debug.Log($"[AbilitySystemRandom] Applied {entry.displayName} -> level {newLevel}");

        // notificar
        OnAbilityApplied?.Invoke(id, newLevel);
    }

    /// <summary>
    /// Implementa aquí, de forma literal, lo que hace cada habilidad.
    /// Ajusta los nombres de campos para que coincidan con tus componentes (AutoAttack, PlayerMovement, etc.).
    /// </summary>
    void ApplyEffect(AbilityEntry entry, int level)
    {
        // ejemplo de cálculo simple (puedes cambiar la fórmula)
        int valueInt = entry.intValue + (level - 1) * entry.intValue;
        float valueFloat = entry.floatValue + (level - 1) * entry.floatValue;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[AbilitySystemRandom] No se encontró GameObject con tag 'Player'.");
            return;
        }

        // Aquí pones las implementaciones literales. Cambia los nombres de campos si tus scripts difieren.
        switch (entry.id)
        {
            case "damage":
                {
                    var auto = player.GetComponent<AutoAttack>();
                    if (auto != null) auto.damage += valueInt;
                    else Debug.Log("[AbilitySystemRandom] AutoAttack no encontrado para 'damage'.");
                }
                break;

            case "firerate":
                {
                    var auto = player.GetComponent<AutoAttack>();
                    if (auto != null) auto.attackInterval = Mathf.Max(0.02f, auto.attackInterval - valueFloat);
                    else Debug.Log("[AbilitySystemRandom] AutoAttack no encontrado para 'firerate'.");
                }
                break;

            case "movespeed":
                {
                    var pm = player.GetComponent<PlayerMovement>();
                    if (pm != null) pm.moveSpeed += valueFloat;
                    else Debug.Log("[AbilitySystemRandom] PlayerMovement no encontrado para 'movespeed'.");
                }
                break;

            case "maxhealth":
                {
                    var pm = player.GetComponent<PlayerMovement>();
                    if (pm != null)
                    {
                        pm.maxHealth += valueInt;
                        pm.currentHealth += valueInt; // curar al subir max
                    }
                    else Debug.Log("[AbilitySystemRandom] PlayerMovement no encontrado para 'maxhealth'.");
                }
                break;

            // Añade aquí más cases según tus ids
            default:
                Debug.Log($"[AbilitySystemRandom] No hay implementación para ability id '{entry.id}'. Añádela en ApplyEffect.");
                break;
        }
    }

    // utilidad
    public int GetLevel(string id)
    {
        levels.TryGetValue(id, out int l);
        return l;
    }

    // opcional: resetear todo (útil al reiniciar nivel)
    public void ResetAll()
    {
        levels.Clear();
    }
}


