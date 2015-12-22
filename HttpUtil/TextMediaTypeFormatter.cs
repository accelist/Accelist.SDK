using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace HttpUtil
{
    /// <summary>
    /// Represents MediaTypeFormatter to handle plain text and XML-RPC contents.
    /// </summary>
    public class TextMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        /// Creates a new instance of TextMediaTypeFormatter.
        /// </summary>
        public TextMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            //SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
        }

        /// <summary>
        /// Called during deserialization to read an object of the specified type from the specified stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="readStream"></param>
        /// <param name="content"></param>
        /// <param name="formatterLogger"></param>
        /// <returns></returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            try
            {
                var memoryStream = new MemoryStream();
                readStream.CopyTo(memoryStream);
                var s = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                taskCompletionSource.SetResult(s);
            }
            catch (Exception e)
            {
                taskCompletionSource.SetException(e);
            }
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="writeStream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
        {
            var buff = System.Text.Encoding.UTF8.GetBytes(value.ToString());
            return writeStream.WriteAsync(buff, 0, buff.Length, cancellationToken);
        }

        /// <summary>
        /// Determines whether this TextMediaTypeFormatter can read objects of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Determines whether this TextMediaTypeFormatter can write objects of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            return type == typeof(string);
        }
    }
}