using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lag.GBLib
{
    [Serializable]
    public class CustomOperandList : Dictionary<int, Word>
    {
        public CustomOperandList() : base()
        {
        }

        protected CustomOperandList(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}