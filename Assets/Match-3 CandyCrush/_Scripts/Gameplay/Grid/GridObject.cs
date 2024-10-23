using System.Collections;
using UnityEngine;

namespace Match3
{
	public class GridObject<T>
	{
		public class Gem
		{

		}
		private GridSystem2D<GridObject<T>> _grid;
		private T _gem;
		private int _x;
		private int _y;

		public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y)
		{
			_grid = grid;
			_x = x;
			_y = y;
		}
		public void SetValue(T gem)
		{
			_gem = gem;
		}
		public T GetValue() => _gem;
	}
}