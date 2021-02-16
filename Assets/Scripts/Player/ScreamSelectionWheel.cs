using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreamSelectionWheel : MonoBehaviour
{
    #region Public Fields
    [Header("Animation Speeds")]
    [Tooltip("D�lai avant disparition de l'affichage")]
    [Range(0.1f, 5f)]
    public float fadeOutDelay = 2f;
    [Tooltip("Vitesse de rotation de la roue pour un pas")]
    public float speedRotationWheel = 1f;
    [Header("Icons Scale")]
    [Tooltip("�chelle d'un ic�ne lorsque actif")]
    public Vector3 scaleIconActive = new Vector3(0.75f, 0.75f, 1f);
    [Tooltip("�chelle d'un ic�ne lorsqu'il est en cours de d�sactivation")]
    public Vector3 scaleIconInactive = new Vector3(0.5f, 0.5f, 1f);
    [Header("SFX")]
    [Tooltip("SFX lors du gain d'un nouveau cri")]
    public AK.Wwise.Event newScreamSound;

    /// <summary>
    /// D�termine si la roue est affich�e ou non
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            ToggleWheelDisplay();
        }
    }

    /// <summary>
    /// Enregistre la nouvelle position et l'ancienne
    /// </summary>
    public int Position
    {
        get { return position;  }
        set
        {
            if(oldPosition != position) oldPosition = position;
            position = value;
        }
    }
    #endregion

    #region Private Fields
    //Composants n�cessaires pour le fonctionnement
    private SpriteRenderer spriteRender;
    private Player player;
    /// <summary>
    /// Le cadran d'affichage qui va tourner pour afficher le bon ic�ne
    /// </summary>
    private GameObject wheel;
    //Donn�es sur l'objet
    private Vector3 rotation;
    /// <summary>
    /// M�moire de la position sur le cadran (se synchronise lors d'un changement chez le joueur)
    /// </summary>
    private int position = 0;
    /// <summary>
    /// Pr�c�dente position
    /// </summary>
    private int oldPosition;
    /// <summary>
    /// D�termine si la roue doit �tre affich�e ou non. Passez plut�t par IsActive
    /// </summary>
    private bool isActive = false;
    /// <summary>
    /// Le rayon de la roue. N�cessaire pour d�terminer le placement des ic�nes
    /// </summary>
    private float radiusWheel = 2f;
    //Objets joints pour la logique
    /// <summary>
    /// Coroutine contenant le temps avant disparition automatique
    /// </summary>
    private Coroutine timeToDie;
    /// <summary>
    /// Liste contenant les diff�rents objets d'ic�nes
    /// </summary>
    private List<GameObject> wheelItems;
    #endregion

    private void Awake()
    {
        //On r�cup�re les diff�rents composants n�cessaires pour le fonctionnement
        spriteRender = GetComponent<SpriteRenderer>();
        player = transform.parent.GetComponent<Player>();
        if (player == null)
        {
            this.enabled = false;
            throw new System.NullReferenceException("Player parent is missing. Please add a GameObject as parent with Player component");
        }

        //On cr�er la roue qui va servir de "cadran d'affichage"
        wheel = Instantiate(new GameObject("Wheel"), transform);
        wheel.transform.localPosition = new Vector3(0f, radiusWheel, 0f);

        wheelItems = new List<GameObject>();

        //Par d�faut, on rend l'UI invisible
        IsActive = false;
        spriteRender.color = Color.clear;

        PopulateWheel();
        AddListeners();
    }

    #region Listeners
    /// <summary>
    /// On s'inscrit aux �v�nements interessants pour l'objet
    /// </summary>
    void AddListeners()
    {
        GameEvents.Instance.changeScreamAbilitySelected += OnChangeScreamAbilitySelected;
        GameEvents.Instance.newScreamAbility += OnNewScreamAbility;
        GameEvents.Instance.loseScreamAbility += OnLoseScreamAbility;
    }
    /// <summary>
    /// On se d�sinscrit aux �v�nements interessants pour l'objet
    /// </summary>
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

    /// <summary>
    /// Peuple la roue d'ic�nes en fonction des cris d�bloqu�s par le joueur associ�
    /// </summary>
    private void PopulateWheel()
    {
        //On nettoie la roue si elle �tait d�j� peupl�e
        foreach (GameObject item in wheelItems)
        {
            Destroy(item);
        }

        wheelItems.Clear();

        //On r�initialise la rotation pour �viter des probl�mes lors du positionnement des ic�nes
        wheel.transform.localEulerAngles = new Vector3(0,0,0);
        //Pour chaque ic�ne...
        for (int i = 0; i < player.unlockedScreams.Count; i++)
        {
            //...on instancie un GameObject..
            GameObject gj = Instantiate(new GameObject(System.Enum.GetName(typeof(ScreamType), player.unlockedScreams[i]) + " Icon"), wheel.transform);
            //...qu'on d�place de la longueur du rayon vis � vis du centre de la roue en Y...
            gj.transform.localPosition += gj.transform.InverseTransformDirection(Vector3.up * -radiusWheel);
            //...puis lui fait faire une rotation en prenant comme point pivot le centre de la roue...
            gj.transform.RotateAround(wheel.transform.position, Vector3.forward, 360 / player.unlockedScreams.Count * i);
            //...on lui attribue l'�chelle d'un ic�ne d�sactiv�...
            gj.transform.localScale = scaleIconInactive;
            //...on lui ajoute un SpriteRenderer, auquel on lui attribue le sprite de l'ic�ne correspondant au cri, et on le rend transparent...
            SpriteRenderer sr = gj.AddComponent<SpriteRenderer>();
            sr.sprite = player.screamsDataParsed[player.unlockedScreams[i]].iconUI;
            sr.color = Color.clear;

            //...puis on finit par l'ajouter � la liste des ic�nes de la roue
            wheelItems.Add(gj);
        }

        //if (wheelItems.Count != 0) RotatePositionSelectionWheel(Position);
    }

    #region Wheel Rotation Logic
    /// <summary>
    /// Tourne la roue d'un pas, vers le prochain ic�ne. Le pas est calcul� automatiquement
    /// </summary>
    /// <param name="isAnglePositive">Si on veut tourner vers la gauche, 'true', sinon 'false'</param>
    private void RotateStepSelectionWheel(bool isAnglePositive = true)
    {
        if (wheelItems.Count != 0)
            RotateSelectionWheel((isAnglePositive ? 1 : -1) * (360f / wheelItems.Count));
        else
            RotateSelectionWheel(0);
    }

    /// <summary>
    /// Effectue une rotation au cadran. On peut soit ajouter un angle, soit le faire tourner � une valeur d'angle sp�cifique.
    /// </summary>
    /// <param name="angle">Valeur d'angle � ajouter ou bien destination</param>
    /// <param name="addAngleMode">Si angle doit �tre consid�r� comme une valeur ajout� � l'angle, 'true'. 
    /// Sinon, il sera consid�r� comme la nouvelle valeur d'angle</param>
    private void RotateSelectionWheel(float angle, bool addAngleMode = true)
    {
        //Si on doit ajouter l'angle en param�tre
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

        wheel.transform.DORotate(rotation, speedRotationWheel);
        //N�cessite probablement un .OnComplete pour r�initialiser la valeur de rotation ou un truc du genre
        
        //On active / d�sactive les ic�nes en fonction de la position
        if (wheelItems.Count != 0)
        {
            SetIconActive(wheelItems[position], true);
            if(oldPosition != position) SetIconActive(wheelItems[oldPosition], false);
        }
        //On r�initialise le temps avant la disparition automatique
        ResetTimeToDie();
    }

    /// <summary>
    /// Fait tourner le cadran jusqu'� l'emplacement d'ic�ne demand�
    /// </summary>
    /// <param name="positionRequested">Emplacement d'ic�ne � afficher</param>
    private void RotatePositionSelectionWheel(int positionRequested)
    {
        Position = positionRequested;
        RotateSelectionWheel(360 - (360 / wheelItems.Count) * Position, false);
    }
    #endregion

    #region Display Logic
    /// <summary>
    /// Si la roue est consid�r�e active, on l'affiche progressivement. Si d�sactiv�e, alors on l'a fait dispara�tre progressivement
    /// </summary>
    private void ToggleWheelDisplay()
    {
        if (isActive)
            spriteRender.DOColor(Color.white, 1f);
        else
            spriteRender.DOColor(Color.clear, 1f);
    }

    /// <summary>
    /// Change l'affichage de l'ic�ne en fonction de s'il doit �tre actif, ou inactif
    /// </summary>
    /// <param name="icon">L'ic�ne dont on change l'�tat</param>
    /// <param name="isActive">L'�tat que l'ic�ne doit prendre</param>
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

    /// <summary>
    /// R�initialise le temps avant disparition � l'�cran quand une nouvelle s�lection est faite
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
    /// Delai automatis� avant la disparition de l'UI � l'�cran
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        IsActive = false;
        if (wheelItems.Count != 0) SetIconActive(wheelItems[position], false);
    }
    #endregion

    #region GameEvents Callbacks
    /// <summary>
    /// Appel� quand le cri selectionn� par un joueur est chang�. On d�place alors le cadran pour afficher celui-ci
    /// </summary>
    /// <param name="player">Joueur qui a chang� de cri</param>
    /// <param name="ability">Cri selectionn�</param>
    private void OnChangeScreamAbilitySelected(Player player, ScreamType ability)
    {
        if (player.Equals(this.player))
        {
            RotatePositionSelectionWheel(player.screamUnlockedIndex);
        }
    }

    /// <summary>
    /// Appel� quand un cri est appris par un joueur. On repeuple alors le cadran, et on d�place la roue pour afficher le nouveau cri avec SFX
    /// </summary>
    /// <param name="player">Joueur qui a appris le cri</param>
    /// <param name="newAbility">Cri appris</param>
    private void OnNewScreamAbility(Player player, ScreamType newAbility)
    {
        if (player.Equals(this.player))
        {
            PopulateWheel();
            RotatePositionSelectionWheel(player.unlockedScreams.Count - 1);
            newScreamSound.Post(gameObject);
        }
    }

    /// <summary>
    /// Appel� quand un cri est d�sappris par un joueur. On repeuple alors le cadran
    /// </summary>
    /// <param name="player"></param>
    /// <param name="losedAbility"></param>
    private void OnLoseScreamAbility(Player player, ScreamType losedAbility)
    {
        if (player.Equals(this.player))
        {
            PopulateWheel();
        }
    }
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// Quand l'objet est d�truit, on se d�sinscrit aux abonnements
    /// </summary>
    private void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion
}
