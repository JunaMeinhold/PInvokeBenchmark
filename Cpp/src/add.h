#ifndef ADD_H
#define ADD_H

#if defined _WIN32 || defined __CYGWIN__
#ifdef ADD_NO_EXPORT
#define API
#else
#define API __declspec(dllexport)
#endif
#else
#ifdef __GNUC__
#define API __attribute__((__visibility__("default")))
#else
#define API
#endif
#endif

#ifdef __cplusplus
extern "C"
{
#endif

    API int add(int a, int b);

    API int str(char *a, int len);

#ifdef __cplusplus
}
#endif

#endif // ADD_H
