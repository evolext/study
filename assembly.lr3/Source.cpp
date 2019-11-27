#include <stdio.h>
#define EPS_LIMIT 1e-15

extern "C" int SOLVE(double eps, double *x);

int main()
{
    // ������� �������� �������
    double eps = .0;
    // ��������� �������
    double X = .0;
    // ���� ������
    printf("Enter eps: ");
    scanf("%lf", &eps);

    if (eps < 0)
        printf("error: negative value of eps\n");
    else if (eps < EPS_LIMIT)
        printf("error: too samll value of eps\n");
    else
    {
        int iter = SOLVE(eps, &X);
        printf("Found solve X = %lf at %d iterations\n", X, iter);
    }
    return 0;
}