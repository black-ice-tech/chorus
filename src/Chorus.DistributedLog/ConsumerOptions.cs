using System.Diagnostics.CodeAnalysis;

namespace Chorus.DistributedLog
{
    public class ConsumerOptions
    {
        public bool StopConsumingAtEOF { get; private set; }
        public int StartOffset { get; private set; }
        public string ConsumerGroupId { get; private set; }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Builder classes are well-suited to be nested")]
        public class Builder
        {
            private bool _stopConsumingAtEOF;
            private string _consumerGroupId;
            private int _offset;

            public Builder StopConsumingAtEOF()
            {
                _stopConsumingAtEOF = true;
                return this;
            }

            public Builder WithGroupId(string consumerGroupId)
            {
                _consumerGroupId = consumerGroupId;
                return this;
            }

            public Builder FromOffset(int offset)
            {
                _offset = offset;
                return this;
            }

            public ConsumerOptions Build()
            {
                return new ConsumerOptions
                {
                    StopConsumingAtEOF = _stopConsumingAtEOF,
                    ConsumerGroupId = _consumerGroupId,
                    StartOffset = _offset
                };
            }
        }
    }
}
