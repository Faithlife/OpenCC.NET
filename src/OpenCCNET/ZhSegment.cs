using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;

namespace OpenCCNET
{
    public static partial class ZhConverter
    {
        public static class ZhSegment
        {
            /// <summary>
            /// 语句分词
            /// </summary>
            public static Func<string, IEnumerable<string>> Segment = SegmentByJieba;

            /// <summary>
            /// jieba.NET分词
            /// </summary>
            public static JiebaSegmenter Jieba = new JiebaSegmenter();

            /// <summary>
            /// 利用jieba.NET分词
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            private static IEnumerable<string> SegmentByJieba(string text)
            {
                return Jieba.Cut(text);
            }

            /// <summary>
            /// 重置分词所用方法
            /// </summary>
            public static void ResetSegment()
            {
                Segment = SegmentByJieba;
                Jieba = new JiebaSegmenter();
            }
        }
    }
}