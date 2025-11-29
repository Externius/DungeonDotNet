using Microsoft.Extensions.Options;
using RDMG.Core.Abstractions.Configuration;
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
    public readonly IUserService UserService;
    public readonly IOptions<AppConfig> Config;
    private readonly TestEnvironment _env = new();
    private bool _disposedValue;

    public TestFixture()
    {
        DungeonService = _env.GetService<IDungeonService>();
        OptionService = _env.GetService<IOptionService>();
        OptionRepository = _env.GetService<IOptionRepository>();
        UserService = _env.GetService<IUserService>();
        Config = _env.GetService<IOptions<AppConfig>>();
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