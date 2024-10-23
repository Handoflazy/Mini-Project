using DG.Tweening;
using Match3;
using Match3.Scriptable;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class Match3Test : MonoBehaviour
{

	[Header(" Settings ")]
	[SerializeField] int _width = 8;
	[SerializeField] int _height = 8;
	[SerializeField] float _cellSize = 1f;
	[SerializeField] Vector3 _originPosition;
	[SerializeField] bool _debug = true;
	[SerializeField] Ease Ease = Ease.InQuad;


	[Header(" Elements ")]
	[SerializeField] Gem _gemPrefab;
	[SerializeField] GemType[] _gemTypes;

	[SerializeField] AudioManager _audioManager;


	private GridSystem2D<GridObject<Gem>> _grid;

	private InputReader _inputReader;

	private Vector2Int _selectedGem;

	private Camera _camera;

	private bool _isUpdateGameField=false;

	private void Awake()
	{
		_inputReader = GetComponent<InputReader>();
		_audioManager = GetComponent<AudioManager>();
	}

	private void Start()
	{

		InitializeGrid();
		_inputReader.Fire += OnSelection;
		_inputReader.Release += OnRelease;
		DeselectGem();
		_camera = Camera.main;
	}
	private void OnDisable()
	{
		_inputReader.Fire -= OnSelection;
		_inputReader.Release -= OnRelease;
	}
	private void OnSelection()
	{
		if (_isUpdateGameField)
			return;
		var gridPos = _grid.GetXY(_camera.ScreenToWorldPoint(_inputReader.Selected));
		if (!IsValidPosition(gridPos) && IsEmptyPosition(gridPos))
			return;
		else if (_selectedGem == Vector2Int.one * -1)
		{
			SelectGem(gridPos);
			_audioManager.PlayClick();
		}
	}

	private void OnRelease()
	{
		if (_selectedGem == Vector2Int.one * -1)
			return;
		var neigborGrid = _selectedGem + _inputReader.GetFireDirection();
		if (!IsValidPosition(neigborGrid) && IsEmptyPosition(neigborGrid))
		{
			print(neigborGrid);
			DeselectGem();
			return;
		}
		_isUpdateGameField = true;
		StartCoroutine(RunGameLoop(_selectedGem, neigborGrid));
	}

	private bool IsValidPosition(Vector2Int gridPos) => gridPos is { x: >= 0, y: >= 0 } && gridPos.x <= _width && gridPos.y < -_height;
	private bool IsEmptyPosition(Vector2Int gridPos) => _grid.GetValue(gridPos.x, gridPos.y) == null;

	private bool IsCardinarNeightor(Vector2Int gridPosA, Vector2Int gridPosB)
	{
		return Mathf.Abs(gridPosA.x - gridPosB.x) <= 1 && Mathf.Abs(gridPosA.y - gridPosB.y) <= 1 && Vector2Int.Distance(gridPosA, gridPosB) == 1;
	}
	private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
	{
		DeselectGem();
		yield return StartCoroutine(SwapGem(gridPosA, gridPosB));
		if(FindMatches().Count>0)
			yield return UpdateGem();
		else
			yield return StartCoroutine(SwapGem(gridPosA, gridPosB));
		_isUpdateGameField = false;
		yield return null;
	}

	private IEnumerator UpdateGem() 
	{
		List<Vector2Int> matches = FindMatches();
		while (matches.Count > 0)
		{
			yield return StartCoroutine(ExplodeGem(matches));
			yield return StartCoroutine(MakeGemsFall());
			yield return StartCoroutine(FillEmptySpots());
			matches = FindMatches();

		}
	}

	private IEnumerator FillEmptySpots()
	{
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				if (_grid.GetValue(x, y) == null)
				{
					CreateGem(x, y);
					_audioManager.PlayPop();
					yield return new WaitForSeconds(0.15f);
				}
			}
		}
	}
	private IEnumerator MakeGemsFall()
	{
		var sequence = DOTween.Sequence();
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				if (_grid.GetValue(x, y) == null)
				{
					for (int i = y + 1; i < _height; i++)
					{
						if (_grid.GetValue(x, i) != null)
						{
							_grid.SetValue(x, y, _grid.GetValue(x, i));
							_grid.SetValue(x, i, null);
							var gem = _grid.GetValue(x, y).GetValue();
							gem.transform.DOLocalMove(_grid.GetWorldPositionCenter(x, y), 0.5f).SetEase(Ease);
							_audioManager.PlayWoosh();
							//yield return new WaitForSeconds(0.05f);
							break;
						}
					}
				}
			}
		}
		yield return new WaitForSeconds(0.5f);
	}
	private IEnumerator ExplodeGem(List<Vector2Int> matches)
	{
		foreach (var match in matches)
		{
			_audioManager.PlayPop();
			var gem = _grid.GetValue(match.x, match.y).GetValue();
			_grid.SetValue(match.x, match.y, null);

			// Explode VFX
			gem.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f).OnComplete(() => gem.DestroyGem());

		}
		yield return 0.1 * matches.Count;

	}

	private List<Vector2Int> FindMatches()
	{
		HashSet<Vector2Int> matches = new();

		//Horizontal
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width - 2; x++)
			{
				var gemA = _grid.GetValue(x, y);
				var gemB = _grid.GetValue(x + 1, y);
				var gemC = _grid.GetValue(x + 2, y);
				if (gemA == null || gemB == null || gemC == null)
					continue;
				if (gemA.GetValue().Type == gemB.GetValue().Type && gemB.GetValue().Type == gemC.GetValue().Type)
				{
					matches.Add(new Vector2Int(x, y));
					matches.Add(new Vector2Int(x + 1, y));
					matches.Add(new Vector2Int(x + 2, y));
				}
			}
		}

		//Vertical
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height - 2; y++)
			{
				var gemA = _grid.GetValue(x, y);
				var gemB = _grid.GetValue(x, y + 1);
				var gemC = _grid.GetValue(x, y + 2);
				if (gemA == null || gemB == null || gemC == null)
					continue;
				if (gemA.GetValue().Type == gemB.GetValue().Type && gemB.GetValue().Type == gemC.GetValue().Type)
				{
					matches.Add(new Vector2Int(x, y));
					matches.Add(new Vector2Int(x, y + 1));
					matches.Add(new Vector2Int(x, y + 2));
				}
			}
		}
		if (matches.Count == 0)
		{
			_audioManager.PlayNoMatch();
		}
		return new List<Vector2Int>(matches);

	}

	private IEnumerator SwapGem(Vector2Int gridPosA, Vector2Int gridPosB)
	{
		var gridObjectA = _grid.GetValue(gridPosA.x, gridPosA.y);
		var gridObjectB = _grid.GetValue(gridPosB.x, gridPosB.y);

		gridObjectA.GetValue().transform
			.DOLocalMove(_grid.GetWorldPositionCenter(gridPosB.x, gridPosB.y), 0.5f)
			.SetEase(Ease);
		gridObjectB.GetValue().transform
			.DOLocalMove(_grid.GetWorldPositionCenter(gridPosA.x, gridPosA.y), 0.5f)
			.SetEase(Ease);

		_grid.SetValue(gridPosA.x, gridPosA.y, gridObjectB);
		_grid.SetValue(gridPosB.x, gridPosB.y, gridObjectA);
		yield return new WaitForSeconds(0.5f);
	}

	private void DeselectGem() => _selectedGem = Vector2Int.one * -1;
	private void SelectGem(Vector2Int gridPos) => _selectedGem = gridPos;

	private void InitializeGrid()
	{
		_grid = GridSystem2D<GridObject<Gem>>.VerticalGrid(_width, _height, _cellSize, _originPosition, _debug);

		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				CreateGem(x, y);
			}
		}
		StartCoroutine(UpdateGem());	
	}

	private void CreateGem(int x, int y)
	{
		Gem gem = Instantiate(_gemPrefab, _grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);
		gem.SetGemType(_gemTypes[Random.Range(0, _gemTypes.Length)]);
		var gridObject = new GridObject<Gem>(_grid, x, y);
		gridObject.SetValue(gem);
		_grid.SetValue(x, y, gridObject);
	}
}
