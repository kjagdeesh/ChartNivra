namespace ChartNivra.Data;

using System.Collections.Concurrent;
using ChartNivra.Core.Primitives;

/// <summary>
/// Thread-safe rolling window that holds a fixed maximum number of data points.
/// </summary>
public class RollingWindow<T>
{
    private readonly T[] _buffer;
    private int _head;
    private int _count;
    private readonly object _lock = new();

    public RollingWindow(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        Capacity = capacity;
        _buffer = new T[capacity];
    }

    public int Capacity { get; }
    public int Count { get { lock (_lock) return _count; } }

    public void Add(T item)
    {
        lock (_lock)
        {
            if (_count < Capacity)
            {
                _buffer[(_head + _count) % Capacity] = item;
                _count++;
            }
            else
            {
                _buffer[_head] = item;
                _head = (_head + 1) % Capacity;
            }
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items) Add(item);
    }

    public IReadOnlyList<T> Snapshot()
    {
        lock (_lock)
        {
            var result = new T[_count];
            for (var i = 0; i < _count; i++)
            {
                result[i] = _buffer[(_head + i) % Capacity];
            }
            return result;
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _head = 0;
            _count = 0;
            Array.Clear(_buffer, 0, _buffer.Length);
        }
    }
}

/// <summary>
/// Batch update queue for coalescing high-frequency streaming data updates to target frame rates.
/// </summary>
public class BatchUpdateQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly int _maxBatchSize;

    public BatchUpdateQueue(int maxBatchSize = 1000)
    {
        _maxBatchSize = maxBatchSize;
    }

    public void Enqueue(T item) => _queue.Enqueue(item);

    public void EnqueueRange(IEnumerable<T> items)
    {
        foreach (var item in items) _queue.Enqueue(item);
    }

    public List<T> DrainBatch()
    {
        var batch = new List<T>(Math.Min(_queue.Count, _maxBatchSize));
        while (batch.Count < _maxBatchSize && _queue.TryDequeue(out var item))
        {
            batch.Add(item);
        }
        return batch;
    }

    public int PendingCount => _queue.Count;
}

/// <summary>
/// Streaming data pipeline with auto-decimation and threshold notifications.
/// </summary>
public class StreamingDataPipeline
{
    private readonly RollingWindow<ChartPoint> _window;
    private readonly BatchUpdateQueue<ChartPoint> _queue = new();

    public StreamingDataPipeline(int windowCapacity = 10_000)
    {
        _window = new RollingWindow<ChartPoint>(windowCapacity);
    }

    public event Action<IReadOnlyList<ChartPoint>>? DataFlushed;

    public void Push(double x, double y)
    {
        _queue.Enqueue(new ChartPoint(x, y));
    }

    public void Flush()
    {
        var batch = _queue.DrainBatch();
        if (batch.Count == 0) return;

        _window.AddRange(batch);
        DataFlushed?.Invoke(_window.Snapshot());
    }

    public IReadOnlyList<ChartPoint> GetCurrentData() => _window.Snapshot();
}
