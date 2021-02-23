using DG.Tweening;
using UnityEngine;

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
    /// Le Tween actuel chargé de déplacer l'élévateur
    /// </summary>
    private Sequence sequenceMoving;
    /// <summary>
    /// Le Tween chargé d'activer sequenceMoving après le délai d'activation;
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
    /// SpriteMask created in case isMasked is true
    /// </summary>
    private SpriteMask mask;
    /// <summary>
    /// BoxCollider2D of the object
    /// </summary>
    private BoxCollider2D boxCollider2D;
    /// <summary>
    /// Original Bounds of the BoxCollider2D of the object. "Theoric" BoxCollider.
    /// </summary>
    private Bounds originalBC2D;
    private Vector3 originalOffset;
    #endregion

    private void Awake()
    {
        //On définit le point de départ comme étant la position actuelle, et la prochaine destination comme étant le point de fin
        startPoint = this.transform.position;
        //On récupère les composants importants pour la logique
        spriteRender = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalBC2D = new Bounds(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
        originalOffset = boxCollider2D.offset;

        //Si l'objet doit être masqué à destination, alors on créer un masque à la destination lorsqu'on est en jeu
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

    private void FixedUpdate()
    {
        ColliderMasking();
    }

    #region Collider Masking
    /// <summary>
    /// Détermine la nouvelle forme du BoxCollider de sorte à masquer les parties qui sont dans le SpriteMask
    /// </summary>
    private void ColliderMasking()
    {
        //J'ai énormément commenté pour que la logique soit aisément compréhensible pour pas que vous vous cassiez la tête comme moi :B Bon courage si vous voulez le changer

        //Si l'objet doit être masqué et est en mouvement...
        if (isMasked && sequenceMoving != null && sequenceMoving.active)
        {
            //...on replace la BoxCollider théorique comme elle devrait être... 
            originalBC2D.center = transform.TransformPoint(-originalOffset);
            Vector2 changes = Vector2.zero;

            //...puis, si le BoxCollider est dans la zone du masque et que l'objet va vers le masque...
            if (toEndPoint && mask.bounds.Intersects(boxCollider2D.bounds))
            {
                //... on calcule le vector Distance boxCollider2D -> mask ...
                Vector3 centersDistance = mask.bounds.center - boxCollider2D.bounds.center;

                //... puis on compare les coordonnées X et Y. Si X est le plus grand ou égal, 
                // et que la taille du BoxCollider en X est supérieur au minimum,
                // alors on ne prend que lui en considération...
                if (Mathf.Abs(centersDistance.x) >= Mathf.Abs(centersDistance.y) && boxCollider2D.size.x > 0.0001f)
                {
                    //On détermine alors le vecteur du changement à effectuer : 
                    //Si la distance en X est positive, alors on fait la distance entre le x de l'arête de gauche du mask 
                    //et le x de l'arête de droite du BoxCollider.
                    //Sinon, on fait la distance entre le x de l'arête de droite du mask et le x de l'arête gauche du BoxCollider
                    //On applique InverseTransform pour convertir à la bonne échelle
                    changes = transform.InverseTransformDirection
                        (
                            new Vector2(
                            centersDistance.x > 0 ?
                            mask.bounds.min.x - boxCollider2D.bounds.max.x :
                            mask.bounds.max.x - boxCollider2D.bounds.min.x
                            , 0)
                        );
                }
                //... sinon se concentre sur l'axe de Y, mais uniquement si la taille en Y du Collide est supérieur au minimum.
                else if (Mathf.Abs(centersDistance.x) < Mathf.Abs(centersDistance.y) && boxCollider2D.size.y > 0.0001f)
                {
                    //On détermine alors le vecteur du changement à effectuer : 
                    //Si la distance en Y est positive, alors on fait la distance entre le y de l'arête bas du mask 
                    //et le y de l'arête haut du BoxCollider.
                    //Sinon, on fait la distance entre le y de l'arête de bas du mask et le y de l'arête haut du BoxCollider.
                    //On applique InverseTransform pour convertir à la bonne échelle
                    changes = transform.InverseTransformVector
                        (
                            new Vector2(0,
                            centersDistance.y > 0 ?
                            mask.bounds.min.y - boxCollider2D.bounds.max.y :
                            mask.bounds.max.y - boxCollider2D.bounds.min.y
                            )
                        );
                }

                //On applique les modifications au BoxCollider.
                //On réduit sa taille de la longueur du vecteur qui représente la zone dans le masque...
                boxCollider2D.size += new Vector2(-Mathf.Abs(changes.x), -Mathf.Abs(changes.y));
                //...et on décale l'offset de sorte à ce qu'il se retrouve centré dans ce qu'il reste du Collider
                boxCollider2D.offset += changes / 2;

                //Si jamais le centre du Collider n'est plus dans le BoxCollider théorique, alors on le place au point le plus proche de la périphérie du BoxCollider théorique
                if (!originalBC2D.Contains((Vector2)transform.position - boxCollider2D.offset))
                    boxCollider2D.offset = -transform.InverseTransformPoint(originalBC2D.ClosestPoint(transform.TransformPoint(-boxCollider2D.offset)));

            }
            //...sinon, si le l'objet s'éloigne du mask, mais que le BoxCollider théorique est dans le mask...
            else if (!toEndPoint && mask.bounds.Intersects(originalBC2D))
            {
                //...on calcule la distance BoxCollider théorique -> mask
                Vector3 centersDistance = mask.bounds.center - originalBC2D.center;

                // Ensuite, on compare le X et le Y de cette distance. 
                // Si X est plus grand ou égal, que la taille en X du BoxCollider est inférieur à celle du BoxCollider théorique et
                // que BoxCollider théorique dépasse en X du mask...
                if (Mathf.Abs(centersDistance.x) >= Mathf.Abs(centersDistance.y)
                    && boxCollider2D.size.x < originalBC2D.size.x
                    && ((originalBC2D.min.x - mask.bounds.min.x) < 0f || (originalBC2D.max.x - mask.bounds.max.x) > 0f))
                {
                    //...alors on calcul le changement à appliquer de la manière suivante :
                    // Si X est positif, alors on fait la distance entre le X de l'arête gauche du mask et le X de l'arête gauche du BoxCollider théorique.
                    // Sinon alors on fait la distance entre le X de l'arête droite du mask et le X de l'arête droite du BoxCollider théorique.
                    // On applique InverseTransform pour convertir à la bonne échelle.
                    changes = transform.InverseTransformDirection
                        (
                            new Vector2(
                            centersDistance.x > 0 ?
                            originalBC2D.min.x - mask.bounds.min.x :
                            originalBC2D.max.x - mask.bounds.max.x
                            , 0)
                        );
                }
                //... sinon, si le Y est inférieur, que la taille en Y du BoxCollider est inférieur à celle du BoxCollider théorique,
                // et que le BoxCollider théorique dépasse en Y du mask
                else if (Mathf.Abs(centersDistance.x) < Mathf.Abs(centersDistance.y)
                    && boxCollider2D.size.y < originalBC2D.size.y
                    && ((originalBC2D.min.y - mask.bounds.min.y) < 0f || (originalBC2D.max.y - mask.bounds.max.y) > 0f))
                {
                    //...alors on calcul le changement à appliquer de la manière suivante :
                    // Si Y est positif, alors on fait la distance entre le Y de l'arête gauche du mask et le Y de l'arête gauche du BoxCollider théorique.
                    // Sinon alors on fait la distance entre le Y de l'arête droite du mask et le Y de l'arête droite du BoxCollider théorique.
                    // On applique InverseTransform pour convertir à la bonne échelle.
                    changes = transform.InverseTransformDirection
                        (
                            new Vector2(0,
                            centersDistance.y > 0 ?
                            originalBC2D.min.y - mask.bounds.min.y :
                            originalBC2D.max.y - mask.bounds.max.y
                            )
                        );
                }

                //On applique les modifications au BoxCollider.
                //On augmente sa taille de la longueur du vecteur qui représente la zone du BoxCollider théorique en dehors du mask...
                boxCollider2D.size += new Vector2(Mathf.Abs(changes.x), Mathf.Abs(changes.y));
                //...et on décale l'offset de sorte à ce qu'il se retrouve centré dans cette zone calculée
                boxCollider2D.offset -= changes / 2;
            }
            // ...sinon si le BoxCollider théorique n'est pas dans le mask,
            // alors on effectue une correction sur le centre du BoxCollider de sorte à ce qu'il soit centré comme il faut
            else if (!mask.bounds.Intersects(originalBC2D))
                boxCollider2D.offset = originalOffset;
        }
    }
    #endregion

    #region Mecanism Logic
    /// <summary>
    /// Appelé lorsque l'objet est activé. L'élévateur commence alors un déplacement vers sa prochaine destination
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
    /// Appelé lorsque l'objet est désactivé. L'élévateur interrompt alors ses trajectoires, 
    /// et en commence une nouvelle en direction de sa position de départ
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
