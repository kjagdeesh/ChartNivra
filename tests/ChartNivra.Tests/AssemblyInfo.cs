using Xunit;

// Timing-sensitive performance checks should not compete with rendering tests.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
