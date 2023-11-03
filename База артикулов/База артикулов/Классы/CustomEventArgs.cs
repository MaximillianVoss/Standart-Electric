using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Классы
{
    public class CustomEventArgs : EventArgs
    {
        public bool isList { get; }
        public Type type { get; }
        public object Data { get; set; }
        public CustomEventArgs(object data)
        {
            this.isList = data is IList;
            this.type = data.GetType();
            Data = data;
        }

    }
}
