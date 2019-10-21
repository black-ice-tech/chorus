namespace Chorus.DistributedLog.Abstractions
{
    public class LogEntry
    {
        public string StreamName { get; set; }

        public int Partition { get; set; }
    }
}
