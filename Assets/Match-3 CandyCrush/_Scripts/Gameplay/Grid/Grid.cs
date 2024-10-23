using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEditor.PlayerSettings;

namespace Match3
{

	public class GridSystem2D<T>
	{
		private int _width;
		private int _height;
		private float _cellSize;
		private Vector3 _origin;
		private T[,] _gridArray;

		CoordinateConverter _coordinateConverter;

		public event Action<int, int, T> OnValueChange;
		public static GridSystem2D<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin, bool debug = false)
		{
			return new GridSystem2D<T>(width, height, cellSize, origin, new VerticalConvertor(), debug);
		}
		public static GridSystem2D<T> HorinzontalGrid(int width, int height, float cellSize, Vector3 origin, bool debug = false)
		{
			return new GridSystem2D<T>(width, height, cellSize, origin, new HorizontalConvertor(), debug);
		}

		public GridSystem2D(int width, int height, float cellSize, Vector3 origin, CoordinateConverter coordinateConverter, bool debug)
		{
			_width = width;
			_height = height;
			_cellSize = cellSize;
			_origin = origin;
			_coordinateConverter = coordinateConverter ?? new VerticalConvertor(); // coordinateConveror==null?

			_gridArray = new T[width, height];
			if (debug)
			{
				DrawDebugLines();
			}

		}
		//Set a value form a grid position
		public void SetValue(Vector3 worldPosition, T value)
		{
			Vector2Int pos = _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _origin);
			SetValue(pos.x, pos.y, value);
		}
		public void SetValue(int x, int y, T value)
		{
			if (IsValid(x, y))
			{
				_gridArray[x, y] = value;
				OnValueChange?.Invoke(x, y, value);
			}
		}
		//Get a Value form a grid position
		public T GetValue(Vector3 worldPosition)
		{
			var pos = GetXY(worldPosition);
			return GetValue(pos.x, pos.y);
		}
		public T GetValue(int x, int y)
		{
			return IsValid(x, y) ? _gridArray[x, y] : default;
		}
		private bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < _width && y < _height;
		public Vector3 GetWorldPosition(int x, int y) => _coordinateConverter.GridToWorld(x, y, _cellSize, _origin);
		public Vector3 GetWorldPositionCenter(int x, int y) => _coordinateConverter.GridToWorldCenter(x, y, _cellSize, _origin);
		public Vector2Int GetXY(Vector3 worldPosition) => _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _origin);
		private void DrawDebugLines()
		{
			const float duration = 100f;
			GameObject parent = new GameObject("Debugging");

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					//CreateWorldText(parent, x + "," + y, GetWorldPositionCenter(x, y), _coordinateConverter.Forward, 2, Color.white);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
				}
			}
			Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, duration);
			Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, duration);
		}



		TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
			int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortOrder = 0)
		{
			GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
			gameObject.transform.SetParent(parent.transform);
			gameObject.transform.position = position;
			gameObject.transform.forward = dir;

			TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
			textMeshPro.text = text;
			textMeshPro.color = color;
			//textMeshPro.sortingOrder = sortOrder;
			textMeshPro.fontSize = fontSize;
			textMeshPro.alignment = textAnchor;
			return textMeshPro;
		}
		public abstract class CoordinateConverter
		{
			public abstract Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin);
			public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin);
			public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin);
			public abstract Vector3 Forward { get; }
		}

		public class VerticalConvertor : CoordinateConverter
		{
			public override Vector3 Forward => Vector3.forward;
			public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x, y, 0) * cellSize + origin;
			}
			public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
			}
			public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
			{
				Vector3 gridPosition = (worldPosition - origin) / cellSize;
				var x = Mathf.FloorToInt(gridPosition.x);
				var y = Mathf.FloorToInt(gridPosition.y);
				return new Vector2Int(x, y);
			}
		}
		public class HorizontalConvertor : CoordinateConverter
		{
			public override Vector3 Forward => -Vector3.up;

			public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x, 0, y) * cellSize + origin;
			}


			public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x * cellSize + cellSize * 0.5f, 0, y * cellSize + cellSize * 0.5f) + origin;
			}


			public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
			{
				Vector3 gridPosition = (worldPosition - origin) / cellSize;
				var x = Mathf.FloorToInt(gridPosition.x);
				var y = Mathf.FloorToInt(gridPosition.z);
				return new Vector2Int(x, y);
			}
		}
	}
}