using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnitySphereCollider;

[TaskCategory("Basic/SphereCollider")]
[TaskDescription("Stores the radius of the SphereCollider. Returns Success.")]
public class GetRadius : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The radius of the SphereCollider")]
	[RequiredField]
	public SharedFloat storeValue;

	private SphereCollider sphereCollider;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			sphereCollider = defaultGameObject.GetComponent<SphereCollider>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if ((Object)(object)sphereCollider == (Object)null)
		{
			Debug.LogWarning((object)"SphereCollider is null");
			return (TaskStatus)1;
		}
		((SharedVariable<float>)storeValue).Value = sphereCollider.radius;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		storeValue = 0f;
	}
}
