﻿using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Types;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Arch;

[ShortRunJob]
public class QueryT1
{
    private World   world;
    private Query   query;
    
    [GlobalSetup]
    public void Setup()
    {
        world   = World.Create();
        world.CreateEntities(Constant.EntityCount).AddComponents();
        var queryDescription = new QueryDescription().WithAll<Component1>();
        query = world.Query(in queryDescription);
        Assert.AreEqual(Constant.EntityCount, world.CountEntities(queryDescription));
    }
    
    [GlobalCleanup]
    public void Shutdown()
    {
        World.Destroy(world);
    }
    
    [Benchmark]
    public void Run()
    {
        foreach(ref var chunk in query.GetChunkIterator())
        {
            var components = chunk.GetFirst<Component1>;    // chunk.GetArray, chunk.GetSpan...
            foreach(var entity in chunk)                    // Iterate over each row/entity inside chunk
            {
                ref var _ = ref Unsafe.Add(ref components, entity);
            }
        }
    }
}