using System;
using System.IO;

namespace Com_Methods
{
    // метод бисопряжённых градиентов
    class BiConjugate_Gradient_Method
    {

        //максимальное число итераций
        public int Max_Iter { set; get; }
        //точность решения
        public double Eps { set; get; }
        //текущая итерация 
        public int Iter { set; get; }
        //предобусловливатель
        public Preconditioner Preconditioner { set; get; }

        //конструктор
        public BiConjugate_Gradient_Method(int MAX_ITER, double EPS)
        {
            Max_Iter = MAX_ITER;
            Eps = EPS;
            Iter = 0;
        }

        //реализация метода
        public Vector Start_Solver(CSlR_Matrix A, Vector F, Preconditioner.Type_Preconditioner PREC)
        {
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\ЧМ.лр3(5 семестр)\ЧМ.лр3(5 семестр)\Output.txt"))
            {
                //реализация предобусловливателя
                switch (PREC)
                {
                    //диагональный
                    case Com_Methods.Preconditioner.Type_Preconditioner.Diagonal_Preconditioner:
                        { Preconditioner = new Diagonal_Preconditioner(A); break; }
                    //ILU(0) - декомпозиция
                    case Com_Methods.Preconditioner.Type_Preconditioner.LU_Decomposition:
                        { Preconditioner = new ILU_Preconditioner(A); break; }
                }

                //размер системы
                int n = A.N;
                //результат
                var RES = new Vector(n);

                // Истинное значение
                var X_true = new Vector(A.N);
                for (int i = 0; i < A.N; i++)
                {
                    X_true.Elem[i] = 1.0;
                }

                //вспомогательные векторы
                var r = new Vector(n);
                var p = new Vector(n);
                var r_ = new Vector(n);
                var p_ = new Vector(n);
                var vec = new Vector(n);
                var vec_ = new Vector(n);
                var vec_help = new Vector(n);

                //параметры метода
                double alpha, betta, sc1, sc2;
                bool Flag = true;

                //норма невязки
                double Norma_r = 0;

                //невязка r = M^(-1) * (F - Ax) 
                A.Mult_MV(RES, vec_help);
                for (int i = 0; i < n; i++)
                    vec_help.Elem[i] = F.Elem[i] - vec_help.Elem[i];
                Preconditioner.Start_Preconditioner(vec_help, r);

                //выбор начальных значений векторов метода BiCG
                for (int i = 0; i < n; i++)
                {
                    RES.Elem[i] = 0.0;
                    p.Elem[i] = r.Elem[i];
                    r_.Elem[i] = r.Elem[i];
                    p_.Elem[i] = r.Elem[i];
                }


                while (Flag && Iter < Max_Iter)
                {
                    //скалярное произведение sc1 = (r; r_)
                    sc1 = r * r_;
                    //vec_help = A * p
                    A.Mult_MV(p, vec_help);
                    //vec = M^(-1) * A * p
                    Preconditioner.Start_Preconditioner(vec_help, vec);
                    //(M^(-1) * A * p; p_)
                    sc2 = vec * p_;
                    //коэффициент линейного поиска
                    alpha = sc1 / sc2;
                    //определим новый результат и невязку
                    for (int i = 0; i < n; i++)
                    {
                        RES.Elem[i] += alpha * p.Elem[i];
                        r.Elem[i] -= alpha * vec.Elem[i];
                    }
                    //переводим вектор p_ в преобусловленную систему
                    Preconditioner.Start_Tr_Preconditioner(p_, vec_help);
                    //vec_ = A_t * M^(-t) * p_
                    A.Mult_MtV(vec_help, vec_);
                    //новая невязка r_
                    for (int i = 0; i < n; i++) r_.Elem[i] -= alpha * vec_.Elem[i];
                    //(r_new; (r_)_new)
                    sc2 = r * r_;
                    //коэффициент в выборе направления поиска на базе предыдущих
                    betta = sc2 / sc1;
                    //норма обычной невязки
                    Norma_r = r.Norma();
                    //проверка завершения итерационного процесса
                    if (Norma_r < Eps) Flag = false;
                    //новые направления поиска
                    if (Flag)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            p.Elem[i] = r.Elem[i] + betta * p.Elem[i];
                            p_.Elem[i] = r_.Elem[i] + betta * p_.Elem[i];
                        }
                    }

                    Iter++;


                    var help = RES - X_true;
                    double solve_eps = help.Norma() / X_true.Norma();

                    Sw.WriteLine("{0} {1}", Iter, Math.Log10(Norma_r));
                }
                return RES;
            }
        }
    }

    // Метод сопряженных градиентов
    class Conjugate_Gradient_Method
    {
        // Максимальное число итераций
        public int Max_Iter { set; get; }
        // Точность решения
        public double Eps { set; get; }
        // Текущая итерация 
        public int Iter { set; get; }
        // Предобусловливатель
        public Preconditioner Preconditioner { set; get; }

        // Конструктор
        public Conjugate_Gradient_Method(int MAX_ITER, double EPS)
        {
            Max_Iter = MAX_ITER;
            Eps = EPS;
            Iter = 0;
        }

        // Реализация метода
        public Vector Start_Solver(CSlR_Matrix A, Vector F, Preconditioner.Type_Preconditioner PREC)
        {
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\ЧМ.лр3(5 семестр)\ЧМ.лр3(5 семестр)\Output.txt"))
            {
                // Реализация предобусловливателя
                switch (PREC)
                {
                    // Диагональный
                    case Com_Methods.Preconditioner.Type_Preconditioner.Diagonal_Preconditioner:
                        { Preconditioner = new Diagonal_Preconditioner(A); break; }
                    // ILU(0) - декомпозиция
                    case Com_Methods.Preconditioner.Type_Preconditioner.LU_Decomposition:
                        { Preconditioner = new ILU_Preconditioner(A); break; }
                }

                // Размер системы
                int n = A.N;
                // Результат
                var RES = new Vector(n);

                // Истинное значение
                var X_true = new Vector(A.N);
                for (int i = 0; i < A.N; i++)
                {
                    X_true.Elem[i] = 1.0;
                }
                // Вспомогательные векторы
                var r = new Vector(n);
                var p = new Vector(n);
                var vec = new Vector(n);
                var vec_help = new Vector(n);

                // Параметры метода
                double alpha, betta, sc1, sc2;
                bool Flag = true;

                // Норма невязки
                double Norma_r = 0;

                // Невязка r = F - Ax 
                A.Mult_MV(RES, vec);
                for (int i = 0; i < n; i++)
                    r.Elem[i] = F.Elem[i] - vec.Elem[i];
                // p = M^(-1) * r
                Preconditioner.Start_Preconditioner(r, p);

                while (Flag && Iter < Max_Iter)
                {
                    // vec_help = M^(-1) * r
                    Preconditioner.Start_Preconditioner(r, vec_help);
                    // скалярное произведение sc1 = (M^(-1)*r,r)
                    sc1 = vec_help * r;
                    // vec_help = A * p
                    A.Mult_MV(p, vec_help);
                    // sc2 = (Ap,p)
                    sc2 = vec_help * p;
                    //коэффициент линейного поиска
                    alpha = sc1 / sc2;

                    //определим новый результат и невязку
                    for (int i = 0; i < n; i++)
                    {
                        RES.Elem[i] += alpha * p.Elem[i];
                        r.Elem[i] -= alpha * vec_help.Elem[i];
                    }
                    // M^(-1) * r
                    Preconditioner.Start_Preconditioner(r, vec_help);
                    // (M^(-1) * r, r)
                    sc2 = vec_help * r;
                    //коэффициент в выборе направления поиска на базе предыдущих
                    betta = sc2 / sc1;
                    //норма обычной невязки
                    Norma_r = r.Norma();
                    //проверка завершения итерационного процесса
                    if (Norma_r < Eps) Flag = false;
                    //новые направления поиска
                    if (Flag)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            p.Elem[i] = vec_help.Elem[i] + betta * p.Elem[i];
                        }
                    }

                    Iter++;

                    var help = RES - X_true;
                    double solve_eps = help.Norma() / X_true.Norma();

                    Sw.WriteLine("{0} {1}", Iter, Math.Log10(Norma_r));
                }
                return RES;
            }
        }
    }
}