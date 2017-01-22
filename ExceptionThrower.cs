using System;

namespace webapisample
{
    public interface IExceptionThrower
    {
        void ThrowNotImplementedException();
    }

    public class ExceptionThrower: IExceptionThrower
    {
        public void ThrowNotImplementedException()
        {
            throw new NotImplementedException("Not implemented!");
        }
    }
}