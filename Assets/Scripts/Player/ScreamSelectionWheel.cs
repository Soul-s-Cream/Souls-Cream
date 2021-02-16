using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreamSelectionWheel : MonoBehaviour
{
    #region Public Fields
    public int position = 0;
    [Range(0.1f, 5f)]
    public float fadeOutDelay = 2f;
    public float radiusWheel = 2f;
    public Vector3 scaleIconActive = new Vector3(0.75f, 0.75f, 1f);
    public Vector3 scaleIconInactive = new Vector3(0.5f, 0.5f, 1f);
    public float speedRotationWheel = 1f;
    public AK.Wwise.Event newSceamSound;

    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            ToggleWheelDisplay();
        }
    }

    public int Position
    {
        get { return position;  }
        set
        {
            oldPosition = position;
            position = value;
        }
    }
    #endregion

    #region Private Fields
    private Coroutine timeToDie;
    private Vector3 rotation;
    private bool isActive = false;
    private SpriteRenderer spriteRender;
    private Player player;
    public List<GameObject> wheelItems;
    private GameObject wheel;
    public int oldPosition;
    #endregion

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();

        IsActive = false;
        spriteRender.color = Color.clear;

        wheel = Instantiate(new GameObject("Wheel"), transform);
        wheel.transform.localPosition = new Vector3(0f, radiusWheel, 0f);
        wheelItems = new List<GameObject>();

        player = transform.parent.GetComponent<Player>();
        if (player == null)
        {
            this.enabled = false;
            throw new System.NullReferenceException("Player parent is missing. Please add a GameObject as parent with Player component");
        }

        PopulateWheel();
        AddListeners();
    }

    #region Listeners
    void AddListeners()
    {
        GameEvents.Instance.changeScreamAbilitySelected += OnChangeScreamAbilitySelected;
        GameEvents.Instance.newScreamAbility += OnNewScreamAbility;
        GameEvents.Instance.loseScreamAbility += OnLoseScreamAbility;
    }

    void RemoveListeners()
    {
        GameEvents.Instance.changeScreamAbilitySelected -= OnChangeScreamAbilitySelected;
        GameEvents.Instance.newScreamAbility -= OnNewScreamAbility;
        GameEvents.Instance.loseScreamAbility -= OnLoseScreamAbility;
    }
    #endregion

    private void Start()
    {
        rotation = wheel.transform.eulerAngles;
    }

    private void RotateStepSelectionWheel(bool isAnglePositive = true)
    {
        if (wheelItems.Count != 0)
            RotateSelectionWheel((isAnglePositive ? 1 : -1) * (360f / wheelItems.Count));
        else
            RotateSelectionWheel(0);
    }

    private void RotateSelectionWheel(float angle, bool addAngleMode = true)
    {
        if (addAngleMode)
        {
            if (rotation.z + angle >= 360f)
                rotation.z += angle - 360f;
            else if (rotation.z + angle <= 0f)
                rotation.z += angle + 360f;
            else
                rotation.z += angle;
        }
        else
        {
            rotation.z = angle;
        }
        Debug.Log("Rotate wheel to " + rotation.z);

        wheel.transform.DORotate(rotation, speedRotationWheel);
        //Nécessite probablement un .OnComplete pour réinitialiser la valeur de rotation ou un truc du genre
        if (wheelItems.Count != 0)
        {
            SetIconActive(wheelItems[position], true);
            if(position != oldPosition) SetIconActive(wheelItems[oldPosition], false);
        }
        ResetTimeToDie();
    }

    private void RotatePositionSelectionWheel(int i)
    {
        Position = i;
        RotateSelectionWheel(360 - (360 / wheelItems.Count) * Position, false);
    }

    private void SetIconActive(GameObject icon, bool isActive = true)
    {

        if (isActive)
        {
            icon.transform.DOScale(scaleIconActive, 1f);
            icon.GetComponent<SpriteRenderer>().DOColor(Color.white, 1f);
        }
        else
        {
            icon.transform.DOScale(scaleIconInactive, 1f);
            icon.GetComponent<SpriteRenderer>().DOColor(Color.clear, 1f);
        }
    }

    private void PopulateWheel()
    {
        foreach (GameObject item in wheelItems)
        {
            Destroy(item);
        }

        wheelItems.Clear();
        wheel.transform.localEulerAngles = new Vector3(0,0,0);
        for (int i = 0; i < player.unlockedScreams.Count; i++)
        {
            GameObject gj = Instantiate(new GameObject(System.Enum.GetName(typeof(ScreamType), player.unlockedScreams[i]) + " Icon"), wheel.transform);

            gj.transform.localPosition += gj.transform.InverseTransformDirection(Vector3.up * -radiusWheel);
            gj.transform.RotateAround(wheel.transform.position, Vector3.forward, 360 / player.unlockedScreams.Count * i);
            gj.transform.localScale = scaleIconInactive;
            SpriteRenderer sr = gj.AddComponent<SpriteRenderer>();
            sr.sprite = player.screamsDataParsed[player.unlockedScreams[i]].iconUI;
            sr.color = Color.clear;

            wheelItems.Add(gj);
        }

        //if (wheelItems.Count != 0) RotatePositionSelectionWheel(Position);
    }

    /// <summary>
    /// Réinitialise le temps avant disparition à l'écran quand une nouvelle sélection est faite
    /// </summary>
    private void ResetTimeToDie()
    {
        IsActive = true;
        if (timeToDie != null)
        {
            StopCoroutine(timeToDie);
        }
        timeToDie = StartCoroutine(TimeToDie());
    }

    /// <summary>
    /// Delai automatisé avant la disparition de l'UI à l'écran
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        IsActive = false;
        if (wheelItems.Count != 0) SetIconActive(wheelItems[position], false);
    }

    /// <summary>
    /// Si la roue est considérée active, on l'affiche progressivement. Si désactivée, alors on l'a fait disparaître progressivement
    /// </summary>
    private void ToggleWheelDisplay()
    {
        if (isActive)
            spriteRender.DOColor(Color.white, 1f);
        else
            spriteRender.DOColor(Color.clear, 1f);
    }

    private void OnChangeScreamAbilitySelected(Player player, ScreamType ability)
    {
        if (player.Equals(this.player))
        {
            RotatePositionSelectionWheel(player.screamUnlockedIndex);
        }
    }

    private void OnNewScreamAbility(Player player, ScreamType newAbility)
    {
        if(player.Equals(this.player))
        {
            PopulateWheel();
            RotatePositionSelectionWheel(player.unlockedScreams.Count - 1);
            newSceamSound.Post(gameObject);
        }
    }

    private void OnLoseScreamAbility(Player player, ScreamType losedAbility)
    {
        if (player.Equals(this.player))
        {
            PopulateWheel();
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
