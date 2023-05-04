using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ray.Infrastructure.Aop
{
    public class DelimiterInterceptorAttribute : MoAttribute
    {
        private readonly ILogger _logger;
        private readonly string _title;
        private readonly DelimiterScale _delimiterScale;

        public DelimiterInterceptorAttribute(
            string title = null, 
            DelimiterScale delimiterScale = DelimiterScale.M
            )
        {
            _title = title;
            _delimiterScale = delimiterScale;

            _logger = RayGlobal.ServiceProviderRoot.GetRequiredService<ILogger<DelimiterInterceptorAttribute>>(); //todo:lazy load
        }

        public override void OnEntry(MethodContext context)
        {
            if (_title == null) return;

            string end = _delimiterScale == DelimiterScale.L ? Environment.NewLine : "";
            string delimiter = GetDelimiterStr();
            _logger.LogInformation(delimiter + "开始 {taskName} " + delimiter + end, _title);
        }

        public override void OnExit(MethodContext context)
        {
            if (_title == null) return;

            string delimiter = GetDelimiterStr();
            var append = new string(GetDelimiterChar(), _title.Length);

            _logger.LogInformation(delimiter + append + "结束" + append + delimiter + Environment.NewLine);
        }

        private string GetDelimiterStr()
        {
            char delimiter = GetDelimiterChar();

            int count = Convert.ToInt32(_delimiterScale.DefaultValue());
            return new string(delimiter, count);
        }

        private char GetDelimiterChar()
        {
            switch (_delimiterScale)
            {
                case DelimiterScale.L:
                    return '#';
                case DelimiterScale.M:
                    return '-';
                case DelimiterScale.S:
                    return '-';
                default:
                    throw new ArgumentOutOfRangeException(nameof(_delimiterScale), _delimiterScale, null);
            }
        }
    }

    public enum DelimiterScale
    {
        [DefaultValue(5)]
        L,

        [DefaultValue(3)]
        M,

        [DefaultValue(2)]
        S,
    }
}
