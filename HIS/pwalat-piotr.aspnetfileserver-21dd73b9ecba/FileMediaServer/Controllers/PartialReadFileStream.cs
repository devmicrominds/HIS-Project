using System;
using System.IO;

namespace FileMediaServer.Controllers
{
    internal class PartialReadFileStream : Stream
    {
        private readonly long _start;
        private readonly long _end;
        private long _position;
        private FileStream _fileStream;
        public PartialReadFileStream(FileStream fileStream, long start, long end)
        {
            _start = start;
            _position = start;
            _end = end;
            _fileStream = fileStream;

            if (start > 0)
            {
                _fileStream.Seek(start, SeekOrigin.Begin);
            }
        }



        public override void Flush()
        {
            _fileStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                _position = _start + offset;
                return _fileStream.Seek(_start + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                _position += offset;
                return _fileStream.Seek(_position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("Seeking from SeekOrigin.End is not implemented");
            }
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position) + 1;
            }
            var result = _fileStream.Read(buffer, offset, byteCountToRead);
            _position += byteCountToRead;
            return result;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position);
            }
            var result = _fileStream.BeginRead(buffer, offset,
                                               count, (s) =>
                                                          {
                                                              _position += byteCountToRead;
                                                              callback(s);
                                                          }, state);
            return result;
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _fileStream.EndRead(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override int ReadByte()
        {
            int result = _fileStream.ReadByte();
            _position++;
            return result;
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return _end - _start; }
        }

        public override long Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _fileStream.Seek(_position, SeekOrigin.Begin);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}