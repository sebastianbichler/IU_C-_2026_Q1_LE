using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Playground.E01G02;

public class Program : Launcher
{
    public override void Run()
    {
        Automat.ProcessPayment();
    }
}
