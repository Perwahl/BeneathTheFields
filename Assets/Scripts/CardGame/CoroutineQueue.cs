using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Data/CoroutineQueue", order = 1)]
public class CoroutineQueue: ScriptableObject
{
    public MonoBehaviour m_Owner = null;
    Coroutine m_InternalCoroutine = null;
    [SerializeField] private Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public CoroutineQueue(MonoBehaviour aCoroutineOwner)
    {
        m_Owner = aCoroutineOwner;
    }
    public void StartLoop()
    {
        actions = new Queue<IEnumerator>();
        m_InternalCoroutine = m_Owner.StartCoroutine(Process());
    }
    public void StopLoop()
    {
        m_Owner.StopCoroutine(m_InternalCoroutine);
        m_InternalCoroutine = null;
    }
    public void EnqueueAction(IEnumerator aAction)
    {
       // Debug.Log(aAction);
        actions.Enqueue(aAction);
    }

    private IEnumerator Process()
    {
        while (true)
        {
            if (actions.Count > 0)
                yield return m_Owner.StartCoroutine(actions.Dequeue());
            else
                yield return null;
        }
    }

    public void EnqueueWait(float aWaitTime)
    {
       // Debug.Log(aWaitTime);
        actions.Enqueue(Wait(aWaitTime));
    }

    private IEnumerator Wait(float aWaitTime)
    {
        yield return new WaitForSeconds(aWaitTime);
    }
}