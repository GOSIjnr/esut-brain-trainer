using Godot;
using Godot.Collections;
using System.Linq;

[GlobalClass]
public partial class ContentScroller : CanvasLayer
{
	[Export] private Array<ContentPage> _contentPages = [];
	[Export] private string _nextPageID;

	private int _currentPage;

	public override void _EnterTree()
	{
		// Remove null pages and log the behavior
		int initialCount = _contentPages.Count;
		_contentPages = [.. _contentPages.Where(page => page != null)];
		int removedCount = initialCount - _contentPages.Count;

		if (removedCount > 0)
		{
			Logger.Log($"{removedCount} null pages were removed from the content pages array.", Logger.LogLevel.Warning);
		}

		foreach (var page in _contentPages)
		{
			page.Visible = false;
			page.OnNextPageRequested += DisplayNextPage;
			page.OnPreviousPageRequested += DisplayPreviousPage;
		}

		if (_contentPages.Count == 0)
		{
			Logger.Log("No content pages available after filtering null pages.", Logger.LogLevel.Warning);
			DisplayNextPage();
			return;
		}

		// Make the first page visible
		_contentPages[0].Visible = true;
	}

	public override void _ExitTree()
	{
		foreach (var page in _contentPages)
		{
			page.OnNextPageRequested -= DisplayNextPage;
			page.OnPreviousPageRequested -= DisplayPreviousPage;
		}
	}

	private void DisplayNextPage()
	{
		if (_contentPages.Count == 0)
		{
			Logger.Log("No content pages available to display. Switching to the next scene.", Logger.LogLevel.Warning);
			Core.Instance.SceneManager.SwitchScene(_nextPageID, "None");
			return;
		}

		_contentPages[_currentPage].Visible = false;
		_currentPage++;

		if (_currentPage < _contentPages.Count)
		{
			_contentPages[_currentPage].Visible = true;
		}
		else
		{
			Core.Instance.SaveManager.SaveUserData();
			Core.Instance.SceneManager.SwitchScene(_nextPageID, "None");
		}
	}

	private void DisplayPreviousPage()
	{
		if (_currentPage <= 0) return;

		_contentPages[_currentPage].Visible = false;
		_currentPage--;

		if (_currentPage >= 0)
		{
			_contentPages[_currentPage].Visible = true;
		}
	}
}
