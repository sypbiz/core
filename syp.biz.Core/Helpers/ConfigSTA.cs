using syp.biz.Core.Configuration;

namespace syp.biz.Core.Helpers
{
    internal class ConfigSTA : ConfigurationBase<ConfigSTA>
    {
        /// <summary>
        /// Minimum number of available thread in the <see cref="STATaskScheduler"/>.
        /// </summary>
        public uint MinThreads { get; set; } = 20;

        /// <summary>
        /// Maximum number of available thread in the <see cref="STATaskScheduler"/>.
        /// </summary>
        public uint MaxThreads { get; set; } = 20;

//        /// <summary>
//        /// The interval between checks for possible thread shrinkage.
//        /// </summary>
//        public TimeSpan ShrinkInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}
