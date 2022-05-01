using System.Collections.Concurrent;

class ConcurrentRingBuffer<T>
{
    private ConcurrentQueue<T> _queue;
    public ConcurrentRingBuffer(int capacity)
    {
        _queue = new ConcurrentQueue<T>();
    }
    public void Enqueue(T data) => _queue.Enqueue(data);
    public bool TryDequeue(out T result, int tail)
    {
        while (_queue.Count >= tail)
        {
            _queue.TryDequeue(out result);
        }
        return _queue.TryDequeue(out result);
    }
}
