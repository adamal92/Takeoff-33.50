namespace MetalLimb.Tests
{
    internal class BadBoyStream : MemoryStream
    {
        // Source - https://stackoverflow.com/a/75772747
        // Posted by Alex from Jitbit, modified by community. See post 'Timeline' for change history
        // Retrieved 2026-11-22, License - CC BY-SA 4.0

        //public class BadBoyStream : MemoryStream
        public BadBoyStream(byte[] buffer) : base(buffer) { }

        public override bool CanSeek => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => base.Position;
            set => throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotSupportedException();
    }
}
