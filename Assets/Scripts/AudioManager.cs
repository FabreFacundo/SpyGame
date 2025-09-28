using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    [Header("Songs")]
    [SerializeField] private AudioClip[] songs;

    private AudioClip prevSong;
    private AudioClip nextSong;
    private AudioSource _musicAudioSource;

    private void Start()
    {
        _musicAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TrackRandomizer();
    }

    private void TrackRandomizer()
    {
        if (!_musicAudioSource.isPlaying)
        {

            nextSong = songs[Random.Range(0, songs.Length - 1)];
            prevSong = _musicAudioSource.clip;
            if (prevSong != nextSong)
            {
                _musicAudioSource.clip = nextSong;
                _musicAudioSource.Play();
            }
        }
    }
}
