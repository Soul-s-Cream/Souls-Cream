using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class SwitchPressure : Switch
{
    #region Public Fields
    [Tooltip("Le tag de l'objet accept� pour activer l'interrupteur")]
    [TagSelector]
    public string tagFilter = "";
    public AK.Wwise.Event switchSound;
    #endregion

    #region Private Fields
    private Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Quand le bouton est en contact avec le bon objet, ajoute alors un signal positif en entr�e
    /// </summary>
    /// <param name="collision">L'objet en collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagFilter))
        {
            AddInput(true);
            animator.SetBool("BoutonON", true);
            PlaySound();
        }
    }

    /// <summary>
    /// Quand le bouton est en contact avec le bon objet, ajoute alors un signal n�gatif en entr�e
    /// </summary>
    /// <param name="collision">L'objet en collision</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagFilter))
        {
            AddInput(false);
            animator.SetBool("BoutonON", false);
            PlaySound();
        }
    }

    /// <summary>
    /// Son �mis lors d'un changement d'�tat du bouton (appuy� / non appuy�)
    /// </summary>
    private void PlaySound()
    {
        switchSound.Stop(gameObject);
        switchSound.Post(gameObject);
    }
}
