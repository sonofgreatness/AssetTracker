using AssetLocater.Domain.Models;
using AssetLocater.Domain.Services;

public sealed class VehicleIndexHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly VehicleIndex _index;

    public VehicleIndexHostedService(IServiceProvider provider, VehicleIndex index)
    {
        _provider = provider;
        _index = index;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var fileService = scope.ServiceProvider.GetRequiredService<FileService>();

        var file = await fileService.GetVehiclesCsvAsync();
        if (file?.Content == null)
            return;

        var records = CsvVehicleParser.Parse(file.Content);

        var newIndex = VehicleIndexBuilder.Build(records);

        // atomic swap
        _index.Clear();
        Copy(newIndex, _index);
    }

    private static void Copy(VehicleIndex source, VehicleIndex target)
    {
        foreach (var kv in source.ByNationalId)
            target.ByNationalId[kv.Key] = kv.Value;

        foreach (var kv in source.ByNameTokens)
            target.ByNameTokens[kv.Key] = kv.Value;

        foreach (var kv in source.ByCompanyNGram)
            target.ByCompanyNGram[kv.Key] = kv.Value;
    }
}
