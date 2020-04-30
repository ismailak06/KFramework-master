﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KFramework.Core.Utilities.Common
{
    public class WcfProxy<T>
    {
        //http://localhost:59085/{0}.svc
        public static T CreateChannel()
        {
            string baseAddress = ConfigurationManager.AppSettings["ServiceAddress"];
            string address = string.Format(baseAddress, typeof(T).Name.Substring(1));
            var binding = new BasicHttpBinding();
            var channel = new ChannelFactory<T>(binding,address);
            return channel.CreateChannel();
        }
    }
}
