﻿using BenchmarkDotNet.Attributes;
using Flecs.NET.Core;

namespace Flecs.NET;

[ShortRunJob]
[BenchmarkCategory(Category.QueryT5)]
// ReSharper disable once InconsistentNaming
public class QueryT5_FlecsNet
{
    private World   world;
    private Query   query;
    
    [GlobalSetup]
    public void Setup() {
        world = World.Create();
        world.CreateEntities(Constants.EntityCount).AddComponents();
        query = world.QueryBuilder().With<Component1>().With<Component2>().With<Component3>().With<Component4>().With<Component5>().Build();
    }
    
    [GlobalCleanup]
    public void Shutdown() {
        world.Dispose();
    }
    
    [Benchmark]
    public void Run()
    {
        query.Each((ref Component1 c1, ref Component2 c2, ref Component3 c3, ref Component4 c4, ref Component5 c5) =>
        {
            c1.Value = c2.Value + c3.Value + c4.Value + c5.Value;
        });
    }
}