using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecanism : Photon.PunBehaviour
{
    private void Start()
    {
        AddListeners();
    }

    #region Listeners
    void AddListeners()
    {
        GameEvents.Instance.switchOn += OnSwitchOn;
        GameEvents.Instance.switchOff += OnSwitchOff;
    }

    void RemoveListeners()
    {

    }
    #endregion

    /// <summary>
    /// Le comportement de l'objet lorsque activé
    /// </summary>
    protected virtual void SwitchingOn()
    {
    }

    /// <summary>
    /// Le comportement de l'objet lorsque désactivé
    /// </summary>
    protected virtual void SwitchingOff()
    {
    }

    #region Callbacks
    private void OnDestroy()
    {
        RemoveListeners();
    }

    /// <summary>
    /// Quand l'objet reçoit un événement onSwitchOn, il vérifie s'il est parmi les objets qui doivent s'activer
    /// </summary>
    /// <param name="mecanisms">La liste des objets qui doivent être activés </param>
    protected void OnSwitchOn(Mecanism[] mecanisms)
    {
        foreach (Mecanism mecanism in mecanisms)
        {
            if (mecanism == this)
            {
                SwitchingOn();
            }
        }
    }

    /// <summary>
    /// Quand l'objet reçoit un événement onSwitchOff, il vérifie s'il est parmi les objets qui doivent se désactiver
    /// </summary>
    /// <param name="mecanisms">La liste des objets qui doivent être désactivés</param>
    protected void OnSwitchOff(Mecanism[] mecanisms)
    {
        foreach (Mecanism mecanism in mecanisms)
        {
            if (mecanism == this)
            {
                SwitchingOff();
            }
        }
    }
    #endregion
}
