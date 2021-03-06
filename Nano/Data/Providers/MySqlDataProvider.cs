using System;
using Microsoft.EntityFrameworkCore;
using Nano.Data.Interfaces;

namespace Nano.Data.Providers
{
    /// <summary>
    /// MySql Data Provider.
    /// </summary>
    public class MySqlDataProvider : IDataProvider
    {
        /// <summary>
        /// Options.
        /// </summary>
        protected virtual DataOptions Options { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">The <see cref="DataOptions"/>.</param>
        public MySqlDataProvider(DataOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.Options = options;
        }

        /// <inheritdoc />
        public void Configure(DbContextOptionsBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var batchSize = this.Options.BatchSize;
            var connectionString = this.Options.ConnectionString;

            builder
                .UseMySql(connectionString, x => x.MaxBatchSize(batchSize));
        }
    }
}