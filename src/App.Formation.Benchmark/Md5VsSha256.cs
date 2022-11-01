namespace App.Formation.Benchmark;

using System;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.NativeAot70)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
public class Md5VsSha256
{
    private readonly SHA256 sha256 = SHA256.Create();

    private readonly MD5 md5 = MD5.Create();

    private byte[] data;

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        this.data = new byte[this.N];
        new Random(42).NextBytes(this.data);
    }

    [Benchmark]
    public byte[] Sha256() => this.sha256.ComputeHash(this.data);

    [Benchmark]
    public byte[] Md5() => this.md5.ComputeHash(this.data);
}
