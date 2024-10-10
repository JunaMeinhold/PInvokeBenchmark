cmake_minimum_required(VERSION 3.10)
project(CInteropExample C)

set(CMAKE_C_STANDARD 99)

# Create a shared library
add_library(add SHARED src/add.c)

# Specify output directory for the library
set_target_properties(add PROPERTIES
    OUTPUT_NAME "add" # This will create add.dll or libadd.so
    LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}
)
