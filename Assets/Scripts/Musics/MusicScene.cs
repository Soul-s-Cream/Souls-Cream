using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicFilter
{
    TOUS,
    BLANC,
    NOIR
}

public class MusicScene : MonoBehaviour
{
    #region Public Fields
    [Tooltip("Filtre qui peut entendre la musique dans la sc�ne")]
    public MusicFilter filter;
    [Tooltip("L'�v�nement post� pour lancer la musique")]
    public AK.Wwise.Event music;
    [Range(100, 5000)]
    [Tooltip("Temps en millisecondes pour le fade out de la musique lorsqu'arr�t�e")]
    public int fadeOutTime = 2000;
    [Tooltip("La musique est-elle arr�t�e quand le jeu change de sc�ne ?")]
    public bool stopOnDestroy = true;
    #endregion

    #region Private Fields
    private delegate void MusicEvent(MusicScene music);
    /// <summary>
    /// Event called when all music must be stopped (generally when a music want to be played)
    /// </summary>
    private event MusicEvent stopAllMusic;
    /// <summary>
    /// An event triggered when a music is trying to be played, but is already playing
    /// </summary>
    private event MusicEvent musicAlreadyPlaying;

    private bool isPlaying = false;
    #endregion

    void Start()
    {
        //En fonction du filtre, on d�termine si on doit lancer la musique ou non
        if(filter == MusicFilter.TOUS)
            PlayMusic();
        else if(GameManager.Instance.localPlayerInstance != null)
        {
            switch(filter)
            {
                case MusicFilter.BLANC:
                    if (GameManager.Instance.localPlayerInstance.GetComponent<Player>().role == Role.BLANC)
                        PlayMusic();
                    break;
                case MusicFilter.NOIR:
                    if (GameManager.Instance.localPlayerInstance.GetComponent<Player>().role == Role.NOIR)
                        PlayMusic();
                    break;
            }
        }
    }

    /// <summary>
    /// Remove all listeners from this object
    /// </summary>
    void RemoveListeners()
    {
        stopAllMusic -= OnStopAllMusic;
        musicAlreadyPlaying -= OnMusicAlreadyPlaying;
    }

    #region Music Control
    /// <summary>
    /// Play the music linked by posting an 
    /// </summary>
    void PlayMusic()
    {
        //On s'abonne pour s'arr�ter imm�diatemment si la musique est d�j� jou�e
        musicAlreadyPlaying += OnMusicAlreadyPlaying;
        //On demande l'arr�t de toutes les autres musiques pour �viter une superposition
        stopAllMusic?.Invoke(this);
        isPlaying = true;
        stopAllMusic += OnStopAllMusic;
        music.Post(gameObject);
    }

    /// <summary>
    /// Stop the music linked by calling the Stop function of the WWise Event in attribute
    /// </summary>
    void StopMusic()
    {
        isPlaying = false;
        music.Stop(gameObject, fadeOutTime);
        RemoveListeners();
    }
    #endregion

    #region Callbacks
    
    void OnStopAllMusic(MusicScene music)
    {
        //Si l'�v�nement provient de cet objet, ou bien que la musique qui demande � �tre est d�j� en train d'�tre jou�e, on refuse
        if (music.Equals(this) || (music.music.Equals(this.music) && this.isPlaying))
            musicAlreadyPlaying?.Invoke(music);
        else
            StopMusic();
    }

    /// <summary>
    /// Callback when a music is trying to be played, but is already playing. Check if the music is trying to be played from this object,
    /// and if true, stop it.
    /// </summary>
    /// <param name="music">The MusicScene object trying to played an already playing music</param>
    void OnMusicAlreadyPlaying(MusicScene music)
    {
        if (this.Equals(music))
            StopMusic();
    }

    /// <summary>
    /// If the object must stop its music on destroy, it stops it. Always call remove all listeners.
    /// </summary>
    private void OnDestroy()
    {
        if (stopOnDestroy)
            StopMusic();
        else
        {
            RemoveListeners();
        }
    }
    #endregion
}
