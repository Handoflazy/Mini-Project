using Match3.Scriptable;
using UnityEngine;

namespace Match3
{
	public class Gem:MonoBehaviour
	{

		[Header(" Elements ")]
		public GemType Type;
		private SpriteRenderer _spriteRenderer;
		private Animator _animator;
		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_animator = GetComponent<Animator>();
		}
		public void SetGemType(GemType gemType)
		{
			Type = gemType;
			_spriteRenderer.sprite = Type.Sprite;
		}

		public GemType GetGemType() => Type;
		public void Explode() => _animator.SetTrigger("destroy"); //TODO: fIX
		public void DestroyGem()=> Destroy(gameObject);

	}
}