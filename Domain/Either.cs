using System;

namespace HexagonalImpl.Domain
{
    public class Either<T1, T2>
    {
        private T1 _left;
        private T2 _right;

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

        internal void ContinueWith(Action<T1> onT1, Action<T2> onT2)
        {
            if (_left != null)
            {
                onT1(_left);
            }
            onT2(_right);
        }
    }

    public class EitherVoid<T>
    {
        internal void ContinueWith(Action onVoid, Action<T> onT)
        {

        }
    }
}