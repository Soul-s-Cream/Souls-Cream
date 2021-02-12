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
    [Tooltip("Filtre qui peut entendre la musique dans la scène")]
    public MusicFilter filter;
    [Tooltip("L'événement posté pour lancer la musique")]
    public AK.Wwise.Event music;

    // Start is called before the first frame update
    void Start()
    {
        if(filter == MusicFilter.TOUS)
        {
            PlayMusic();
        }
        else if(GameManager.Instance.localPlayerInstance != null)
        {
            switch(filter)
            {
                case MusicFilter.BLANC:
                    if (GameManager.Instance.localPlayerInstance.GetComponent<Player>().role == Role.BLANC)
                    {
                        PlayMusic();
                    }
                    break;
                case MusicFilter.NOIR:
                    if (GameManager.Instance.localPlayerInstance.GetComponent<Player>().role == Role.NOIR)
                    {
                        PlayMusic();
                    }
                    break;
            }
        }
    }

    void PlayMusic()
    {
        AkSoundEngine.SetState("MusicPlayStop", "Stop");
        AkSoundEngine.SetState("MusicPlayStop", "Play");
        music.Post(gameObject);
    }

}
