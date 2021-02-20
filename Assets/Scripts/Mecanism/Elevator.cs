using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class Elevator : Mecanism
{
    #region Public Fields
    public Vector3 endPoint { get { return m_EndPosition; } set { m_EndPosition = value; } }
    
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
    /// Point de fin de la trajectoire.
    /// </summary>
    [HideInInspector]
    [SerializeField]
    private Vector3 m_EndPosition;
    /// <summary>
    /// Destination de la trajectoire actuelle
    /// </summary>
    private Vector3 destination;
    /// <summary>
    /// Le Tween actuel charg� de d�placer l'�l�vateur
    /// </summary>
    private Tween tweenRunning;
    #endregion

    private void Awake()
    {
        //On d�finit le point de d�part comme �tant la position actuelle, et la prochaine destination comme �tant le point de fin
        startPoint = this.transform.position;
        destination = endPoint;
    }

    private void Reset()
    {
        m_EndPosition = this.transform.position + Vector3.up * 2;
    }

    #region Mecanism Logic
    /// <summary>
    /// Appel� lorsque l'objet est activ�. L'�l�vateur commence alors un d�placement vers sa prochaine destination
    /// </summary>
    protected override void SwitchOn()
    {
        tweenRunning?.Kill();

        isOn = true;
        MoveToDestination();
    }

    /// <summary>
    /// Appel� lorsque l'objet est d�sactiv�. L'�l�vateur interrompt alors ses trajectoires, 
    /// et en commence une nouvelle en direction de sa position de d�part
    /// </summary>
    protected override void SwitchOff()
    {
        tweenRunning?.Kill();

        isOn = false;
        tweenRunning = transform.DOMove(startPoint, TimeReachingPosition(startPoint))
            .SetEase(trajectoryEasing);
    }
    #endregion

    #region Elevator Logic
    /// <summary>
    /// D�placer l'�l�vateur vers sa prochaine trajectoire. Une fois termin�e, il effectue la trajectoire inverse si
    /// l'�l�vateur est encore actif.
    /// </summary>
    private void MoveToDestination()
    {
        tweenRunning = transform.DOMove(destination, TimeReachingPosition(destination))
            .SetEase(trajectoryEasing)
            .OnComplete(() => { StartCoroutine("DelayBeforeNewTrajectory"); });
    }

    /// <summary>
    /// Coroutine lan�ant la nouvelle trajectoire en d�cidant de la nouvelle destination apr�s que delayBeforeTrajectory se soit �coul�.
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayBeforeNewTrajectory()
    {
        if (isOn)
        {
            yield return new WaitForSeconds(delayBeforeNewTrajectory);
            destination = this.transform.position == startPoint ? endPoint : startPoint;
            MoveToDestination();
        }
    }

    /// <summary>
    /// Calcul le temps n�cessaire pour atteindre une position sp�cifi� par rapport � la position actuel du GameObject
    /// </summary>
    /// <param name="position">Position de destination pour calculer le temps de trajectoire</param>
    /// <returns>Temps pour effectuer la trajectoire jusqu'au point sp�cifi�</returns>
    private float TimeReachingPosition(Vector3 position)
    {
        return Mathf.Abs((this.transform.position - position).magnitude) / speed;
    }
    #endregion
}
