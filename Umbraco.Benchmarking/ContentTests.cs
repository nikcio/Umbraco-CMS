using BenchmarkDotNet.Attributes;

namespace Umbraco.Benchmarking
{
    public class ContentTests
    {
        [GlobalSetup]
        public async Task Setup()
        {
            await BulkClean();
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            await BulkClean();
        }

        private static async Task Clean()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromDays(10),
            };
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/Clean");
        }

        private static async Task BulkClean()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromDays(10),
            };
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkClean");
        }

        // Save and publish

        [Benchmark]
        public async Task ContentSaveAndPublishSingle()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleSingle");
        }

        [Benchmark]
        public async Task BulkContentSaveAndPublishSingle()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleSingle");
        }

        [Benchmark]
        public async Task ContentSaveAndPublishTen()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=10");
        }

        [Benchmark]
        public async Task BulkContentSaveAndPublishTen()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=10");
        }


        [Benchmark]
        public async Task ContentSaveAndPublishTwenty()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=20");
        }

        [Benchmark]
        public async Task BulkContenSaveAndPublishTwenty()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleMulti?count=20");
        }

        [Benchmark]
        public async Task ContentSaveAndPublishFifty()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=50");
        }

        [Benchmark]
        public async Task BulkContentSaveAndPublishFifty()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(10);
            _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleMulti?count=50");
        }

        // LifeCycle

        //[Benchmark]
        //public async Task ContentLifeCycleSingle()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleSingle");
        //    await Clean();
        //}

        //[Benchmark]
        //public async Task BulkContentLifeCycleSingle()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleSingle");
        //    await BulkClean();
        //}

        //[Benchmark]
        //public async Task ContentLifeCycleTen()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=10");
        //    await Clean();
        //}

        //[Benchmark]
        //public async Task BulkContentLifeCycleTen()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleMulti?count=10");
        //    await BulkClean();
        //}

        //[Benchmark]
        //public async Task ContentLifeCycleTwenty()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=20");
        //    await Clean();
        //}

        //[Benchmark]
        //public async Task BulkContentLifeCycleTwenty()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleMulti?count=20");
        //    await BulkClean();
        //}

        //[Benchmark]
        //public async Task ContentLifeCycleFifty()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/ContentLifecycleMulti?count=50");
        //    await Clean();
        //}

        //[Benchmark]
        //public async Task BulkContentLifeCycleFifty()
        //{
        //    using var client = new HttpClient();
        //    client.Timeout = TimeSpan.FromDays(10);
        //    _ = await client.GetAsync("https://localhost:44331/umbraco/api/Content/BulkContentLifecycleMulti?count=50");
        //    await BulkClean();
        //}
    }
}
