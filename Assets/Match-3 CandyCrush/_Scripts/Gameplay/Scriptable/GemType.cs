using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scriptable
{
	[CreateAssetMenu(fileName ="GemType",menuName ="Match3/GemType")]
	public class GemType : ScriptableObject
	{
		public Sprite Sprite;
	}
}