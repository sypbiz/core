//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Jacada.RPA.Common.Utils.Extensions;
//using System.Threading.Tasks.Dataflow;
//
//namespace Jacada.RPA.Common.Utils.DataTypes
//{
//    public class ResourcePool<T> : IDisposable
//    {
//        public delegate Task<T> ResourceFactoryDelegate();
//
//        private readonly CancellationTokenSource _token = new CancellationTokenSource();
//        private readonly BufferBlock<T> _pool;
//        private readonly ResourceFactoryDelegate _factory;
//        private readonly int _min;
//        private readonly int _max;
//        private int _current;
//
//        public ResourcePool(uint max, ResourceFactoryDelegate factory) : this(0, max, factory) { }
//        public ResourcePool(ResourceFactoryDelegate factory) : this(0, uint.MaxValue, factory) { }
//        public ResourcePool(uint min, uint max, ResourceFactoryDelegate factory)
//        {
//            this._min = (int)min;
//            this._max = (int)max;
//            this._pool = new BufferBlock<T>(new DataflowBlockOptions()
//            {
//                BoundedCapacity = this._max,
//                CancellationToken = this._token.Token
//            });
//            this._factory = factory;
//            this._current = 0;
//            this.Init();
//        }
//
//        #region IDisposable
//        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//        public void Dispose()
//        {
//            this._token.Cancel();
//            if (!typeof(IDisposable).IsAssignableFrom(typeof(T)) || !this._pool.TryReceiveAll(out var resources)) return;
//            resources.OfType<IDisposable>().ForEach(r => r.Dispose());
//            this._token.Dispose();
//        }
//        #endregion IDisposable
//
//        public bool AutoGrow { get; set; } = true;
//
//        private void Init()
//        {
//            var tasks = new Task[this._min];
//            for (var i = 0; i < tasks.Length; i++)
//            {
//                tasks[i] = this.AddResource();
//            }
//            Task.WaitAll(tasks);
//        }
//
//        private async Task<T> GetResource()
//        {
//            // try to get resource inline
//            if (this._pool.TryReceive(out var resource)) return resource;
//
//            // no resource available now
//            // grow?
//            if (this.AutoGrow) this.AddResource().NoAwait();
//
//            // wait for resource
//            resource = await this._pool.ReceiveAsync(this._token.Token);
//            return resource;
//        }
//
//        private void ReturnResource(T resource)
//        {
//            this._pool.Post(resource);
//        }
//
//        private async Task<bool> AddResource()
//        {
//            if (this._current >= this._max || !this._pool.Post(await this._factory())) return false;
//            this._current++;
//            return true;
//        }
//
//        public void UseResource(Action<T> action)
//        {
//            var resource = this.GetResource().Result;
//            try
//            {
//                action(resource);
//            }
//            finally
//            {
//                this.ReturnResource(resource);
//            }
//        }
//
//        public TResult UseResource<TResult>(Func<T, TResult> function)
//        {
//            var resource = this.GetResource().Result;
//            try
//            {
//                return function(resource);
//            }
//            finally
//            {
//                this.ReturnResource(resource);
//            }
//        }
//    }
//}
