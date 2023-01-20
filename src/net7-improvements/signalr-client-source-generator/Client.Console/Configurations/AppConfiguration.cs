namespace Client.Console.Configurations;

internal class AppConfiguration
{
    public SignalRRegistrationType SignalRRegistrationType { get; set; }
}

internal enum SignalRRegistrationType
{
    Classic = 1,
    SourceGenerator = 2
}