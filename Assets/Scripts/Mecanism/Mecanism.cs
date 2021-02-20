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
    /// Le comportement de l'objet lorsque activ�
    /// </summary>
    protected virtual void SwitchingOn()
    {
    }

    /// <summary>
    /// Le comportement de l'objet lorsque d�sactiv�
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
    /// Quand l'objet re�oit un �v�nement onSwitchOn, il v�rifie s'il est parmi les objets qui doivent s'activer
    /// </summary>
    /// <param name="mecanisms">La liste des objets qui doivent �tre activ�s </param>
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
    /// Quand l'objet re�oit un �v�nement onSwitchOff, il v�rifie s'il est parmi les objets qui doivent se d�sactiver
    /// </summary>
    /// <param name="mecanisms">La liste des objets qui doivent �tre d�sactiv�s</param>
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
