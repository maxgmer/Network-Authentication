﻿//Copyright 2023 Kostasel
//See license.txt for license details

namespace NetworkAuth
{
    internal sealed class CryptoTransforms
    {
        private readonly static System.Random rnd = new(System.DateTime.Now.Millisecond);
        private int selected;
        private int selectedX;

        private static readonly byte[] prime0 = new byte[213] { 3, 67, 71, 33, 226, 102, 196, 11, 159, 18, 6, 102, 229, 39, 199, 98, 253, 41, 184, 110, 75, 180, 105, 114, 100, 130, 254, 248, 224, 130, 186, 127, 76, 140, 131, 199, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 235, 129, 45, 180, 79, 188, 176, 64, 171, 79, 89, 60, 13, 182, 161, 44, 36, 92, 167, 113, 113, 150, 119, 25, 70, 38, 221, 173, 121, 44, 171, 218, 3, 181, 173, 85, 237, 181, 236, 96, 69, 175, 57, 249, 137, 97, 182, 45, 125, 61, 53, 96, 81, 72, 229, 203, 77, 22, 245, 195, 184, 238, 88, 60, 58, 7, 216, 137, 28, 161, 101, 159, 112, 216, 161, 132, 117, 159, 97, 248, 60, 239, 105, 240, 229, 113, 29, 35, 207, 98, 191, 250, 66, 239, 126, 150, 30, 141, 141, 25, 201, 9, 1, 153, 167, 216, 231, 156, 130, 252, 6, 21, 229, 237, 8, 71, 142, 177, 181, 93, 86, 133, 146, 124, 179, 212, 209, 68, 147, 7, 125, 193, 92, 140, 125, 63, 214, 168, 115, 215, 252, 214, 212, 6, 209, 101, 180, 53, 244, 138, 212, 242, 23, 187, 155, 113, 245, 238, 68, 5, };
        private static readonly byte[] prime1 = new byte[213] { 211, 77, 219, 143, 174, 30, 191, 213, 65, 224, 226, 191, 4, 236, 110, 239, 4, 168, 125, 56, 56, 63, 216, 47, 125, 233, 115, 85, 140, 215, 79, 166, 245, 10, 158, 147, 58, 179, 163, 230, 162, 219, 92, 185, 133, 40, 61, 247, 121, 128, 80, 55, 124, 194, 251, 149, 224, 23, 196, 6, 38, 167, 16, 247, 250, 242, 193, 146, 48, 66, 16, 81, 185, 81, 73, 141, 169, 81, 75, 63, 255, 248, 169, 175, 122, 201, 207, 84, 17, 66, 83, 2, 19, 224, 113, 6, 84, 120, 0, 106, 83, 97, 32, 67, 147, 13, 222, 22, 105, 168, 166, 70, 36, 252, 26, 211, 88, 186, 86, 197, 17, 91, 252, 49, 113, 171, 180, 43, 61, 155, 164, 57, 179, 104, 30, 67, 61, 156, 122, 195, 148, 87, 31, 203, 73, 111, 121, 191, 21, 175, 42, 253, 47, 76, 139, 239, 155, 233, 12, 189, 38, 151, 137, 24, 201, 253, 155, 38, 142, 198, 77, 124, 64, 164, 67, 65, 92, 192, 69, 191, 93, 16, 74, 93, 184, 61, 14, 177, 1, 224, 68, 79, 72, 185, 174, 104, 38, 118, 235, 190, 100, 234, 0, 228, 161, 225, 251, 219, 23, 144, 3, 120, 5, };
        private static readonly byte[] prime2 = new byte[213] { 231, 197, 13, 198, 96, 244, 199, 11, 159, 82, 103, 158, 192, 130, 11, 83, 121, 241, 51, 18, 184, 116, 64, 143, 167, 56, 18, 53, 126, 90, 65, 37, 59, 83, 7, 200, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 235, 197, 242, 210, 85, 68, 90, 239, 178, 236, 33, 162, 132, 124, 154, 225, 151, 2, 171, 248, 10, 214, 40, 207, 141, 249, 235, 55, 115, 216, 40, 211, 60, 22, 195, 195, 143, 31, 3, 214, 186, 246, 244, 127, 9, 69, 208, 198, 61, 167, 144, 30, 63, 73, 76, 124, 207, 114, 180, 32, 234, 240, 78, 69, 150, 209, 186, 249, 143, 225, 236, 39, 35, 126, 144, 72, 134, 162, 135, 59, 214, 15, 199, 43, 9, 120, 219, 99, 11, 241, 177, 120, 168, 215, 103, 39, 86, 100, 77, 111, 197, 198, 34, 165, 178, 1, 251, 143, 3, 253, 174, 177, 52, 251, 141, 179, 36, 247, 199, 71, 105, 44, 237, 17, 242, 95, 162, 139, 222, 103, 86, 179, 24, 171, 10, 210, 98, 94, 172, 9, 119, 21, 56, 249, 172, 8, 156, 220, 29, 42, 40, 20, 190, 145, 142, 54, 229, 205, 84, 8, };
        private static readonly byte[] prime3 = new byte[213] { 157, 201, 13, 198, 96, 244, 199, 11, 159, 82, 103, 158, 192, 130, 11, 83, 121, 241, 51, 18, 184, 116, 64, 143, 167, 56, 18, 53, 126, 90, 65, 37, 59, 83, 7, 200, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 72, 101, 73, 252, 1, 244, 179, 26, 169, 101, 76, 105, 227, 91, 217, 55, 174, 71, 226, 130, 116, 54, 111, 205, 138, 170, 232, 163, 22, 69, 202, 73, 5, 127, 29, 124, 64, 50, 169, 144, 61, 222, 232, 180, 161, 236, 227, 214, 237, 86, 186, 14, 250, 95, 148, 214, 101, 104, 166, 13, 169, 175, 239, 193, 101, 221, 165, 224, 197, 136, 213, 148, 82, 191, 83, 107, 192, 174, 199, 57, 169, 190, 255, 141, 189, 149, 73, 225, 23, 206, 207, 134, 58, 22, 254, 99, 123, 0, 50, 23, 168, 247, 196, 117, 226, 107, 88, 66, 53, 169, 90, 95, 10, 188, 48, 134, 7, 130, 46, 236, 45, 244, 133, 57, 193, 109, 173, 43, 126, 7, 234, 119, 74, 197, 92, 170, 236, 51, 7, 228, 105, 153, 83, 105, 23, 76, 75, 59, 7, 165, 162, 132, 159, 9, 49, 10, };
        private static readonly byte[] prime4 = new byte[213] { 15, 72, 71, 33, 226, 102, 196, 11, 159, 18, 6, 102, 229, 39, 199, 98, 253, 41, 184, 110, 75, 180, 105, 114, 100, 130, 254, 248, 224, 130, 186, 127, 76, 140, 131, 199, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 72, 101, 73, 252, 1, 116, 189, 209, 218, 92, 253, 45, 218, 96, 119, 224, 170, 130, 40, 95, 71, 165, 28, 25, 97, 101, 230, 26, 16, 231, 8, 130, 137, 239, 52, 28, 135, 44, 197, 25, 141, 74, 166, 27, 171, 116, 24, 67, 186, 183, 12, 42, 88, 37, 173, 58, 9, 134, 156, 213, 231, 234, 57, 63, 197, 225, 240, 39, 140, 96, 140, 121, 220, 0, 144, 8, 24, 140, 127, 65, 98, 4, 227, 234, 247, 106, 202, 69, 116, 43, 247, 57, 198, 108, 85, 199, 43, 244, 85, 121, 94, 66, 239, 215, 7, 244, 50, 32, 48, 218, 54, 223, 174, 81, 85, 90, 91, 69, 247, 168, 80, 73, 250, 32, 168, 97, 84, 31, 150, 75, 244, 233, 208, 201, 97, 23, 225, 52, 209, 177, 234, 70, 136, 241, 8, 223, 52, 178, 60, 140, 226, 166, 57, 196, 17, 14, };
        private static readonly byte[] prime5 = new byte[213] { 235, 79, 219, 143, 174, 30, 191, 213, 65, 224, 226, 191, 4, 236, 110, 239, 4, 168, 125, 56, 56, 63, 216, 47, 125, 233, 115, 85, 140, 215, 79, 166, 245, 10, 158, 147, 58, 179, 163, 230, 162, 219, 92, 185, 133, 40, 61, 247, 121, 128, 80, 100, 121, 230, 33, 203, 252, 150, 4, 85, 101, 127, 22, 189, 0, 130, 250, 227, 85, 11, 243, 97, 93, 128, 151, 115, 90, 134, 56, 8, 17, 211, 246, 27, 117, 136, 197, 82, 155, 175, 241, 187, 169, 238, 15, 174, 161, 69, 249, 232, 138, 62, 166, 184, 150, 109, 91, 145, 182, 179, 163, 192, 99, 250, 119, 4, 230, 100, 128, 245, 70, 195, 145, 236, 28, 204, 187, 230, 110, 29, 189, 68, 154, 137, 136, 160, 252, 37, 21, 225, 141, 45, 190, 12, 246, 51, 165, 230, 83, 121, 49, 207, 141, 247, 167, 145, 96, 47, 167, 243, 124, 67, 46, 205, 107, 116, 162, 184, 244, 9, 139, 59, 23, 132, 69, 41, 193, 80, 32, 4, 232, 102, 121, 149, 183, 164, 84, 83, 10, 7, 78, 143, 220, 198, 75, 42, 51, 205, 203, 58, 171, 210, 202, 167, 165, 9, 121, 189, 180, 161, 199, 194, 16, };
        private static readonly byte[] prime6 = new byte[213] { 181, 68, 71, 33, 226, 102, 196, 11, 159, 18, 6, 102, 229, 39, 199, 98, 253, 41, 184, 110, 75, 180, 105, 114, 100, 130, 254, 248, 224, 130, 186, 127, 76, 140, 131, 199, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 72, 101, 73, 252, 1, 244, 131, 237, 201, 205, 199, 164, 211, 199, 27, 2, 172, 42, 128, 8, 72, 173, 147, 56, 122, 167, 18, 140, 120, 180, 67, 190, 37, 201, 150, 12, 118, 207, 69, 92, 47, 65, 228, 211, 217, 185, 37, 246, 219, 187, 49, 74, 170, 81, 54, 60, 216, 2, 99, 86, 7, 158, 165, 191, 12, 181, 33, 32, 13, 82, 32, 81, 73, 215, 170, 112, 216, 247, 180, 204, 114, 37, 32, 21, 95, 154, 158, 109, 149, 12, 230, 24, 203, 81, 17, 165, 179, 252, 74, 230, 49, 5, 169, 41, 223, 186, 80, 199, 183, 67, 225, 36, 145, 144, 29, 69, 112, 254, 153, 58, 75, 185, 9, 22, 229, 213, 35, 82, 180, 40, 135, 173, 242, 103, 10, 76, 219, 180, 202, 179, 130, 153, 50, 146, 152, 46, 187, 197, 155, 50, 139, 130, 193, 94, 230, 23, };
        private static readonly byte[] prime7 = new byte[213] { 105, 88, 219, 143, 174, 30, 191, 213, 65, 224, 226, 191, 4, 236, 110, 239, 4, 168, 125, 56, 56, 63, 216, 47, 125, 233, 115, 85, 140, 215, 79, 166, 245, 10, 158, 147, 58, 179, 163, 230, 162, 219, 92, 185, 133, 40, 61, 247, 121, 128, 80, 55, 124, 194, 251, 121, 55, 238, 25, 43, 45, 99, 222, 207, 219, 45, 73, 109, 16, 36, 118, 142, 45, 201, 206, 45, 201, 76, 203, 192, 81, 190, 127, 231, 88, 84, 196, 253, 76, 251, 225, 240, 159, 112, 175, 159, 109, 222, 110, 175, 74, 114, 149, 202, 176, 243, 12, 121, 80, 3, 41, 242, 90, 23, 166, 164, 193, 47, 85, 31, 24, 26, 37, 52, 214, 222, 194, 95, 218, 190, 251, 191, 116, 36, 226, 141, 43, 102, 82, 146, 29, 200, 74, 68, 144, 18, 19, 241, 128, 200, 53, 4, 10, 96, 16, 14, 165, 76, 104, 238, 135, 32, 28, 134, 44, 41, 114, 210, 172, 136, 25, 14, 154, 59, 190, 80, 248, 112, 194, 50, 90, 131, 16, 110, 117, 199, 6, 53, 211, 26, 212, 12, 196, 79, 26, 110, 31, 240, 155, 74, 114, 175, 100, 184, 179, 26, 56, 145, 116, 219, 240, 70, 25, };
        private static readonly byte[] prime8 = new byte[213] { 207, 72, 71, 33, 226, 102, 196, 11, 159, 18, 6, 102, 229, 39, 199, 98, 253, 41, 184, 110, 75, 180, 105, 114, 100, 130, 254, 248, 224, 130, 186, 127, 76, 140, 131, 199, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 72, 101, 73, 252, 1, 244, 177, 169, 23, 210, 234, 239, 186, 243, 216, 186, 183, 205, 167, 41, 34, 167, 224, 33, 122, 33, 177, 142, 197, 93, 187, 244, 253, 10, 57, 200, 16, 42, 93, 146, 146, 27, 198, 164, 80, 4, 250, 79, 69, 65, 194, 188, 70, 45, 185, 113, 142, 18, 20, 150, 22, 11, 111, 136, 171, 168, 38, 224, 82, 28, 11, 238, 244, 36, 225, 254, 240, 40, 41, 159, 148, 238, 40, 59, 43, 230, 25, 74, 135, 193, 65, 203, 211, 121, 94, 70, 175, 109, 180, 102, 33, 169, 246, 145, 240, 44, 58, 95, 7, 210, 170, 143, 216, 16, 71, 236, 43, 12, 34, 198, 233, 218, 201, 140, 91, 252, 194, 187, 102, 168, 7, 4, 75, 148, 57, 157, 112, 195, 133, 243, 4, 164, 146, 157, 90, 108, 189, 200, 153, 114, 178, 106, 27, 40, 89, 25, };
        private static readonly byte[] prime9 = new byte[213] { 145, 200, 13, 198, 96, 244, 199, 11, 159, 82, 103, 158, 192, 130, 11, 83, 121, 241, 51, 18, 184, 116, 64, 143, 167, 56, 18, 53, 126, 90, 65, 37, 59, 83, 7, 200, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 192, 232, 86, 201, 33, 207, 19, 200, 70, 144, 14, 110, 1, 92, 222, 234, 69, 206, 121, 243, 141, 218, 205, 205, 108, 21, 103, 98, 68, 124, 145, 98, 192, 180, 42, 249, 74, 50, 109, 236, 25, 91, 133, 75, 63, 140, 67, 233, 161, 167, 255, 135, 210, 60, 6, 26, 56, 8, 182, 210, 189, 220, 7, 62, 102, 241, 43, 190, 113, 19, 110, 201, 183, 120, 200, 80, 205, 44, 6, 51, 210, 190, 93, 61, 197, 214, 252, 43, 197, 45, 56, 95, 226, 102, 176, 153, 116, 123, 128, 91, 230, 59, 99, 109, 168, 45, 201, 172, 181, 18, 44, 41, 153, 49, 138, 159, 161, 39, 55, 37, 82, 143, 39, 46, 53, 59, 229, 78, 230, 13, 6, 241, 174, 19, 227, 39, 236, 11, 157, 140, 174, 191, 164, 87, 14, 69, 209, 19, 111, 124, 192, 169, 111, 30, 32, 26, };
        private static readonly byte[] prime10 = new byte[213] { 129, 203, 13, 198, 96, 244, 199, 11, 159, 82, 103, 158, 192, 130, 11, 83, 121, 241, 51, 18, 184, 116, 64, 143, 167, 56, 18, 53, 126, 90, 65, 37, 59, 83, 7, 200, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 80, 197, 107, 55, 114, 53, 72, 101, 73, 252, 1, 244, 3, 84, 124, 165, 250, 123, 69, 147, 244, 228, 255, 31, 168, 147, 189, 162, 182, 71, 187, 19, 165, 110, 174, 106, 10, 13, 243, 15, 81, 45, 229, 76, 110, 60, 187, 27, 69, 147, 5, 44, 70, 175, 26, 59, 28, 170, 109, 33, 148, 191, 83, 60, 110, 135, 2, 213, 219, 15, 171, 183, 102, 62, 166, 192, 76, 167, 55, 84, 32, 53, 112, 187, 97, 145, 123, 155, 11, 55, 55, 205, 53, 22, 150, 114, 198, 207, 128, 175, 30, 200, 112, 185, 48, 99, 115, 224, 186, 243, 214, 24, 249, 177, 41, 1, 176, 71, 80, 254, 197, 249, 18, 43, 62, 50, 67, 115, 200, 239, 119, 24, 207, 114, 106, 185, 39, 155, 78, 63, 52, 153, 179, 249, 139, 2, 57, 52, 141, 212, 140, 12, 22, 117, 161, 199, 209, 23, 42, 23, 189, 26, };
        private static readonly byte[] prime11 = new byte[213] { 105, 157, 75, 249, 164, 198, 173, 190, 81, 220, 4, 173, 182, 99, 204, 205, 83, 46, 143, 80, 40, 125, 124, 201, 82, 23, 167, 167, 38, 145, 4, 237, 211, 181, 222, 144, 233, 11, 45, 81, 240, 9, 161, 104, 11, 86, 101, 230, 11, 253, 251, 115, 2, 165, 171, 204, 212, 245, 211, 116, 189, 5, 44, 241, 241, 225, 224, 63, 38, 254, 224, 224, 189, 62, 2, 228, 225, 66, 164, 130, 188, 37, 184, 18, 116, 231, 148, 228, 187, 217, 41, 123, 27, 57, 123, 29, 203, 188, 172, 120, 234, 253, 102, 249, 211, 64, 22, 252, 80, 49, 120, 218, 40, 202, 27, 163, 234, 25, 158, 152, 230, 74, 175, 146, 220, 251, 95, 235, 17, 28, 60, 125, 149, 82, 50, 200, 3, 208, 228, 128, 6, 175, 83, 183, 242, 9, 62, 0, 90, 82, 209, 51, 42, 2, 94, 71, 197, 82, 22, 121, 14, 248, 198, 93, 104, 165, 35, 71, 232, 39, 199, 127, 253, 181, 93, 83, 40, 205, 63, 220, 228, 172, 54, 184, 149, 9, 96, 160, 73, 252, 253, 1, 30, 242, 40, 132, 94, 191, 66, 1, 160, 241, 167, 105, 90, 248, 24, 143, 133, 178, 81, 115, 27, };
        private static readonly byte[] prime12 = new byte[213] { 1, 94, 7, 233, 202, 244, 199, 11, 159, 82, 103, 158, 192, 130, 11, 83, 121, 241, 51, 18, 184, 116, 64, 143, 167, 56, 18, 53, 126, 90, 65, 37, 59, 83, 7, 200, 64, 110, 123, 141, 177, 62, 44, 117, 248, 105, 63, 186, 169, 81, 86, 58, 205, 239, 140, 67, 208, 163, 204, 79, 224, 149, 59, 87, 217, 106, 22, 214, 234, 176, 155, 141, 42, 47, 255, 139, 246, 83, 28, 207, 209, 166, 101, 89, 59, 197, 73, 239, 114, 153, 136, 94, 185, 99, 157, 48, 125, 58, 232, 204, 31, 113, 149, 76, 155, 155, 132, 118, 72, 86, 121, 118, 150, 57, 103, 11, 74, 134, 28, 31, 158, 203, 174, 16, 220, 145, 142, 168, 171, 239, 235, 198, 47, 241, 224, 76, 227, 26, 177, 191, 17, 38, 34, 37, 51, 240, 193, 99, 231, 235, 24, 250, 63, 150, 146, 185, 143, 228, 4, 61, 70, 28, 196, 180, 20, 19, 169, 101, 93, 36, 253, 205, 92, 91, 139, 94, 240, 29, 34, 44, 18, 175, 50, 197, 159, 182, 152, 162, 144, 135, 142, 247, 249, 175, 181, 209, 32, 114, 155, 207, 121, 116, 148, 184, 34, 116, 150, 8, 81, 114, 167, 225, 27, };
        private static readonly byte[] salt = new byte[256] { 184, 202, 180, 250, 138, 74, 124, 175, 64, 191, 108, 78, 221, 109, 163, 62, 67, 184, 202, 22, 42, 165, 116, 151, 53, 138, 109, 43, 150, 30, 105, 143, 161, 86, 251, 60, 149, 90, 120, 18, 40, 93, 137, 222, 160, 7, 39, 60, 29, 60, 222, 177, 73, 134, 247, 52, 184, 30, 66, 84, 12, 108, 144, 52, 124, 138, 241, 57, 235, 63, 214, 44, 65, 148, 179, 109, 242, 195, 101, 166, 2, 81, 80, 72, 121, 204, 175, 37, 181, 110, 195, 184, 71, 68, 167, 59, 61, 133, 51, 53, 76, 47, 62, 157, 59, 109, 99, 146, 166, 3, 177, 168, 15, 157, 15, 32, 162, 21, 165, 83, 46, 7, 255, 152, 76, 176, 170, 128, 212, 23, 243, 178, 179, 19, 29, 40, 202, 231, 148, 108, 144, 157, 229, 43, 173, 75, 219, 41, 37, 230, 177, 140, 43, 147, 58, 162, 169, 153, 78, 129, 29, 217, 209, 36, 173, 150, 99, 35, 251, 227, 62, 49, 116, 190, 179, 245, 223, 9, 17, 241, 58, 222, 138, 97, 246, 191, 6, 6, 212, 44, 164, 75, 184, 1, 87, 217, 160, 36, 85, 176, 38, 63, 227, 209, 179, 125, 132, 102, 96, 60, 31, 150, 183, 136, 11, 166, 27, 62, 212, 153, 158, 132, 129, 210, 113, 209, 201, 248, 25, 88, 126, 170, 242, 146, 190, 46, 143, 236, 128, 143, 176, 196, 237, 195, 191, 77, 94, 220, 140, 68, 254, 117, 169, 116, 18, 161 };
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CryptoTransforms()
        {
            selected = rnd.Next(6, 12);
            selectedX = rnd.Next(0, 5);
        }

