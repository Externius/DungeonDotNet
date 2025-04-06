using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;

namespace RDMG.Core.Tests;

public class TestFixture : IDisposable
{
    public readonly IDungeonService DungeonService;
    public readonly IOptionService OptionService;
    public readonly IOptionRepository OptionRepository;
    public readonly IDungeonNoCorridor DungeonNoCorridor;
    private readonly TestEnvironment _env = new();
    private bool _disposedValue;

    public TestFixture()
    {
        DungeonService = _env.GetService<IDungeonService>();
        OptionService = _env.GetService<IOptionService>();
        OptionRepository = _env.GetService<IOptionRepository>();
        DungeonNoCorridor = _env.GetNcDungeon();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
        {
            _env.Dispose();
        }

        _disposedValue = true;
    }

    ~TestFixture()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}