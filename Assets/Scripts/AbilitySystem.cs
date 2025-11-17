using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public int intValue = 1;          // valor base (se usa según la lógica)
        public float floatValue = 0.5f;   // valor base float
        public int maxLevel = 5;
    }

    [Header("Definición de habilidades (edítalas aquí)")]
    public List<AbilityEntry> abilities = new List<AbilityEntry>();

    [Header("UI - Picker (simple)")]
    public GameObject panel;               // panel root del picker (inactivo por defecto)
    public Transform optionsParent;        // contenedor donde se instancian los botones
    public GameObject optionButtonPrefab;  // prefab simple: Button con child Image + TMP Text

    [Header("Picker settings")]
    public int optionsToShow = 3;

    // estado interno: niveles por id
    Dictionary<string, int> levels = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    // ----------------- API pública -----------------
    // muestra el picker con N opciones aleatorias
    public void ShowPicker()
    {
        if (panel == null || optionButtonPrefab == null || optionsParent == null) return;

        panel.SetActive(true);

        // limpiar antiguas opciones
        foreach (Transform t in optionsParent) Destroy(t.gameObject);

        // crear opciones
        for (int i = 0; i < optionsToShow; i++)
        {
            var entry = GetRandomAbility();
            var go = Instantiate(optionButtonPrefab, optionsParent);
            SetupOption(go, entry);
        }
    }

    public void HidePicker()
    {
        if (panel != null) panel.SetActive(false);
    }

    // aplica habilidad por id (sube nivel si ya la tienes)
    public void ApplyAbilityById(string id)
    {
        var entry = GetById(id);
        if (entry == null) return;

        levels.TryGetValue(id, out int cur);
        if (cur >= entry.maxLevel) { Debug.Log($"{entry.displayName} at max level"); return; }
        cur++;
        levels[id] = cur;

        // aplicar efecto concreto según id y nivel (lógica literal aquí)
        ApplyEffect(entry, cur);

        // cerrar picker después de elegir
        HidePicker();
    }

    // ----------------- util -----------------
    AbilityEntry GetById(string id) => abilities.Find(a => a != null && a.id == id);
    AbilityEntry GetRandomAbility()
    {
        if (abilities == null || abilities.Count == 0) return null;
        return abilities[UnityEngine.Random.Range(0, abilities.Count)];
    }

    void SetupOption(GameObject go, AbilityEntry entry)
    {
        // buscar componentes básicos en el prefab: Image e TextMeshProUGUI y Button
        var img = go.GetComponentInChildren<Image>();
        var texts = go.GetComponentsInChildren<TextMeshProUGUI>();
        var btn = go.GetComponent<Button>();

        if (img != null) img.sprite = entry != null ? entry.icon : null;
        if (texts.Length > 0) texts[0].text = entry != null ? entry.displayName : "N/A";
        if (texts.Length > 1) texts[1].text = entry != null ? entry.description : "";

        string id = entry != null ? entry.id : "";
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => {
                ApplyAbilityById(id);
            });
        }
    }

    // ----------------- Lógica de efectos (aquí está todo, sencillo) -----------------
    // modifica tr?cctamente componentes del player (PlayerMovement y AutoAttack si existen)
    void ApplyEffect(AbilityEntry entry, int level)
    {
        if (entry == null) return;
        float valueFloat = entry.floatValue + (level - 1) * entry.floatValue;
        int valueInt = entry.intValue + (level - 1) * entry.intValue;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) { Debug.Log("No player found for ability apply."); return; }

        // Aquí pones las implementaciones literales por id; añade cases según tus necesidades
        switch (entry.id)
        {
            case "damage":
                {
                    var auto = player.GetComponent<AutoAttack>();
                    if (auto != null)
                        auto.damage += valueInt; // suma daño entero
                    else
                        Debug.Log("No AutoAttack component found for damage ability.");
                }
                break;

            case "firerate":
                {
                    var auto = player.GetComponent<AutoAttack>();
                    if (auto != null)
                        auto.attackInterval = Mathf.Max(0.02f, auto.attackInterval - valueFloat);
                }
                break;

            case "movespeed":
                {
                    var pm = player.GetComponent<PlayerMovement>();
                    if (pm != null)
                        pm.moveSpeed += valueFloat;
                }
                break;

            case "maxhealth":
                {
                    var pm = player.GetComponent<PlayerMovement>();
                    if (pm != null)
                    {
                        pm.maxHealth += valueInt;
                        pm.currentHealth += valueInt; // curamos al mejorar max
                    }
                }
                break;

            default:
                Debug.Log($"Ability id '{entry.id}' no tiene implementación. Añádela en ApplyEffect.");
                break;
        }

        Debug.Log($"Applied ability {entry.displayName} level {level}");
    }

    // helper para ver nivel actual (opcional)
    public int GetLevel(string id)
    {
        levels.TryGetValue(id, out int l);
        return l;
    }
}

