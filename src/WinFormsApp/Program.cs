namespace LTNet;
internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Control.CheckForIllegalCrossThreadCalls = true;
        if (args.Contains("--self-test")) { SelfTests.Run(args); return; }
        Application.Run(new frmLauncher());
    }
}
