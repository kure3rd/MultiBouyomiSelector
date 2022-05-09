using System.Collections.Concurrent;

class ConcurrentRingBuffer<T>
{
    private ConcurrentQueue<T> _queue;
    private int length;
    public ConcurrentRingBuffer(int capacity, int length)
    {
        _queue = new ConcurrentQueue<T>();
    }
    public void Enqueue(T data) => _queue.Enqueue(data);
    public bool TryDequeue(out T result)
    {
        while (_queue.Count >= length)
        {
            _queue.TryDequeue(out result);
        }
        return _queue.TryDequeue(out result);
    }
}
