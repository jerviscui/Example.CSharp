using Polly.Telemetry;

namespace ConsolePollyTest;

internal sealed class MyMeteringEnricher : MeteringEnricher
{

    #region Methods

    public override void Enrich<TResult, TArgs>(in EnrichmentContext<TResult, TArgs> context)
    {
        context.Tags.Add(new("my-custom-tag", "custom-value"));
    }

    #endregion

}
