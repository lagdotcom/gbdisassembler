using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GBLib
{
    [Serializable]
    public class CustomOperandList : Dictionary<int, IOperand>
    {
        public CustomOperandList() : base()
        {
        }

        protected CustomOperandList(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}