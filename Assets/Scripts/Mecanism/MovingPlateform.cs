using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovingPlateform : Mecanism
{
    #region Public Fields
    [SerializeField]
    public Vector3 EndPosition
    {
        get 
        {
            if (!Application.isPlaying)
                return transform.TransformPoint(endPositionRelative);
            else
                return transform.TransformPoint(transform.InverseTransformPoint(startPoint) + endPositionRelative);
        }
        set { endPositionRelative = transform.InverseTransformPoint(value); }
    }

    [Header("Behavior")]
    /// <summary>
    /// Point de fin de la trajectoire.
    /// </summary>
    [Tooltip("Coordonnées relatives du point de fin")]
    [SerializeField]
    public Vector3 endPositionRelative;
    /// <summary>
    /// Si la plateforme fait des allées-retours entre le endPosition et le startPoint;
    /// </summary>
    [Tooltip("Est-ce que la plateforme fait des allées-retours entre le point de fin et le départ ?")]
    public bool looping = true;
    /// <summary>
    /// Si la plateforme doit être masqué lorsqu'elle arrive au point de fin
    /// </summary>
    [Tooltip("Est-ce que la plateforme se masque arrivé au point final ?")]
    public bool isMasked = false;
    [Header("Vitesses d'animation")]
    /// <summary>
    /// Vitesse de parcours d'une trajectoire. En nombre d'unité dans l'espace parcourue par seconde
    /// </summary>
    [Tooltip("Vitesse de parcours d'une trajectoire. En nombre d'unité dans l'espace parcourue par seconde")]
    [Range(0.1f, 5f)]
    public float speed = 0.5f;

    /// <summary>
    /// Délai avant d'initier une nouvelle trajectoire
    /// </summary>
    [Tooltip("Délai avant d'initier une nouvelle trajectoire")]
    [Range(0f, 5f)]
    public float delayBeforeNewTrajectory = 0f;

    /// <summary>
    /// Courbe de vitesse de la trajectoire
    /// </summary>
    [Tooltip("La courbe de vitesse de la trajectoire")]
    public Ease trajectoryEasing = Ease.Linear;

    [Tooltip("Délai avant l'animation d'ouverture")]
    public float delayBeforeActivation = 0.2f;
    [Header("Sound")]
    public AK.Wwise.Event openSound;
    #endregion

    #region Private Fields
    /// <summary>
    /// Si l'élévateur est actif ou non
    /// </summary>
    private bool isOn = false;
    /// <summary>
    /// Point de départ de la trajectoire. Automatiquement définie comme étant la position d'origine.
    /// </summary>
    private Vector3 startPoint;
    /// <summary>
    /// Destination de la trajectoire actuelle
    /// </summary>
    private Vector3 destination;
    /// <summary>
    /// Le Tween actuel chargé de déplacer l'élévateur
    /// </summary>
    private Tween tweenRunning;
    /// <summary>
    /// SpriteRender from the GameObject
    /// </summary>
    private SpriteRenderer spriteRender;
    /// <summary>
    /// SpriteMask in case isMasked is true
    /// </summary>
    private SpriteMask mask;
    /// <summary>
    /// BoxCollider2D of the object
    /// </summary>
    private BoxCollider2D boxCollider2D;
    /// <summary>
    /// Original Bounds of the BoxCollider2D of the object
    /// </summary>
    private Bounds originalBC2D;
    #endregion

    private void Awake()
    {
        //On définit le point de départ comme étant la position actuelle, et la prochaine destination comme étant le point de fin
        startPoint = this.transform.position;
        destination = EndPosition;
        spriteRender = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        /*originalBC2D = new Bounds(boxCollider2D.bounds.center, boxCollider2D.bounds.size);

        if(isMasked && Application.isPlaying)
        {
            spriteRender.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            mask = Instantiate(new GameObject(this.name + " Mask"), this.EndPosition, Quaternion.identity)
                .AddComponent<SpriteMask>();
            mask.sprite = spriteRender.sprite;
            mask.alphaCutoff = 0f;
        }*/
    }

    private void Reset()
    {
        EndPosition = DefaultEndPointPosition();
    }

    private void FixedUpdate()
    {
        /*if (isMasked)
        {
            if(mask.bounds.Intersects(boxCollider2D.bounds))
            {
                Vector2 reduction = Vector3.zero;
                Vector3 centerDistance = boxCollider2D.bounds.center - mask.bounds.center;
                if(centerDistance.x >= centerDistance.y)
                {
                    reduction = transform.InverseTransformDirection(new Vector2(centerDistance.x - mask.bounds.extents.x, 0));
                }
                else
                {
                    reduction = transform.InverseTransformDirection(new Vector2(0, centerDistance.y - mask.bounds.extents.y));
                }

                boxCollider2D.size += reduction;
                boxCollider2D.offset += reduction;
            }
        }*/
    }

    #region Mecanism Logic
    /// <summary>
    /// Appelé lorsque l'objet est activé. L'élévateur commence alors un déplacement vers sa prochaine destination
    /// </summary>
    protected override void SwitchingOn()
    {
        StartCoroutine("SwitchingOnBehavior");
    }

    IEnumerator SwitchingOnBehavior()
    {
        yield return new WaitForSeconds(delayBeforeActivation);
        tweenRunning?.Kill();

        isOn = true;
        MoveToDestination();
    }

    /// <summary>
    /// Appelé lorsque l'objet est désactivé. L'élévateur interrompt alors ses trajectoires, 
    /// et en commence une nouvelle en direction de sa position de départ
    /// </summary>
    protected override void SwitchingOff()
    {
        StartCoroutine("SwitchingOffBehavior");
    }

    IEnumerator SwitchingOffBehavior()
    {
        yield return new WaitForSeconds(delayBeforeActivation);
        tweenRunning?.Kill();

        openSound.Post(gameObject);
        isOn = false;
        tweenRunning = transform.DOMove(startPoint, TimeReachingPosition(startPoint))
            .OnKill(() => { openSound.Stop(gameObject, 200); })
            .SetEase(trajectoryEasing)
            .OnComplete(() => { openSound.Stop(gameObject, 200); });
    }
    #endregion

    #region Moving Plateform Logic
    public Vector3 DefaultEndPointPosition()
    {
        return transform.TransformPoint(Vector3.up * 2);
    }
    /// <summary>
    /// Déplacer l'élévateur vers sa prochaine trajectoire. Une fois terminée, il effectue la trajectoire inverse si
    /// l'élévateur est encore actif.
    /// </summary>
    private void MoveToDestination()
    {
        openSound.Post(gameObject);
        tweenRunning = transform.DOMove(destination, TimeReachingPosition(destination))
            .SetEase(trajectoryEasing)
            .OnKill(() => { openSound.Stop(gameObject, 200); })
            .OnComplete(() => { if(looping) StartCoroutine("DelayBeforeNewTrajectory"); openSound.Stop(gameObject, 200); });
    }

    /// <summary>
    /// Coroutine lançant la nouvelle trajectoire en décidant de la nouvelle destination après que delayBeforeTrajectory se soit écoulé.
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayBeforeNewTrajectory()
    {
        if (isOn)
        {
            yield return new WaitForSeconds(delayBeforeNewTrajectory);
            destination = this.transform.position == startPoint ? EndPosition : startPoint;
            MoveToDestination();
        }
    }

    /// <summary>
    /// Calcul le temps nécessaire pour atteindre une position spécifié par rapport à la position actuel du GameObject
    /// </summary>
    /// <param name="position">Position de destination pour calculer le temps de trajectoire</param>
    /// <returns>Temps pour effectuer la trajectoire jusqu'au point spécifié</returns>
    public float TimeReachingPosition(Vector3 position)
    {
        return Mathf.Abs((this.transform.position - position).magnitude) / speed;
    }
    #endregion
}
