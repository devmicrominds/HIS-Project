using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileMediaServer.Client
{
    public static class Extensions
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            int dwIoControlCode,
            IntPtr InBuffer,
            int nInBufferSize,
            IntPtr OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            [In] ref NativeOverlapped lpOverlapped
        );

        public static bool MarkAsSparseFile(FileStream fileStream)
        {
            int bytesReturned = 0;
            NativeOverlapped lpOverlapped = new NativeOverlapped();
            return DeviceIoControl(
                fileStream.SafeFileHandle,
                590020, //FSCTL_SET_SPARSE,
                IntPtr.Zero,
                0,
                IntPtr.Zero,
                0,
                ref bytesReturned,
                ref lpOverlapped);
        }
    }
}
