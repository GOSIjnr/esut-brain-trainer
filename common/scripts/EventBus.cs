using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class EventBus : Node
{
	private readonly Dictionary<string, List<EventListener>> _eventListenersMap = [];

	public enum EventPriority
	{
		High,
		Medium,
		Low,
	}

	private class EventListener(Delegate invocation, bool oneTime, EventPriority priority, int priorityOrder)
	{
		public Delegate Invocation { get; } = invocation;
		public bool OneTime { get; } = oneTime;
		public EventPriority Priority { get; } = priority;
		public int PriorityOrder { get; } = priorityOrder;
		private readonly WeakReference<object> _targetRef = invocation.Target != null ? new WeakReference<object>(invocation.Target) : null;
		public bool IsValid => _targetRef == null || _targetRef.TryGetTarget(out _);
	}

	private List<EventListener> RetrieveListeners(string eventName)
	{
		_eventListenersMap.TryGetValue(eventName, out var listeners);
		return listeners;
	}

	private void ExecuteListeners(List<EventListener> listenerList, object eventPayload = null)
	{
		if (listenerList == null || listenerList.Count == 0)
		{
			Logger.Log("ExecuteListeners: Listener list is null; skipping invocation.", Logger.LogLevel.Error);
			return;
		}

		var obsoleteListeners = new List<EventListener>();

		foreach (var listener in listenerList.OrderBy(l => l.Priority).ThenByDescending(l => l.PriorityOrder))
		{
			if (!listener.IsValid)
			{
				obsoleteListeners.Add(listener);
				Logger.Log($"Listener for method {listener.Invocation.Method.Name} is invalid and will be removed.", Logger.LogLevel.Debug);
				continue;
			}

			var parametersInfo = listener.Invocation.Method.GetParameters();
			var listenerTargetType = listener.Invocation.Target?.GetType().Name ?? "Static Method";

			if (parametersInfo.Length == 0 && eventPayload == null)
			{
				if (listener.Invocation is Action action)
				{
					Logger.Log($"Invoking listener {listenerTargetType}:{listener.Invocation.Method.Name} with no parameters.", Logger.LogLevel.Debug);
					action();
				}
			}
			else if (parametersInfo.Length == 1 && parametersInfo[0].ParameterType.IsInstanceOfType(eventPayload))
			{
				if (listener.Invocation is Action<object> actionWithParam)
				{
					Logger.Log($"Invoking listener {listenerTargetType}:{listener.Invocation.Method.Name} expecting {parametersInfo[0].ParameterType.Name} with provided payload type {(eventPayload == null ? "null" : eventPayload.GetType().Name)}.", Logger.LogLevel.Debug);
					actionWithParam(eventPayload);
				}
			}
			else
			{
				Logger.Log($"Listener {listenerTargetType}:{listener.Invocation.Method.Name} expected parameter type '{(parametersInfo.Length == 1 ? parametersInfo[0].ParameterType.Name : "none")}', but received payload type '{(eventPayload == null ? "null" : eventPayload.GetType().Name)}'. Invocation skipped.", Logger.LogLevel.Warning);
			}

			if (listener.OneTime)
			{
				obsoleteListeners.Add(listener);
			}
		}

		foreach (var listener in obsoleteListeners)
		{
			string listenerName = $"{listener.Invocation.Target?.GetType().Name ?? "Static Method"}:{listener.Invocation.Method.Name}";
			Logger.Log($"Removing obsolete listener {listenerName} from event subscription.", Logger.LogLevel.Debug);
			listenerList.Remove(listener);
		}

		if (listenerList.Count == 0)
		{
			var eventKey = _eventListenersMap.FirstOrDefault(pair => pair.Value == listenerList).Key;
			Logger.Log($"No active listeners remain for event '{eventKey}'. Removing event from subscription map.", Logger.LogLevel.Debug);
			_eventListenersMap.Remove(_eventListenersMap.FirstOrDefault(pair => pair.Value == listenerList).Key);
		}
	}

	private void RemoveListener(string eventName, Delegate action)
	{
		if (action == null)
		{
			Logger.Log("Provided delegate is null. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		if (_eventListenersMap.TryGetValue(eventName, out var listenerList))
		{
			int countBefore = listenerList.Count;
			listenerList.RemoveAll(item => Equals(item.Invocation, action) || !item.IsValid);
			int countAfter = listenerList.Count;

			Logger.Log($"Listener {action.Target?.GetType().Name}:{action.Method.Name} removed from event '{eventName}'. Count reduced from {countBefore} to {countAfter}.", Logger.LogLevel.Debug);

			if (listenerList.Count == 0)
			{
				_eventListenersMap.Remove(eventName);
				Logger.Log($"No listeners remain for event '{eventName}'. Entry removed.", Logger.LogLevel.Debug);
			}
		}
	}

	private void RecordSubscription(string eventName, Delegate action, bool oneTime, string callerMethod, string callerFile, int callerLine)
	{
		string callerDetails = $"{callerMethod} in {callerFile}:{callerLine}";
		Logger.Log($"Subscribed listener {action.Target?.GetType().Name}:{action.Method.Name} to event '{eventName}' (OneTime: {oneTime}). Called from {callerDetails}.", Logger.LogLevel.Debug);
	}

	private void RecordUnsubscription(string eventName, Delegate action, string callerMethod, string callerFile, int callerLine)
	{
		string callerDetails = $"{callerMethod} in {callerFile}:{callerLine}";
		Logger.Log($"Unsubscribed listener {action.Target?.GetType().Name}:{action.Method.Name} from event '{eventName}'. Action initiated from {callerDetails}.", Logger.LogLevel.Debug);
	}

	private void RecordEventPublish(string eventName, string callerMethod, string callerFile, int callerLine)
	{
		string callerDetails = $"{callerMethod} in {callerFile}:{callerLine}";
		Logger.Log($"Publishing event '{eventName}' initiated from {callerDetails}.", Logger.LogLevel.Debug);
	}

	private void AddNewListener(string eventName, EventListener listener)
	{
		if (listener == null)
		{
			Logger.Log("AddNewListener: Provided listener is null. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		if (!_eventListenersMap.TryGetValue(eventName, out var listeners))
		{
			listeners = [];
			_eventListenersMap[eventName] = listeners;
			Logger.Log($"New listener collection created for event '{eventName}'.", Logger.LogLevel.Debug);
		}

		listeners.Add(listener);
		Logger.Log($"Listener {listener.Invocation.Target?.GetType().Name}:{listener.Invocation.Method.Name} added to event '{eventName}'. Total listeners: {listeners.Count}.", Logger.LogLevel.Debug);
	}

	private bool AlreadySubscribed(string eventName, Delegate action)
	{
		if (action == null)
		{
			Logger.Log("Provided delegate is null. Returning false.", Logger.LogLevel.Error);
			return false;
		}

		if (_eventListenersMap.TryGetValue(eventName, out var listeners))
		{
			bool exists = listeners.Any(listener => listener.Invocation == action);

			if (exists)
			{
				Logger.Log($"Listener {action.Target?.GetType().Name}:{action.Method.Name} is already subscribed to event '{eventName}'. Duplicate subscription prevented.", Logger.LogLevel.Warning);
			}

			return exists;
		}

		return false;
	}

	private void DispatchEvent(string eventName, object eventPayload, string callerMethod, string callerFile, int callerLine)
	{
		var listeners = RetrieveListeners(eventName);

		if (listeners == null || listeners.Count == 0)
		{
			Logger.Log($"No listeners registered for event '{eventName}'. Publish operation aborted.", Logger.LogLevel.Warning);
			return;
		}

		RecordEventPublish(eventName, callerMethod, callerFile, callerLine);
		ExecuteListeners(listeners, eventPayload);
	}

	public void Subscribe(string eventName, Action handler, bool oneTime = false, EventPriority priority = EventPriority.Medium, int order = 0, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (handler == null)
		{
			Logger.Log("Provided handler is null. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		if (AlreadySubscribed(eventName, handler)) return;

		var newListener = new EventListener(handler, oneTime, priority, order);
		AddNewListener(eventName, newListener);
		RecordSubscription(eventName, handler, oneTime, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Subscribe<T>(string eventName, Action<T> handler, bool oneTime = false, EventPriority priority = EventPriority.Medium, int order = 0, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (handler == null)
		{
			Logger.Log("Provided handler is null. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		if (AlreadySubscribed(eventName, handler)) return;

		var newListener = new EventListener(handler, oneTime, priority, order);
		AddNewListener(eventName, newListener);
		RecordSubscription(eventName, handler, oneTime, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Unsubscribe(string eventName, Action handler, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (handler == null)
		{
			Logger.Log("Provided handler is null. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		RemoveListener(eventName, handler);
		RecordUnsubscription(eventName, handler, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Unsubscribe<T>(string eventName, Action<T> handler, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (handler == null)
		{
			Logger.Log("Provided handler is null. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Operation aborted.", Logger.LogLevel.Error);
			return;
		}

		RemoveListener(eventName, handler);
		RecordUnsubscription(eventName, handler, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Publish(string eventName, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		DispatchEvent(eventName, null, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Publish<T>(string eventName, T eventPayload, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (string.IsNullOrEmpty(eventName))
		{
			Logger.Log("Provided event name is null or empty. Subscription aborted.", Logger.LogLevel.Error);
			return;
		}

		DispatchEvent(eventName, eventPayload, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void PrintAllActiveEvents()
	{
		bool prevBatchMode = Logger.IsBatchModeEnabled;
		Logger.IsBatchModeEnabled = true;
		Logger.Log("Listing all active events and their listener registrations:");

		if (_eventListenersMap.Count == 0)
		{
			Logger.Log("No active events registered in the subscription map.");
			Logger.IsBatchModeEnabled = prevBatchMode;
			return;
		}

		foreach (var kvp in _eventListenersMap)
		{
			Logger.Log($"Event '{kvp.Key}' has {kvp.Value.Count} listener(s).");

			foreach (var listener in kvp.Value)
			{
				string methodName = listener.Invocation.Method.Name;
				string targetName = listener.Invocation.Target?.GetType().Name ?? "Static Method";
				string validStatus = listener.IsValid ? "Valid" : "Invalid";

				Logger.Log($"		Listener -> [{validStatus}] Method: {methodName} in {targetName}, Priority: {listener.Priority}, Order: {listener.PriorityOrder}, OneTime: {listener.OneTime}");
			}
		}

		Logger.IsBatchModeEnabled = prevBatchMode;
	}

	public void CleanupEvents()
	{
		var eventsToRemove = new List<string>();

		foreach (var (eventName, listeners) in _eventListenersMap)
		{
			int initialCount = listeners.Count;
			listeners.RemoveAll(listener => !listener.IsValid);
			int removedCount = initialCount - listeners.Count;

			if (removedCount > 0)
			{
				Logger.Log($"Removed {removedCount} invalid listener(s) from event '{eventName}'.", Logger.LogLevel.Debug);
			}

			if (listeners.Count == 0)
			{
				eventsToRemove.Add(eventName);
			}
		}

		foreach (var eventName in eventsToRemove)
		{
			_eventListenersMap.Remove(eventName);
			Logger.Log($"Event '{eventName}' had no active listeners and was removed.", Logger.LogLevel.Debug);
		}
	}
}
