using PropertyHook;
using System.Runtime.CompilerServices;

namespace Injector
{
    public class DS2Hook : PHook
    {
        public IntPtr SpeedhackDllPtr;
        public string? DllPath { get; init; }

        public DS2Hook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            OnHooked += DS2Hook_OnHooked;
            OnUnhooked += DS2Hook_OnUnhooked;
        }

        private void DS2Hook_OnHooked(object? sender, PHEventArgs e)
        {
            SpeedhackDllPtr = InjectDLL(DllPath);
        }
        private void DS2Hook_OnUnhooked(object? sender, PHEventArgs e)
        {
            SpeedhackDllPtr = IntPtr.Zero;
        }
        public bool IsInjected => Hooked && SpeedhackDllPtr != IntPtr.Zero;

    }

}