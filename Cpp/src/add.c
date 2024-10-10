#include "add.h"

int add(int a, int b) {
    return a + b;
}

int str(char* str, int len)
{
    for (int i = 0; i < len; i++)
    {
        str[i] = (char)i;
    }
    return 1;
}