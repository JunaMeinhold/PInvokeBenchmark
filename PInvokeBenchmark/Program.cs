using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class Program
{
    static void Main()
    {
        var summary = BenchmarkRunner.Run<BenchmarkOptimizedStr>();
    }
}

public unsafe partial class Benchmark
{
    private void** pt = (void**)Marshal.AllocHGlobal(sizeof(nint));
    private nint lib;

    [DllImport("add", EntryPoint = "add", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Add(int a, int b);

    [LibraryImport("add", EntryPoint = "add")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial int AddLib(int a, int b);


    public Benchmark()
    {
        lib = NativeLibrary.Load("add.dll");
        pt[0] = (void*)NativeLibrary.GetExport(lib, "add");
      
    }


    [Benchmark(Baseline =true)]
    public void DllImport()
    {
        int a = 5;
        int b = 10;
        int result = Add(a, b);
    }

    [Benchmark]
    public void LibraryImport()
    {
        int a = 5;
        int b = 10;
        int result = AddLib(a, b);
    }

    [Benchmark]
    public void DirectCall()
    {
        int a = 5;
        int b = 10;
        int result = ((delegate* unmanaged[Cdecl]<int, int, int>)pt[0])(a, b);
    }
}

public unsafe partial class BenchmarkStr
{
    private void** pt = (void**)Marshal.AllocHGlobal(sizeof(nint));
    private nint lib;

    [DllImport("add", EntryPoint = "str", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Str(string str, int len);

    [LibraryImport("add", EntryPoint = "str")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial int StrLib([MarshalAs(UnmanagedType.LPStr)]string str, int len);


    public BenchmarkStr()
    {
        lib = NativeLibrary.Load("add.dll");
        pt[0] = (void*)NativeLibrary.GetExport(lib, "str");
    }


    [Benchmark(Baseline = true)]
    public void DllImport()
    {
        string a = "HelloWorld";
        int result = Str(a, a.Length);
    }

    [Benchmark]
    public void LibraryImport()
    {
        string a = "HelloWorld";
        int result = StrLib(a, a.Length);
    }

    [Benchmark]
    public void DirectCall()
    {
        string a = "HelloWorld";


        byte* pStr = null;
        int count = 0;
        if (a != null)
        {
            count = Encoding.UTF8.GetByteCount(a);
            if (count >= 1024)
            {
                pStr = (byte*)Marshal.AllocHGlobal((count + 1) * sizeof(byte));
            }
            else
            {
                byte* pStack = stackalloc byte[count + 1];
                pStr = pStack;
            }
            fixed (char* pA = a)
                Encoding.UTF8.GetBytes(pA, a.Length, pStr, count);
            pStr[count] = 0;
        }

        int result = ((delegate* unmanaged[Cdecl]<byte*, int, int>)pt[0])(pStr, a.Length);

        if (count >= 1024)
        {
            Marshal.FreeHGlobal((nint)pStr);
        }
    }
}


public unsafe partial class BenchmarkOptimizedStr
{
    private void** pt = (void**)Marshal.AllocHGlobal(sizeof(nint));
    private nint lib;

    [DllImport("add", EntryPoint = "str", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Str(byte* str, int len);

    [LibraryImport("add", EntryPoint = "str")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial int StrLib(byte* str, int len);


    public BenchmarkOptimizedStr()
    {
        lib = NativeLibrary.Load("add.dll");
        pt[0] = (void*)NativeLibrary.GetExport(lib, "str");
    }


    [Benchmark(Baseline = true)]
    public void DllImport()
    {
        string a = "HelloWorld";

        byte* pStr = null;
        int count = 0;
        if (a != null)
        {
             count = Encoding.UTF8.GetByteCount(a);
            if (count >= 1024)
            {
                pStr = (byte*)Marshal.AllocHGlobal((count + 1) * sizeof(byte));
            }
            else
            {
                byte* pStack = stackalloc byte[count + 1];
                pStr = pStack;
            }
            fixed (char* pA = a)
                Encoding.UTF8.GetBytes(pA, a.Length, pStr, count);
            pStr[count] = 0;
        }

        int result = Str(pStr, a.Length);

        if (count >= 1024)
        {
            Marshal.FreeHGlobal((nint)pStr);
        }
    }

    [Benchmark]
    public void LibraryImport()
    {
        string a = "HelloWorld";


        byte* pStr = null;
        int count = 0;
        if (a != null)
        {
            count = Encoding.UTF8.GetByteCount(a);
            if (count >= 1024)
            {
                pStr = (byte*)Marshal.AllocHGlobal((count + 1) * sizeof(byte));
            }
            else
            {
                byte* pStack = stackalloc byte[count + 1];
                pStr = pStack;
            }
            fixed (char* pA = a)
                Encoding.UTF8.GetBytes(pA, a.Length, pStr, count);
            pStr[count] = 0;
        }

        int result = StrLib(pStr, a.Length);

        if (count >= 1024)
        {
            Marshal.FreeHGlobal((nint)pStr);
        }
    }

    [Benchmark]
    public void DirectCall()
    {
        string a = "HelloWorld";

        byte* pStr = null;
        int count = 0;
        if (a != null)
        {
            count = Encoding.UTF8.GetByteCount(a);
            if (count >= 1024)
            {
                pStr = (byte*)Marshal.AllocHGlobal((count + 1) * sizeof(byte));
            }
            else
            {
                byte* pStack = stackalloc byte[count + 1];
                pStr = pStack;
            }
            fixed (char* pA = a)
                Encoding.UTF8.GetBytes(pA, a.Length, pStr, count);
            pStr[count] = 0;
        }

        int result = ((delegate* unmanaged[Cdecl]<byte*, int, int>)pt[0])(pStr, a.Length);

        if (count >= 1024)
        {
            Marshal.FreeHGlobal((nint)pStr);
        }
    }
}
