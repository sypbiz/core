using System.Threading.Tasks;
using syp.biz.Core.Interfaces.Dependency;

namespace syp.biz.Core.Interfaces
{
    /// <summary>
    /// Implement this interface in order for the module to be automatically activated.
    /// </summary>
    public interface IActivatable
    {
        /// <summary>
        /// Name of the module. Usually the full name of the type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Version of the module. Usually the version of the module's assembly.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Called when the module is to be activated.
        /// </summary>
        /// <param name="injector">Provides the global dependency injector.</param>
        /// <returns><c>true</c> if activation was successful; Otherwise <c>false</c>.</returns>
        Task<bool> Activate(IDependencyInjector injector);

        /// <summary>
        /// Called when the module is to be deactivated (before shutdown).
        /// </summary>
        /// <returns><c>true</c> if deactivation was successful; Otherwise <c>false</c>.</returns>
        Task<bool> Deactivate();
    }
}
