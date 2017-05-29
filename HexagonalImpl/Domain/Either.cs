using System;

namespace HexagonalImpl.Domain
{
    public class Either<T1, T2>
    {
        private T1 _left;
        private T2 _right;
        private Func<T2, T1> _onError;

        private Either(T1 t1)
        {
            _left = t1;
        }

        private Either(T2 t2)
        {
            _right = t2;
        }

        public static Either<T1, T2> Left(T1 t1)
        {
            return new Either<T1, T2>(t1);
        }

        public static Either<T1, T2> Right(T2 t2)
        {
            return new Either<T1, T2>(t2);
        }

        public Either<T3, T2> ContinueWith<T3>(Func<T1, Either<T3, T2>> continueWith)
        {
            if (_left != null)
            {
                return continueWith(_left);
            }
            return Either<T3, T2>.Right(_right);
        }

        public Either<T1, T2> OnError(Func<T2, T1> onError)
        {
            _onError = onError;
            return this;
        }

        public T1 Result()
        {
            if (_left != null)
            {
                return _left;
            }
            return _onError(_right);
        }
    }
}