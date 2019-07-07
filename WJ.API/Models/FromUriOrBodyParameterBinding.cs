using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace WJ.API.Models
{
    public class FromUriOrBodyParameterBinding : HttpParameterBinding
    {
        HttpParameterBinding _defaultUriBinding;
        HttpParameterBinding _defaultFormatterBinding;

        public FromUriOrBodyParameterBinding(HttpParameterDescriptor desc)
            : base(desc)
        {
            _defaultUriBinding = new FromUriAttribute().GetBinding(desc);
            _defaultFormatterBinding = new FromBodyAttribute().GetBinding(desc);
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Content != null && actionContext.Request.Content.Headers.ContentLength > 0)
            {
                return _defaultFormatterBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            }
            else
            {
                return _defaultUriBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            }
        }
    }
}