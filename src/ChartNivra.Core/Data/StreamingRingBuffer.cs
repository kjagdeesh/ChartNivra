namespace ChartNivra.Core.Data;

using ChartNivra.Core.Primitives;
using System.Collections.Concurrent;

/// <summary>
/// High-throughput, thread-safe streaming ring buffer for real-time sensor and financial data.
/// Automatically evicts older points when capacity is reached with zero memory re-allocations.
/// </summary>
public class StreamingRingBuffer
{
    private readonly ChartPoint[] _buffer;
    private int _head = 0;
    private int _count = 0;
    private readonly object _lock = new();

    public int Capacity { get; }
    public int Count { get { lock (_lock) { return _count; } } }

    public StreamingRingBuffer(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        Capacity = capacity;
        _buffer = new ChartPoint[capacity];
    }

    public void Enqueue(ChartPoint point)
    {
        lock (_lock)
        {
            _buffer[_head] = point;
            _head = (_head + 1) % Capacity;
            if (_count < Capacity)
            {
                _count++;
            }
        }
    }

    public void EnqueueBatch(IEnumerable<ChartPoint> points)
    {
        lock (_lock)
        {
            foreach (var pt in points)
            {
                _buffer[_head] = pt;
                _head = (_head + 1) % Capacity;
                if (_count < Capacity)
                {
                    _count++;
                }
            }
        }
    }

    public IReadOnlyList<ChartPoint> ToSnapshot()
    {
        lock (_lock)
        {
            if (_count == 0) return Array.Empty<ChartPoint>();

            var result = new ChartPoint[_count];
            var start = (_head - _count + Capacity) % Capacity;

            for (var i = 0; i < _count; i++)
            {
                result[i] = _buffer[(start + i) % Capacity];
            }

            return result;
        }
    }
}
