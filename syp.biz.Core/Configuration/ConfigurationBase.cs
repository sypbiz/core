using System;
using System.IO;
using Newtonsoft.Json;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Configuration
{
    public abstract class ConfigurationBase
    {
        #region Fields
        private readonly string _configFilePath;
        #endregion Fields

        #region Constructors
        protected ConfigurationBase(string configFilePath) => this._configFilePath = configFilePath;

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods
        /// <summary>
        /// Saves configuration to file.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise false.</returns>
        public bool Save()
        {
            try
            {
                var content = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(this._configFilePath, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the configuration from file.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        internal bool Load<T>() where T : ConfigurationBase
        {
            try
            {
                var content = Try.Ignore(() => File.ReadAllText(this._configFilePath));
                if (content.IsNullOrWhiteSpace()) return false;
                var config = JsonConvert.DeserializeObject<T>(content);
                foreach (var property in typeof(T).GetProperties()) property.SetValue(this, property.GetValue(config));
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }
        #endregion Methods
    }

    public abstract class ConfigurationBase<T> : ConfigurationBase where T : ConfigurationBase, new()
    {
        #region Fields
        private static readonly Lazy<T> LazyCurrent = new Lazy<T>(() =>
        {
            var rslt = new T();
            if (!rslt.Load<T>()) rslt.Save();
            return rslt;
        });
        #endregion Fields

        #region Constructors
        protected ConfigurationBase(string configFileDirectory, string configName) : base(Path.Combine(Path.GetDirectoryName(configFileDirectory) ?? "", configName)) { }

        protected ConfigurationBase(Type type) : this(type.Assembly.Location, $"{type.FullName}.json") { }

        protected ConfigurationBase() : this(typeof(T)) { }
        #endregion Constructors

        #region Properties
        public static T Current => LazyCurrent.Value;
        #endregion Properties

        #region Methods
        /// <summary>
        /// Loads the configuration from file.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public bool Load() => this.Load<T>();
        #endregion Methods

        #region Overrides of Object
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public sealed override string ToString() => JsonConvert.SerializeObject(this, Formatting.None);
        #endregion Overrides of Object
    }
}

