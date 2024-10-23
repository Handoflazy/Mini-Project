using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _click;
    [SerializeField] AudioClip _deselect;
    [SerializeField] AudioClip _match;
    [SerializeField] AudioClip _noMatch;
    [SerializeField] AudioClip _woosh;
    [SerializeField] AudioClip _pop;

    [SerializeField] AudioSource _audioSource;
    private const int MAX_PENDING = 16;
    private int _head;
    private int _tail;
    private AudioClip[] _pending = new AudioClip[MAX_PENDING];
	private void OnValidate()
	{
		if( _audioSource == null )
            _audioSource = GetComponent<AudioSource>();
	}
	private void Start()
	{
        _tail = 0;
        _head = 0;
	}

	public void PlayClick()=>PlaySound(_click);
    public void PlayDeselect() => PlaySound(_deselect);
    public void PlayMatch()=>PlaySound(_match);
    public void PlayNoMatch()=>PlaySound(_noMatch);
    public void PlayWoosh()=>PlaySound(_woosh);
    public void PlayPop()=>PlayRandomPitch(_pop);

    private void PlaySound(AudioClip clip)
    {
        for (int i = _head; i !=_tail; i=(i+1)%MAX_PENDING)
        {
            if (_pending[i] = clip)
            {
                return;
            }
        }
  		if ((_tail+1)%MAX_PENDING!=_head)
        {
            
            _pending[_tail%MAX_PENDING] = clip;
            _tail = (_tail + 1) % MAX_PENDING;

		}
  }
	private void Update()
	{
        if (_head == _tail)
            return;
        _audioSource.PlayOneShot(_pending[_head]);
        _head = (_head + 1) % MAX_PENDING;
	}

	void PlayRandomPitch(AudioClip clip)
    {
        _audioSource.pitch = Random.Range(0.0f, 1.1f);
        _audioSource.PlayOneShot(clip);
        _audioSource.pitch = 1f;
    }
}
