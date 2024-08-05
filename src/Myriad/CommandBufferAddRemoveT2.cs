using BenchmarkDotNet.Attributes;
using Myriad.ECS;
using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad;

[BenchmarkCategory(Category.CommandBufferAddRemoveT2)]
// ReSharper disable once InconsistentNaming
public class CommandBufferAddRemoveT2_Myriad
{
    private World world;
    private Entity[] entities;
    private CommandBuffer cmd;

    [GlobalSetup]
    public void Setup()
    {
        world = new WorldBuilder().Build();

        entities = world.CreateEntities(Constants.EntityCount);

        cmd = new CommandBuffer(world);
    }

    [GlobalCleanup]
    public void Shutdown()
    {
        world.Dispose();
    }

    [Benchmark]
    public void Run()
    {
        foreach (var entity in entities)
        {
            cmd.Set(entity, new Component1());
            cmd.Set(entity, new Component2());
        }
        cmd.Playback().Dispose();

        foreach (var entity in entities)
        {
            cmd.Remove<Component1>(entity);
            cmd.Remove<Component2>(entity);
        }
        cmd.Playback().Dispose();
    }
}