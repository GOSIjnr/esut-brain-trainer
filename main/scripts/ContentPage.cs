using Godot;

[GlobalClass]
public partial class ContentPage : CanvasLayer
{
	[Signal] public delegate void OnNextPageRequestedEventHandler();
	[Signal] public delegate void OnPreviousPageRequestedEventHandler();
}
