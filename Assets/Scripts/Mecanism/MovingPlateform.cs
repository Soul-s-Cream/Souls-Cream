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
        //On d�finit le point de d�part comme �tant la position actuelle, et la prochaine destination comme �tant le point de fin
        startPoint = this.transform.position;
        //On r�cup�re les composants importants pour la logique
        spriteRender = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalBC2D = new Bounds(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
        originalOffset = boxCollider2D.offset;

        //Si l'objet doit �tre masqu� � destination, alors on cr�er un masque � la destination lorsqu'on est en jeu
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
    /// D�termine la nouvelle forme du BoxCollider de sorte � masquer les parties qui sont dans le SpriteMask
    /// </summary>
    private void ColliderMasking()
    {
        //J'ai �norm�ment comment� pour que la logique soit ais�ment compr�hensible pour pas que vous vous cassiez la t�te comme moi :B Bon courage si vous voulez le changer

        //Si l'objet doit �tre masqu� et est en mouvement...
        if (isMasked && sequenceMoving != null && sequenceMoving.active)
        {
            //...on replace la BoxCollider th�orique comme elle devrait �tre... 
            originalBC2D.center = transform.TransformPoint(-originalOffset);
            Vector2 changes = Vector2.zero;

            //...puis, si le BoxCollider est dans la zone du masque et que l'objet va vers le masque...
            if (toEndPoint && mask.bounds.Intersects(boxCollider2D.bounds))
            {
                //... on calcule le vector Distance boxCollider2D -> mask ...
                Vector3 centersDistance = mask.bounds.center - boxCollider2D.bounds.center;

                //... puis on compare les coordonn�es X et Y. Si X est le plus grand ou �gal, 
                // et que la taille du BoxCollider en X est sup�rieur au minimum,
                // alors on ne prend que lui en consid�ration...
                if (Mathf.Abs(centersDistance.x) >= Mathf.Abs(centersDistance.y) && boxCollider2D.size.x > 0.0001f)
                {
                    //On d�termine alors le vecteur du changement � effectuer : 
                    //Si la distance en X est positive, alors on fait la distance entre le x de l'ar�te de gauche du mask 
                    //et le x de l'ar�te de droite du BoxCollider.
                    //Sinon, on fait la distance entre le x de l'ar�te de droite du mask et le x de l'ar�te gauche du BoxCollider
                    //On applique InverseTransform pour convertir � la bonne �chelle
                    changes = transform.InverseTransformDirection
                        (
                            new Vector2(
                            centersDistance.x > 0 ?
                            mask.bounds.min.x - boxCollider2D.bounds.max.x :
                            mask.bounds.max.x - boxCollider2D.bounds.min.x
                            , 0)
                        );
                }
                //... sinon se concentre sur l'axe de Y, mais uniquement si la taille en Y du Collide est sup�rieur au minimum.
                else if (Mathf.Abs(centersDistance.x) < Mathf.Abs(centersDistance.y) && boxCollider2D.size.y > 0.0001f)
                {
                    //On d�termine alors le vecteur du changement � effectuer : 
                    //Si la distance en Y est positive, alors on fait la distance entre le y de l'ar�te bas du mask 
                    //et le y de l'ar�te haut du BoxCollider.
                    //Sinon, on fait la distance entre le y de l'ar�te de bas du mask et le y de l'ar�te haut du BoxCollider.
                    //On applique InverseTransform pour convertir � la bonne �chelle
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
                //On r�duit sa taille de la longueur du vecteur qui repr�sente la zone dans le masque...
                boxCollider2D.size += new Vector2(-Mathf.Abs(changes.x), -Mathf.Abs(changes.y));
                //...et on d�cale l'offset de sorte � ce qu'il se retrouve centr� dans ce qu'il reste du Collider
                boxCollider2D.offset += changes / 2;

                //Si jamais le centre du Collider n'est plus dans le BoxCollider th�orique, alors on le place au point le plus proche de la p�riph�rie du BoxCollider th�orique
                if (!originalBC2D.Contains((Vector2)transform.position - boxCollider2D.offset))
                    boxCollider2D.offset = -transform.InverseTransformPoint(originalBC2D.ClosestPoint(transform.TransformPoint(-boxCollider2D.offset)));

            }
            //...sinon, si le l'objet s'�loigne du mask, mais que le BoxCollider th�orique est dans le mask...
            else if (!toEndPoint && mask.bounds.Intersects(originalBC2D))
            {
                //...on calcule la distance BoxCollider th�orique -> mask
                Vector3 centersDistance = mask.bounds.center - originalBC2D.center;

                // Ensuite, on compare le X et le Y de cette distance. 
                // Si X est plus grand ou �gal, que la taille en X du BoxCollider est inf�rieur � celle du BoxCollider th�orique et
                // que BoxCollider th�orique d�passe en X du mask...
                if (Mathf.Abs(centersDistance.x) >= Mathf.Abs(centersDistance.y)
                    && boxCollider2D.size.x < originalBC2D.size.x
                    && ((originalBC2D.min.x - mask.bounds.min.x) < 0f || (originalBC2D.max.x - mask.bounds.max.x) > 0f))
                {
                    //...alors on calcul le changement � appliquer de la mani�re suivante :
                    // Si X est positif, alors on fait la distance entre le X de l'ar�te gauche du mask et le X de l'ar�te gauche du BoxCollider th�orique.
                    // Sinon alors on fait la distance entre le X de l'ar�te droite du mask et le X de l'ar�te droite du BoxCollider th�orique.
                    // On applique InverseTransform pour convertir � la bonne �chelle.
                    changes = transform.InverseTransformDirection
                        (
                            new Vector2(
                            centersDistance.x > 0 ?
                            originalBC2D.min.x - mask.bounds.min.x :
                            originalBC2D.max.x - mask.bounds.max.x
                            , 0)
                        );
                }
                //... sinon, si le Y est inf�rieur, que la taille en Y du BoxCollider est inf�rieur � celle du BoxCollider th�orique,
                // et que le BoxCollider th�orique d�passe en Y du mask
                else if (Mathf.Abs(centersDistance.x) < Mathf.Abs(centersDistance.y)
                    && boxCollider2D.size.y < originalBC2D.size.y
                    && ((originalBC2D.min.y - mask.bounds.min.y) < 0f || (originalBC2D.max.y - mask.bounds.max.y) > 0f))
                {
                    //...alors on calcul le changement � appliquer de la mani�re suivante :
                    // Si Y est positif, alors on fait la distance entre le Y de l'ar�te gauche du mask et le Y de l'ar�te gauche du BoxCollider th�orique.
                    // Sinon alors on fait la distance entre le Y de l'ar�te droite du mask et le Y de l'ar�te droite du BoxCollider th�orique.
                    // On applique InverseTransform pour convertir � la bonne �chelle.
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
                //On augmente sa taille de la longueur du vecteur qui repr�sente la zone du BoxCollider th�orique en dehors du mask...
                boxCollider2D.size += new Vector2(Mathf.Abs(changes.x), Mathf.Abs(changes.y));
                //...et on d�cale l'offset de sorte � ce qu'il se retrouve centr� dans cette zone calcul�e
                boxCollider2D.offset -= changes / 2;
            }
            // ...sinon si le BoxCollider th�orique n'est pas dans le mask,
            // alors on effectue une correction sur le centre du BoxCollider de sorte � ce qu'il soit centr� comme il faut
            else if (!mask.bounds.Intersects(originalBC2D))
                boxCollider2D.offset = originalOffset;
        }
    }
    #endregion

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
