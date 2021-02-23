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
    [Tooltip("Coordonn�es relatives du point de fin")]
    [SerializeField]
    public Vector3 endPositionRelative;
    /// <summary>
    /// Si la plateforme fait des all�es-retours entre le endPosition et le startPoint;
    /// </summary>
    [Tooltip("Est-ce que la plateforme fait des all�es-retours entre le point de fin et le d�part ?")]
    public bool looping = true;
    /// <summary>
    /// Si la plateforme doit �tre masqu� lorsqu'elle arrive au point de fin
    /// </summary>
    [Tooltip("Est-ce que la plateforme se masque arriv� au point final ?")]
    public bool isMasked = false;
    [Header("Vitesses d'animation")]
    /// <summary>
    /// Vitesse de parcours d'une trajectoire. En nombre d'unit� dans l'espace parcourue par seconde
    /// </summary>
    [Tooltip("Vitesse de parcours d'une trajectoire. En nombre d'unit� dans l'espace parcourue par seconde")]
    [Range(0.1f, 5f)]
    public float speed = 0.5f;

    /// <summary>
    /// D�lai avant d'initier une nouvelle trajectoire
    /// </summary>
    [Tooltip("D�lai avant d'initier une nouvelle trajectoire")]
    [Range(0f, 5f)]
    public float delayBeforeNewTrajectory = 0f;

    /// <summary>
    /// Courbe de vitesse de la trajectoire
    /// </summary>
    [Tooltip("La courbe de vitesse de la trajectoire")]
    public Ease trajectoryEasing = Ease.Linear;

    [Tooltip("D�lai avant l'animation d'ouverture")]
    public float delayBeforeActivation = 0.2f;
    [Header("Sound")]
    public AK.Wwise.Event openSound;
    #endregion

    #region Private Fields
    /// <summary>
    /// Si l'�l�vateur est actif ou non
    /// </summary>
    private bool isOn = false;
    /// <summary>
    /// Point de d�part de la trajectoire. Automatiquement d�finie comme �tant la position d'origine.
    /// </summary>
    private Vector3 startPoint;
    /// <summary>
    /// Le Tween actuel charg� de d�placer l'�l�vateur
    /// </summary>
    private Sequence sequenceMoving;
    /// <summary>
    /// Le Tween charg� d'activer sequenceMoving apr�s le d�lai d'activation;
    /// </summary>
    private Sequence sequenceDelayBeforeMove;
    /// <summary>
    /// If MovingPlateform is moving in direction of the EndPoint
    /// </summary>
    private bool toEndPoint;
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
    #endregion

    private void Awake()
    {
        //On d�finit le point de d�part comme �tant la position actuelle, et la prochaine destination comme �tant le point de fin
        startPoint = this.transform.position;
        spriteRender = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        if (isMasked && Application.isPlaying)
        {
            spriteRender.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            mask = Instantiate(new GameObject(this.name + " Mask"), this.EndPosition, Quaternion.identity)
                .AddComponent<SpriteMask>();
            mask.sprite = spriteRender.sprite;
            mask.alphaCutoff = 0f;
        }
    }

    private void Reset()
    {
        EndPosition = DefaultEndPointPosition();
    }

    #region Mecanism Logic
    /// <summary>
    /// Appel� lorsque l'objet est activ�. L'�l�vateur commence alors un d�placement vers sa prochaine destination
    /// </summary>
    protected override void SwitchingOn()
    {
        sequenceMoving.Kill(true);
        sequenceDelayBeforeMove.Kill(true);

        sequenceMoving = DOTween.Sequence();
        sequenceMoving
            .Append
            (
                transform.DOMove(EndPosition, TimeReachingPosition(EndPosition))
                .SetEase(trajectoryEasing)
                .SetDelay(delayBeforeNewTrajectory)
                .OnComplete(() => { if (!looping) openSound.Stop(gameObject, 200); })
            )
            .InsertCallback(delayBeforeNewTrajectory, () => { openSound.Stop(gameObject, 200); openSound.Post(gameObject); toEndPoint = true; })
            .OnKill(() => { openSound.Stop(gameObject, 200); });
        if (looping)
        {
            sequenceMoving
                .AppendInterval(delayBeforeNewTrajectory)
                .Append
                (
                    transform.DOMove(startPoint, TimeReachingPosition(EndPosition))
                    .SetEase(trajectoryEasing)
                    .SetDelay(delayBeforeNewTrajectory)
                 )
                .SetLoops(-1)
                .InsertCallback(TimeReachingPosition(EndPosition) + delayBeforeNewTrajectory, () => { openSound.Stop(gameObject, 200); openSound.Post(gameObject); toEndPoint = false; })
                .Pause();
        }

        DOTween.Sequence()
            .AppendInterval(delayBeforeActivation)
            .OnComplete(() => { sequenceMoving.Play(); })
            .SetAutoKill(true);

        isOn = true;
    }

    /// <summary>
    /// Appel� lorsque l'objet est d�sactiv�. L'�l�vateur interrompt alors ses trajectoires, 
    /// et en commence une nouvelle en direction de sa position de d�part
    /// </summary>
    protected override void SwitchingOff()
    {
        sequenceMoving.Kill(true);
        sequenceDelayBeforeMove.Kill(true);
        openSound.Post(gameObject);

        isOn = false;
        toEndPoint = false;

        sequenceMoving = DOTween.Sequence();
        sequenceMoving.Append(
            transform.DOMove(startPoint, TimeReachingPosition(startPoint))
            .SetDelay(delayBeforeActivation)
            .OnKill(() => { openSound.Stop(gameObject, 200); })
            .SetEase(trajectoryEasing)
            .OnComplete(() => { openSound.Stop(gameObject, 200); })
            );
    }
    #endregion

    #region Moving Plateform Logic
    public Vector3 DefaultEndPointPosition()
    {
        return transform.TransformPoint(Vector3.up * 2);
    }

    /// <summary>
    /// Calcul le temps n�cessaire pour atteindre une position sp�cifi� par rapport � la position actuel du GameObject
    /// </summary>
    /// <param name="position">Position de destination pour calculer le temps de trajectoire</param>
    /// <returns>Temps pour effectuer la trajectoire jusqu'au point sp�cifi�</returns>
    public float TimeReachingPosition(Vector3 position)
    {
        return Mathf.Abs((this.transform.position - position).magnitude) / speed;
    }
    #endregion
}
