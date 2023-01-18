using System.Text;

namespace template.Code.Helpers;


public class StringWriterHelperClass
{    public sealed class ExtentedStringWriter : StringWriter
    {
        private readonly Encoding stringWriterEncoding;
        public ExtentedStringWriter(StringBuilder builder, Encoding desiredEncoding)
            : base(builder)
        {
            this.stringWriterEncoding = desiredEncoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return this.stringWriterEncoding;
            }
        }
    }
}