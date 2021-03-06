using System.Collections.Generic;
using Nano.Api.Requests.Interfaces;
using Nano.Models.Interfaces;

namespace Nano.Api.Requests
{
    /// <summary>
    /// Create Request.
    /// </summary>
    public class CreateRequest<TEntity> : BaseRequest, IRequestJson
        where TEntity : IEntityCreatable
    {
        /// <summary>
        /// Entity.
        /// </summary>
        public virtual TEntity Entity { get; set; }

        /// <inheritdoc cref="IRequest.GetQueryStringParameters()"/>
        public override IList<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            return new List<KeyValuePair<string, string>>();
        }
    }
}