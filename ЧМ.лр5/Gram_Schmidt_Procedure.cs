namespace Com_Methods
{
    /// <summary>
    /// ортогонализация по методу Грама-Шмидта
    /// </summary>
    class Gram_Schmidt_Procedure
    {
        /// <summary>
        /// реализация классического алгоритма: предполагается, что память под матрицы выделена
        /// </summary>
        /// <param name="A - исходная матрица"></param>
        /// <param name="Q - ортогональная матрица преобразований"></param>
        /// <param name="R - результат"></param>
        public static void Classic_Gram_Schmidt_Procedure(Matrix A, Matrix Q, Matrix R)
        {
            //вспомогательный вектор
            var q_ = new Vector(A.M);

            //алгоритм классического метода Грама-Шмидта
            for (int j = 0; j < A.N; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < A.M; k++) R.Elem[i][j] += A.Elem[k][j] * Q.Elem[k][i];
                }

                q_.Copy_Column(A, j);

                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < q_.N; k++) q_.Elem[k] -= Q.Elem[k][i] * R.Elem[i][j];
                }

                R.Elem[j][j] = q_.Norma();

                //модуль не нужен
                if (R.Elem[j][j] < CONST.EPS) return;

                for (int i = 0; i < A.M; i++) Q.Elem[i][j] = q_.Elem[i] / R.Elem[j][j];
            }
        }

        /// <summary>
        /// реализация модифицированного алгоритма: предполагается, что память под матрицы выделена
        /// </summary>
        /// <param name="A - исходная матрица"></param>
        /// <param name="Q - ортогональная матрица преобразований"></param>
        /// <param name="R - результат"></param>
        public static void Modified_Gram_Schmidt_Procedure(Matrix A, Matrix Q, Matrix R)
        {
            //вспомогательный вектор
            var q_ = new Vector(A.M);

            //алгоритм модифицированного метода Грама-Шмидта
            for (int j = 0; j < A.N; j++)
            {
                q_.Copy_Column(A, j);

                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < q_.N; k++)
                    {
                        R.Elem[i][j] += q_.Elem[k] * Q.Elem[k][i];
                    }
                    for (int k = 0; k < q_.N; k++)
                    {
                        q_.Elem[k] -= R.Elem[i][j] * Q.Elem[k][i];
                    }
                }

                R.Elem[j][j] = q_.Norma();

                if (R.Elem[j][j] < CONST.EPS) return;

                for (int i = 0; i < A.M; i++) Q.Elem[i][j] = q_.Elem[i] / R.Elem[j][j];
            }
        }
    }
}
