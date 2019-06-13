using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestXMLPost
{
    public class XDocumentInputFormatter : InputFormatter, IInputFormatter, IApiRequestFormatMetadataProvider
    {
        public XDocumentInputFormatter()
        {
            SupportedMediaTypes.Add("application/xml");
        }

        protected override bool CanReadType(Type type)
        {
            if (type.IsAssignableFrom(typeof(XDocument))) return true;
            return base.CanReadType(type);
        }

        //public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        //{
        //    var xmlDoc = await XDocument.LoadAsync(context.HttpContext.Request.Body, LoadOptions.None, CancellationToken.None);
        //    return InputFormatterResult.Success(xmlDoc);
        //}
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            // Use StreamReader to convert any encoding to UTF-16 (default C# and sql Server).
            using (var streamReader = new StreamReader(context.HttpContext.Request.Body))
            {
                var xmlDoc = await XDocument.LoadAsync(streamReader, LoadOptions.None, CancellationToken.None);
                return InputFormatterResult.Success(xmlDoc);
            }
        }
    }
}