        internal byte[] GetKeygenSalt()
        {
            return salt;
        }

        internal int GetRandomPrimeIndex()
        {
            return selected;
        }

        internal void Randomize()
        {
            selected = rnd.Next(1, 12);
            selectedX = rnd.Next(0, 5);
        }

        internal byte[] GetRandomPrime(int idx)
        {
            selected = idx;
            return GetRandomPrime();
        }
        internal byte[] GetRandomPrimeX(int idx)
        {
            selectedX = idx;
            return GetRandomPrimeX();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte[] GetRandomPrime()
        {
            if (selected == 6)
            {
                return prime6;
            }
            else if (selected == 7)
            {
                return prime7;
            }
            else if (selected == 8)
            {
                return prime8;
            }
            else if (selected == 9)
            {
                return prime9;
            }
            else if (selected == 10)
            {
                return prime10;
            }
            else if (selected == 11)
            {
                return prime11;
            }
            else if (selected == 12)
            {
                return prime12;
            }
            return null;
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte[] GetRandomPrimeX()
        {
            if (selectedX == 0)
            {
                return prime0;
            }
            else if (selectedX == 1)
            {
                return prime1;
            }
            else if (selectedX == 2)
            {
                return prime2;
            }
            else if (selectedX == 3)
            {
                return prime3;
            }
            else if (selectedX == 4)
            {
                return prime4;
            }
            else if (selectedX == 5)
            {
                return prime5;
            }
            return null;
        }
    }
}